using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABP.WebApi.Api.Jobs.Dtos
{
   public class ReturnMsg
    {
        public string Errorcode { get; set; }

        public string Message { get; set; }
    }
}
