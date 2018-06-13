using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Models
{
    public class DeviceResult
    {
        public int DeviceResultId { get; set; }

        [Index]
        [StringLength(32)]
        public string IMEI { get; set; }

        [Index]
        public DateTime RecvTime { get; set; }

        public virtual ICollection<DeviceParam> DeviceParams { get; set; }
    }
}