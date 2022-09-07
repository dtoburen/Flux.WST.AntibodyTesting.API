using Flux.API.Helpers;

namespace Flux.WST.AntibodyTesting.API.Common
{
    public class ApiTestBase
    {
        protected T SetupApiRequestOkta<T>(string authFile) where T : class
        {
            var restApi = AuthConfigHelper.ConfigureServiceWithOkta<T>(authFile);

            return restApi;
        }
    }
}
