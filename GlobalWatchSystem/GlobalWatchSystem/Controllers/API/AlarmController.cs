using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GlobalWatchSystem.Data.Repository;

namespace GlobalWatchSystem.Controllers.API
{
    public class AlarmController : ApiController
    {
        private readonly IAlarmRepository alarmRepository;
        private readonly IAreaRepository areaRepository;
        private readonly IDeviceRepository deviceRepository;

        public AlarmController()
        {
            areaRepository = new AreaRepository();
            deviceRepository = new DeviceRepository();
            alarmRepository = new AlarmRepository();
        }

        [HttpGet]
        public IEnumerable<Alarm> GetAlarmsInArea(int areaId)
        {
            IEnumerable<int> areas = areaRepository.GetChildAreas(areaId).Select(area => area.Id).ToList();
            List<int> deviceInAreas = deviceRepository.FindAll(dev => areas.Contains(dev.AreaId), dev => dev.Id).ToList();
            return alarmRepository
                .FindAll(alarm => deviceInAreas.Contains(alarm.DeviceId))
                .OrderByDescending(alarm => alarm.StartDttm)
                .Select(alarm => new Alarm
                {
                    DeviceId = alarm.DeviceId,
                    DeviceName = alarm.DeviceName,
                    AlarmType = alarm.AlarmType.ToString(),
                    StartDttm = alarm.StartDttm
                });
        }
    }

    public class Alarm
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string AlarmType { get; set; }
        public DateTime StartDttm { get; set; }
    }
}