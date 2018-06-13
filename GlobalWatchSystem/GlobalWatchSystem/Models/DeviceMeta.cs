using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Models
{
    public class DeviceMeta
    {
        public DeviceMeta()
        {
            this.CanLocation = true;
        }

        [Index]
        public int Id { get; set; }
        [StringLength(32)]
        public string DeviceType { get; set; }

        public bool CanLocation { get; set; }

        [StringLength(128)]
        public string MetaContent { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}