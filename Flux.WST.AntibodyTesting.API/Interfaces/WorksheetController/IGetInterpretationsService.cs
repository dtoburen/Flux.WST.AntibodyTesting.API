using Flux.WST.AntibodyTesting.API.Common;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.Interfaces
{
    interface IGetInterpretationsService
    {
        [Headers("Authorization: Bearer")]
        [Get("/v1/sites/{siteCode}/Worksheet/Interpretations")]
        Task<ApiResponse<ApiResultList<Interpretation>>> GetInterpretationsAsync(string siteCode, CancellationToken token);
    }
}
