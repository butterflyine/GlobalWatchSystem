using System;
using System.ComponentModel.DataAnnotations;

namespace GlobalWatchSystem.Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "Password")]
        public String Password { get; set; }
    }
}