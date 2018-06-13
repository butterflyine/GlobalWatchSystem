using System;
using System.Security.Principal;
using System.Web.Security;

namespace GlobalWatchSystem.Security
{
    public class CustomIdentity : IIdentity
    {
        private readonly IIdentity identity;
        private readonly CustomMembershipUser user;

        public CustomIdentity(IIdentity identity)
        {
            this.identity = identity;
            user = (CustomMembershipUser) Membership.GetUser(identity.Name);
            Id = user.Id;
            Phone = user.Phone;
            Email = user.Email;
            AreaId = user.AreaId;
        }

        public int Id { get; private set; }
        public String Phone { get; private set; }
        public String Email { get; private set; }
        public int AreaId { get; set; }

        public bool IsAdmin
        {
            get { return user.IsAdmin; }
        }

        public string Name
        {
            get { return identity.Name; }
        }

        public string AuthenticationType
        {
            get { return identity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return identity.IsAuthenticated; }
        }
    }
}