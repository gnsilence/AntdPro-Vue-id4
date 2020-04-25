using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ABP.WebApi.Authorization
{
  public  interface IAuthorizationHelper
    {
        Task AuthorizeAsync(IEnumerable<IAuthorizeAttribute> authorizeAttributes);

        Task AuthorizeAsync(MethodInfo methodInfo, Type type);
    }
}
