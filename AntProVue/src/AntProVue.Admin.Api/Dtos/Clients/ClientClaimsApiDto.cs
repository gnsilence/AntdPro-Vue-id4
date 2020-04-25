using System.Collections.Generic;

namespace AntProVue.Admin.Api.Dtos.Clients
{
    public class ClientClaimsApiDto
    {
        public ClientClaimsApiDto()
        {
            ClientClaims = new List<ClientClaimApiDto>();
        }

        public List<ClientClaimApiDto> ClientClaims { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}





