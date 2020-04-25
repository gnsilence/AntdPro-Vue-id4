using System.Collections.Generic;
using AntProVue.Admin.Configuration.Identity;

namespace AntProVue.Admin.Configuration.IdentityServer
{
    public class Client : global::IdentityServer4.Models.Client
    {
        public List<Claim> ClientClaims { get; set; } = new List<Claim>();
    }
}






