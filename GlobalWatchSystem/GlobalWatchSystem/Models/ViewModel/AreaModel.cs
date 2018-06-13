using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalWatchSystem.Models.ViewModel
{
    public class AreaModel
    {
        [Index]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "AreaName")]
        public String Name { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Description")]
        public String Description { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "AreaParent")]
        public String ParentId { get; set; }

        public String ParentArea { get; set; }
    }
}