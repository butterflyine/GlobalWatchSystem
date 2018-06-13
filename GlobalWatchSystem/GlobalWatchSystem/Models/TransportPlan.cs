using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Models
{
    public class TransportPlan
    {
        [Key]
        [Index]
        public int Id { get; set; }

        [Index]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "MenuPlanManagement")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true, NullDisplayText="尚未开始")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "StartTime")]
        public DateTime? startTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true, NullDisplayText = "尚未结束")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "EndTime")]
        public DateTime? stopTime { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Description")]
        public string Remark { get; set; }
    }
}