using System;
using System.ComponentModel.DataAnnotations;

namespace GlobalWatchSystem.Models.ViewModel
{
    public class DeviceModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "DeviceName")]
        [StringLength(100,MinimumLength = 5,ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Description")]
        [StringLength(255, MinimumLength = 0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "DeviceIMEI")]
        [StringLength(15, MinimumLength = 15, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldExactLength")]
        public string IMEI { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DevicePower")]
        public float Battery { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DevicePowerMode")]
        public PowerMode PowerMode { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DeviceNumber")]
        [StringLength(15, MinimumLength = 15, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldExactLength")]
        public string SimNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "AreaBelonged")]
        public int AreaId { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Longitude")]
        [Range(-180,180)]
        public float longitude { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Latitude")]
        [Range(-90,90)]
        public float latitude { get; set; }


        public String AreaName { get; set; }
        public float Humidity { get; set; }
        public float Temperature { get; set; }
        public DateTime? DataDttm { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "MenuPlanManagement")]
        public int? TransportPlanId { get; set; }

        public TransportPlan TransportPlan { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DeviceType")]
        public int? DeviceMetaId { get; set; }

    }
}