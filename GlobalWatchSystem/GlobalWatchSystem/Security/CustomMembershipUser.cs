using System;
using System.Web.Security;
using GlobalWatchSystem.Models;

namespace GlobalWatchSystem.Security
{
    public class CustomMembershipUser : MembershipUser
    {
        public CustomMembershipUser(User user)
            : base("CustomMembershipProvider", user.UserName, user.Id, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Phone = user.Phone;
            Id = user.Id;
            AreaId = user.AreaId;
        }

        public int Id { get; private set; }
        public String Phone { get; private set; }
        public int AreaId { get; set; }

        public bool IsAdmin
        {
            get { return UserName.Equals("admin", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}