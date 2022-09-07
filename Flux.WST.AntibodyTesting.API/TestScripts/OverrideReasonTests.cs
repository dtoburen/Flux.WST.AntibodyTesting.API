using Flux.WST.AntibodyTesting.API.Common;
using Flux.WST.AntibodyTesting.API.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.TestScripts
{
    class OverrideReasonTests : ApiTestBase
    {
        [Test]
        public async Task GetOverrideReasons_FreeText_Success()
        {
            // Set up an instance of an api specifying the IGetOverrideReasonsService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetOverrideReasonsService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "AUTO";
            string[] reasonTypes = { "F" };

            var result = await api.GetOverrideReasonsAsync(siteCode, reasonTypes, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual("FT", result.Content.Result[0].Code);
                Assert.AreEqual("Free Text", result.Content.Result[0].Description);
            });

        }

        [Test]
        public async Task GetOverrideReasons_All_Success()
        {
            // Set up an instance of an api specifying the IGetOverrideReasonsService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetOverrideReasonsService>(AuthCredentials.TransfusionClientCred);

            // ***** UNABLE TO SEND MORE THAN 1 REASON TYPE AT A TIME.  NO RESULTS FOUND. *****
            string siteCode = "AUTO";
            string[] reasonTypes = { "F", "P" };
            bool resultsFound;
            var expectedData = new List<string> { "ERRORP","FT","MISC","NLN","PEI","PI" };

            var result = await api.GetOverrideReasonsAsync(siteCode, reasonTypes, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

                // Validate a description is provided for each code.  Descriptions could change, so not checking for exact text.
                for (int i = 0; i < result.Content.Result.Count; i++)
                {
                    Assert.IsNotNull(result.Content.Result[i].Description.Trim());
                }
     
                // Validate the minimum expected data is in the returned result.
                foreach (string OverrideReasonCode in expectedData)
                {
                    resultsFound = false;
                    for (int i = 0; i < result.Content.Result.Count; i++)
                    {
                        if (result.Content.Result[i].Code.Trim() == OverrideReasonCode)
                        {
                            resultsFound = true;
                        }
                    }
                    Assert.AreEqual(true, resultsFound);
                }
            });

        }

        [Test]
        public async Task GetOverrideReasons_NotFound()
        {
            // Set up an instance of an api specifying the IGetOverrideReasonsService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetOverrideReasonsService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "";
            string[] reasonTypes = { "F", "P" };

            var result = await api.GetOverrideReasonsAsync(siteCode, reasonTypes, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
                Assert.IsNull(result.Content);
            });

        }
    }
}
