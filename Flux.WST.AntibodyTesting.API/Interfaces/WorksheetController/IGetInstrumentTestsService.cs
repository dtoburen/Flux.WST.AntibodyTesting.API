using Flux.WST.AntibodyTesting.API.Common;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.Interfaces
{
    interface IGetInstrumentTestsService
    {
        [Headers("Authorization: Bearer")]
        [Get("/v1/sites/{siteCode}/Worksheet/GetInstrumentTests")]
        Task<ApiResponse<ApiResultList<InstrumentTest>>> GetInstrumentTestsAsync(string siteCode, [FromQuery] string sortBy, [FromQuery] string sortDirection,
                [FromQuery] int pageIndex, [FromQuery] int pageSize, CancellationToken token);
    }
}
