using Flux.WST.AntibodyTesting.API.Common;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.Interfaces
{
    interface IGetOverrideReasonsService
    {
        [Headers("Authorization: Bearer")]
        [Get("/v1/sites/{siteCode}/Worksheet/OverrideReasons")]
        Task<ApiResponse<ApiResultList<OverrideReason>>> GetOverrideReasonsAsync(string siteCode, [FromQuery] string[] reasonTypes, CancellationToken token);
    }
}
