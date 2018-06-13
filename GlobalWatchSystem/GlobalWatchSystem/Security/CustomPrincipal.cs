using System.Security.Principal;

namespace GlobalWatchSystem.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private readonly CustomIdentity identity;

        public CustomPrincipal(CustomIdentity identity)
        {
            this.identity = identity;
        }

        public bool IsInRole(string role)
        {
            return identity.Name.Equals("admin");
        }

        public IIdentity Identity { get { return identity;} }
    }
}