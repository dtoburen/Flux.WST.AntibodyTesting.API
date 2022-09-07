namespace Flux.WST.AntibodyTesting.API.Common
{
    public class InstrumentTestResponse
    {
        public int patientNumber { get; set; }
        public string medicalRecordNumber { get; set; }
        public string patientName { get; set; }
        public string specimenNumber { get; set; }
        public int antibodyInternalTestNumber { get; set; }
        public string testCode { get; set; }
        public string testStatus { get; set; }
    }

    public class InstrumentTest
    {
        public InstrumentTestResponse filteredCollection { get; set; }
        public int collectionSize {get; set;}
    }

}
