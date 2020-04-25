using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ABP.WebApi.Authorization;
using ABP.WebApi.Configuration.Dto;

namespace ABP.WebApi.Configuration
{
    [Authorize]
    public class ConfigurationAppService : WebApiAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }

        public virtual void test()
        {
            throw new System.NotImplementedException();
        }
    }
}
