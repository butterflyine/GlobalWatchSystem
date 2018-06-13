using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalWatchSystem.Models
{
    public class User
    {
        [Index]
        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [StringLength(50, MinimumLength = 5, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "UserName")]
        public String UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [StringLength(50, MinimumLength = 5, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        [RegularExpression(@"^[_A-Za-z0-9-]+(.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(.[A-Za-z0-9]+)*(.[A-Za-z]{2,})$", ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldEmail")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Email")]
        public String Email { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [RegularExpression(@"[0-9]{8,20}", ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldPhone")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Phone")]
        public String Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [StringLength(256, MinimumLength = 5, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldLength")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "Password")]
        public String Password { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldRequired")]
        [Compare("Password", ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "MsgFieldConfirmPassword")]
        [Display(ResourceType = typeof (Resources.Resources), Name = "PasswordConfirm")]
        [NotMapped]
        public String ConfirmPassword { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "AreaBelonged")]
        public Int32 AreaId { get; set; }

        [Index]
        public int Id { get; set; }
    }
}