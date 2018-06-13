using System;
using System.Collections.Generic;
using System.Linq;
using GlobalWatchSystem.Services.Alarm;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IAlarmRepository
    {
        IEnumerable<DeviceAlarm> GetAll();

        DeviceAlarm Get(int deviceId, AlarmType alarmType);

        IEnumerable<DeviceAlarm> FindAll(Func<DeviceAlarm, bool> filter);
    }

    public class AlarmRepository : IAlarmRepository
    {
        public IEnumerable<DeviceAlarm> GetAll()
        {
            using (var ctx = new DatabaseContext())
            {
                return ctx.Alarms.ToList();
            }
        }

        public DeviceAlarm Get(int deviceId, AlarmType alarmType)
        {
            using (var ctx = new DatabaseContext())
            {
                return ctx.Alarms.FirstOrDefault(alarm => alarm.DeviceId == deviceId && alarm.AlarmType == alarmType);
            }
        }

        public IEnumerable<DeviceAlarm> FindAll(Func<DeviceAlarm, bool> filter)
        {
            using (var ctx = new DatabaseContext())
            {
                return ctx.Alarms.Where(filter).ToList();
            }
        }
    }
}