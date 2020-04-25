using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.Api.Jobs.Dtos
{
    public class JobListPagedRequestDto : ListPagedRequestDto
    {
        public string Vague { get; set; }
    }
}
