using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalWatchSystem.Services.Alarm
{
    public class DeviceAlarm
    {
        [Column(Order = 0), Key]
        public int DeviceId { get; set; }

        [Column(Order = 1), Key]
        public AlarmType AlarmType { get; set; }

        public String DeviceName { get; set; }
        public DateTime StartDttm { get; set; }
    }

    public enum AlarmType
    {
        Temperature = 0,
        Humidity = 1,
        Battery = 2
    }
}