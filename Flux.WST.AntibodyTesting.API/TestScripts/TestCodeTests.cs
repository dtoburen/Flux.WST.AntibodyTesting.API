using Flux.WST.AntibodyTesting.API.Common;
using Flux.WST.AntibodyTesting.API.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.TestScripts
{
    class TestCodeTests : ApiTestBase
    {
        [Test]
        public async Task GetTestCodes_Success()
        {
            // Set up an instance of an api specifying the IGetTestCodesService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetTestCodesService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "AUTO";
            bool resultsFound;
            var expectedData = new List<string> { "GRIFPANEL","GRIFPANEL2","ORTHOB","ORTHOC" };

            var result = await api.GetTestCodesAsync(siteCode, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

                // Validate a description is provided for each code.  Descriptions could change, so not checking for exact text.
                for (int i = 0; i < result.Content.Result.Count; i++)
                {
                    Assert.IsNotNull(result.Content.Result[i].Description.Trim());
                }

                // Validate the expected data is in the returned result.
                foreach (string testCode in expectedData)
                {
                    resultsFound = false;
                    for (int i = 0; i < result.Content.Result.Count; i++)
                    {
                        if (result.Content.Result[i].Code.Trim() == testCode)
                        {
                            resultsFound = true;
                        }
                    }
                    Assert.AreEqual(true, resultsFound);
                }
            });

        }

        [Test]
        public async Task GetTestCodes_NotFound()
        {
            // Set up an instance of an api specifying the IGetTestCodesService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetTestCodesService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "";

            var result = await api.GetTestCodesAsync(siteCode, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
                Assert.IsNull(result.Content);
            });

        }
    }
}
