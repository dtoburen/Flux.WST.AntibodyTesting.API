using Flux.WST.AntibodyTesting.API.Common;
using Flux.WST.AntibodyTesting.API.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.TestScripts
{
    class InterpretationTests : ApiTestBase
    {
        [Test]
        public async Task GetInterpretations_Success()
        {
            // Set up an instance of an api specifying the IGetInterpretationsService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetInterpretationsService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "AUTO";
            bool resultsFound;
            var expectedData = new List<string> { "A1","C","DPASSIVE","Jka","LC","M COLD" };

            var result = await api.GetInterpretationsAsync(siteCode, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

                // Validate a description is provided for each code.  Descriptions could change, so not checking for exact text.
                for (int i = 0; i < result.Content.Result.Count; i++)
                {
                    Assert.IsNotNull(result.Content.Result[i].Description.Trim());
                }

                // Validate the minimum expected data is in the returned result.
                foreach (string interpretationCode in expectedData)
                {
                    resultsFound = false;
                    for (int i = 0; i < result.Content.Result.Count; i++)
                    {
                        if (result.Content.Result[i].Code.Trim() == interpretationCode)
                        {
                            resultsFound = true;
                        }
                    }
                    Assert.AreEqual(true, resultsFound);
                }
            });

        }

        [Test]
        public async Task GetInterpretations_BadRequest()
        {
            // Set up an instance of an api specifying the IGetInterpretationsService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetInterpretationsService>(AuthCredentials.TransfusionClientCredInvalid);

            string siteCode = "AUTO";

            var result = await api.GetInterpretationsAsync(siteCode, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
                Assert.IsNull(result.Content);
            });

        }
    }
}
