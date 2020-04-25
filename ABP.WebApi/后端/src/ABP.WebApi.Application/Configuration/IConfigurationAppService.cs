using System.Threading.Tasks;
using ABP.WebApi.Configuration.Dto;

namespace ABP.WebApi.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
        void test();
    }
}
