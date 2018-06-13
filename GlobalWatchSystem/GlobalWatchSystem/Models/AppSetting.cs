using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalWatchSystem.Models
{
    [Table("AppSetting")]
    public class AppSetting
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "TemperatureThresholdUpper")]
        public float TemperatureUpper { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "TemperatureThresholdLower")]
        public float TemperatureLower { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "HumidityThresholdUpper")]
        public float HumidityUpper { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "HumidityThresholdLower")]
        public float HumidityLower { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "BatteryThreshold")]
        public float Battery { get; set; }
    }
}