using System;
using System.Collections.Generic;
using System.Linq;
using GlobalWatchSystem.Data;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using NLog;
using Quartz;

namespace GlobalWatchSystem.Services.Alarm
{
    public class AlarmScanJob : IJob
    {
        private readonly AppSettingRepository appSettingRepository;
        private readonly DeviceRepository deviceRepository;
        private readonly DeviceDataService dtuDataService;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private AppSetting appSetting;
        private IList<Device> devices;

        public AlarmScanJob()
        {
            deviceRepository = new DeviceRepository();
            appSettingRepository = new AppSettingRepository();
            dtuDataService = new DeviceDataService();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DoAlarmScan();
            }
            catch (Exception e)
            {
                logger.Error("Error occurred in AlarmScanJob: {0}", e.Message);
                logger.Error(e.StackTrace);
            }
        }

        private void DoAlarmScan()
        {
            appSetting = appSettingRepository.GetAll().First();
            devices = deviceRepository.GetAll().ToList();

            IEnumerable<DtuData> unProcessedDtuData = dtuDataService.GetLatestData(TimeSpan.FromMinutes(JobScheduler.ScanIntervalMinutes));
            foreach (DtuData dtuData in unProcessedDtuData)
            {
                Device device = FindDevice(dtuData.IMEI);
                if (device == null) return;

                float value = dtuData.Value;
                if (dtuData.IsTemperature && dtuData.Channel < 2)
                {
                    ProcessData(device, () => value > appSetting.TemperatureUpper || value < appSetting.TemperatureLower, AlarmType.Temperature);
                }
                else if (dtuData.IsHumidity && dtuData.Channel < 2)
                {
                    ProcessData(device, () => value < appSetting.TemperatureLower || value > appSetting.HumidityUpper, AlarmType.Humidity);
                }
            }

            foreach (Device device in devices)
            {
                float battary = device.Battery;
                if (battary > 0)
                {
                    ProcessData(device, () => battary <= appSetting.Battery, AlarmType.Battery);
                }
            }
        }

        private void ProcessData(Device device, Func<bool> alarmEvaluator, AlarmType alarmType)
        {
            if (alarmEvaluator())
            {
                SetDeviceOnAlarm(device, alarmType);
            }
            else
            {
                CancelDeviceAlarm(device, alarmType);
            }
        }

        private void CancelDeviceAlarm(Device device, AlarmType alarmType)
        {
            using (var ctx = new DatabaseContext())
            {
                DeviceAlarm deviceAlarm = ctx.Alarms.FirstOrDefault(alarm => alarm.DeviceId == device.Id && alarm.AlarmType == alarmType);
                if (deviceAlarm != null)
                {
                    ctx.Alarms.Remove(deviceAlarm);
                    ctx.SaveChanges();

                    logger.Info("Device {0}'s {1} returned to normal status.", device.Name, alarmType);
                }
            }
        }

        private void SetDeviceOnAlarm(Device device, AlarmType alarmType)
        {
            using (var ctx = new DatabaseContext())
            {
                DeviceAlarm deviceAlarm = ctx.Alarms.FirstOrDefault(alarm => alarm.DeviceId == device.Id && alarm.AlarmType == alarmType);
                if (deviceAlarm == null)
                {
                    var alarm = new DeviceAlarm
                    {
                        AlarmType = alarmType,
                        DeviceId = device.Id,
                        DeviceName = device.Name,
                        StartDttm = DateTime.Now
                    };
                    ctx.Alarms.Add(alarm);
                    ctx.SaveChanges();

                    logger.Info("Device {0}'s {1} is in alarm status.", device.Name, alarmType);
                }
            }
        }

        private Device FindDevice(string imei)
        {
            if (devices == null || !devices.Any())
            {
                devices = deviceRepository.GetAll().OrderByDescending(device => device.IMEI).ToList();
            }

            return devices.FirstOrDefault(device => device.IMEI.Equals(imei));
        }
    }
}