using Flux.WST.AntibodyTesting.API.Common;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.Interfaces
{
    interface IGetTestCodesService
    {
        [Headers("Authorization: Bearer")]
        [Get("/v1/sites/{siteCode}/Worksheet/TestCodes")]
        Task<ApiResponse<ApiResultList<TestCode>>> GetTestCodesAsync(string siteCode, CancellationToken token);
    }
}
