using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Models
{
    public class DevicePlanRecord
    {
        [Column("Id")]
        public int Id { get; set; }

        
        public int? PlanId { get; set; }

        public int? DeviceId { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
    }
}