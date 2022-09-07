using Flux.WST.AntibodyTesting.API.Common;
using Flux.WST.AntibodyTesting.API.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.WST.AntibodyTesting.API.TestScripts
{
    class InstrumentTests : ApiTestBase
    {
        [Test]
        public async Task GetInstruments_Success()
        {
            // Set up an instance of an api specifying the IGetInstrumentsService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetInstrumentNumbersService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "AUTO";
            bool resultsFound;
            var expectedData = new List<string> { "DFT" };

            var result = await api.GetInstrumentNumbersAsync(siteCode, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.IsNotNull(result.Content.Result[0].Description);

                // Validate the minimum expected data is in the returned result.
                foreach (string instrumentCode in expectedData)
                {
                    resultsFound = false;
                    for (int i = 0; i < result.Content.Result.Count; i++)
                    {
                        if (result.Content.Result[i].Code.Trim() == instrumentCode)
                        {
                            resultsFound = true;
                        }
                    }
                    Assert.AreEqual(true, resultsFound);
                }
            });

        }

        [Test]
        public async Task GetInstruments_NotFound()
        {
            // Set up an instance of an api specifying the IGetInstrumentNumbersService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetInstrumentNumbersService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "";

            var result = await api.GetInstrumentNumbersAsync(siteCode, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
                Assert.IsNull(result.Content);
            });

        }

        [Test]
        public async Task GetInstrumentTests_Success()
        {
            // Set up an instance of an api specifying the IGetInstrumentTestsService type and passing in the authentication credentials
            var api = SetupApiRequestOkta<IGetInstrumentTestsService>(AuthCredentials.TransfusionClientCred);

            string siteCode = "AUTO";
            bool resultsFound;
            string sortBy = "A";
            string sortDirection = "A";
            int pageIndex = 0;
            int pageSize = 1;

            DBConnection dBConnection = new DBConnection();
            string selectSQL;
            string[] sqlReturn;

            // ***** REPLACE THIS HARD CODED PATIENT WITH A DYNAMIC PATIENT CREATED OR LEAST FROM A BUILDER *****
            int patientNumber = 87762;
            string medicalRecordNumber = "1010";
            string patientName = "Bell, Taco";

            string specimenNumber = null;
            int antibodyInternalTestNumber = 0;
            string testCode = "ORTHOB";
            string testStatus = "Performed, Not Verified";

            selectSQL = "DECLARE @specimenNumCreated VARCHAR(20) ";
            selectSQL = selectSQL + "EXEC WST_ADD_SPECIMEN_INTERNAL @medRecNum = '" + medicalRecordNumber + "', @specimenStatus = 'I', ";
            selectSQL = selectSQL + "@daysUntilExpiration = 3, @facilityCode = '" + siteCode + "', @userName = 'SQA3', ";
            selectSQL = selectSQL + "@techID = 'T03', @specimenNum = @specimenNumCreated OUTPUT ";
            selectSQL = selectSQL + "SELECT @SpecimenNumCreated";
            sqlReturn = dBConnection.ExecuteSQL(selectSQL);

            if (string.IsNullOrEmpty(sqlReturn[0]) || sqlReturn.Length == 0)
            {
                Assert.Fail("Failed to insert a new specimen into the DB for seed data");
            }
            else
            {
                specimenNumber = sqlReturn[0].Trim();
            }

            selectSQL = "DECLARE @internalTestNum INT ";
            selectSQL = selectSQL + "EXEC WST_ADD_ORDERS_AND_TESTS_INTERNAL @medRecNum = '" + medicalRecordNumber + "', ";
            selectSQL = selectSQL + "@orderCode = '" + testCode + "', @facilityCode = '" + siteCode + "', @userName = 'SQA3', ";
            selectSQL = selectSQL + "@techID = 'T03', @intnlTstNum = @internalTestNum OUTPUT ";
            selectSQL = selectSQL + "SELECT @internalTestNum";
            sqlReturn = dBConnection.ExecuteSQL(selectSQL);

            if (string.IsNullOrEmpty(sqlReturn[0]) || sqlReturn.Length == 0)
            {
                Assert.Fail("Failed to insert a new order/test into the DB for seed data");
            }
            else
            {
                antibodyInternalTestNumber = Int32.Parse(sqlReturn[0]);
            }

            // Update the new Test Master row, to mimic that the results were performed by an automated instrument.
            selectSQL = "update TST_MST set TST_STAT_CD = 'P', INSMT_TST_RSLT_FLG = 'Y', ACSN_NUM = '" + specimenNumber + "' ";
            selectSQL = selectSQL + "where INTNL_TST_NUM = " + antibodyInternalTestNumber.ToString();
            sqlReturn = dBConnection.ExecuteSQL(selectSQL);

            var result = await api.GetInstrumentTestsAsync(siteCode, sortBy, sortDirection, pageIndex, pageSize, new CancellationToken());
 
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

                // ***** AN ERROR OCCURRED DESERIALIZING THE RESPONSE *****

                // Validate the new patient/specimen/test data is in the returned result.
                    resultsFound = false;
                    for (int i = 0; i < result.Content.Result.Count; i++)
                    {
                        if (result.Content.Result[i].filteredCollection.antibodyInternalTestNumber == antibodyInternalTestNumber)
                        {
                            resultsFound = true;
                            Assert.AreEqual(patientNumber, result.Content.Result[i].filteredCollection.patientNumber);
                            Assert.AreEqual(medicalRecordNumber, result.Content.Result[i].filteredCollection.medicalRecordNumber);
                            Assert.AreEqual(patientName, result.Content.Result[i].filteredCollection.patientName);
                            Assert.AreEqual(specimenNumber, result.Content.Result[i].filteredCollection.specimenNumber);
                            Assert.AreEqual(testCode, result.Content.Result[i].filteredCollection.testCode);
                            Assert.AreEqual(testStatus, result.Content.Result[i].filteredCollection.testStatus);
                        }
                    }
                    Assert.AreEqual(true, resultsFound);
                
            });

            // Clean up data, by setting the test status to "C" (Cancelled).
            selectSQL = "update TST_MST set TST_STAT_CD = 'C', CANC_DTTM = GetDate(), CANC_TECH_ID = 'T03' ";
            selectSQL = selectSQL + "where INTNL_TST_NUM = " + antibodyInternalTestNumber.ToString();
            sqlReturn = dBConnection.ExecuteSQL(selectSQL);

        }
    }
}
