using System;

namespace XpTestBuilder.Common
{
    public class JobStatus
    {
        public Guid JobID { get; set; }
        public BuildResultType JobStatusType { get; set; }

        public JobStatus() { }

        public JobStatus(Guid jobID, BuildResultType jobStatusType)
        {
            JobID = jobID;
            JobStatusType = jobStatusType;
        }
    }
}