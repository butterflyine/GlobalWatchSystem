using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;

namespace GlobalWatchSystem.Controllers.API
{
    public class DeviceDataController : ApiController
    {
        private const float BatteryUpper = 4.2f;
        private const float BatteryLower = 3.6f;
        private readonly AppSetting appSetting;
        private readonly IDeviceRepository deviceRepository;

        public DeviceDataController()
        {
            deviceRepository = new DeviceRepository();
            appSetting = new AppSettingRepository().GetAll().First();
        }

        public IEnumerable<DeviceStatus> GetDeviceStatus(String ids)
        {
            IEnumerable<int> deviceIds = ids.Split('-').ToList().Select(id => Convert.ToInt32(id));
            return deviceRepository
                .FindAll(device => deviceIds.Contains(device.Id))
                .Select(dev => new DeviceStatus
                {
                    Id = dev.Id,
                    Name = dev.Name,
                    LastUpdate = dev.DataDttm.ToString(),
                    PowerMode = dev.PowerMode.ToString(),
                    OnLine = dev.DataDttm.HasValue && dev.DataDttm.Value.AddHours(1) > DateTime.Now,
                    Data = ParseData(dev)
                });
        }

        private Dictionary<string, List<Entry>> ParseData(Device dev)
        {
            var result = new Dictionary<String, List<Entry>>();

            float batteryLeft = ComputeBattery(dev);
            result.Add("Power", new List<Entry> {new Entry {Value = batteryLeft.ToString("0.00") + "%", InAlarm = batteryLeft < appSetting.Battery}});
            result.Add("Temperature", dev.GetData()
                .OrderBy(data => data.Channel)
                .Where(data => data.Type == DataType.Temperature)
                .Select(data => new Entry {Value = data.Value.ToString(), InAlarm = DeviceInAlarm(data, appSetting.TemperatureLower, appSetting.TemperatureUpper)}).ToList());
            result.Add("Humidity", dev.GetData()
                .OrderBy(data => data.Channel)
                .Where(data => data.Type == DataType.Humidity)
                .Select(data => new Entry {Value = data.Value.ToString(), InAlarm = DeviceInAlarm(data, appSetting.HumidityLower, appSetting.HumidityUpper)}).ToList());
            return result;
        }

        private Boolean DeviceInAlarm(DataEntry data, float lower, float upper)
        {
            // Only check alarm status for the 1st and 2nd channel.
            if (data.Channel > 2) return false;
            return data.Value < lower || data.Value > upper;
        }

        private float ComputeBattery(Device dev)
        {
            if (dev.PowerMode == PowerMode.Power) return 100.0f;
            if (dev.Battery >= BatteryUpper || Math.Abs(dev.Battery) < 0.01f) return 100.0f;
            if (dev.Battery <= BatteryLower) return 0.0f;
            return (dev.Battery - BatteryLower)*100/(BatteryUpper - BatteryLower);
        }
    }

    public class DeviceStatus
    {
        public Dictionary<String, List<Entry>> Data;
        public int Id { get; set; }
        public String Name { get; set; }
        public String PowerMode { get; set; }
        public Boolean OnLine { get; set; }
        public String LastUpdate { get; set; }
    }

    public class Entry
    {
        public String Value { get; set; }
        public Boolean InAlarm { get; set; }
    }
}