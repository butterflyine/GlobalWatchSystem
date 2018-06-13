using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Models
{
    public class DeviceParam
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeviceResultId { get; set; }

        [Column("dev_channel")]
        public int Channel { get; set; }

        [Column("data_type")]
        public int UnitCode { get; set; }

        [Column("data_value")]
        public float Value { get; set; }

        public virtual DeviceResult DeviceResult { get; set; }
    }
}