using Flux.WST.AntibodyTesting.API.Common;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.Interfaces
{
    interface IGetInstrumentNumbersService
    {
        [Headers("Authorization: Bearer")]
        [Get("/v1/sites/{siteCode}/Worksheet/InstrumentNumbers")]
        Task<ApiResponse<ApiResultList<Instrument>>> GetInstrumentNumbersAsync(string siteCode, CancellationToken token);
    }
}
