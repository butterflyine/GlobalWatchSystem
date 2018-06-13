using System;
using System.ComponentModel.DataAnnotations;

namespace GlobalWatchSystem.Models.ViewModel
{
    public class ChangePasswordModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [StringLength(50, MinimumLength = 5, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "UserName")]
        public String UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [StringLength(256, MinimumLength = 5, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Password")]
        public String Password { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Compare("Password", ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldConfirmPassword")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "PasswordConfirm")]
        public String ConfirmPassword { get; set; }
    }
}