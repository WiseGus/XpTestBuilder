using System;
using System.Collections.Generic;

namespace XpTestBuilder.Common
{
    public class BuildResult
    {
        public JobInfo JobInfo { get; set; }
        public bool? Success { get; set; }
        public DateTime? FinishedAt { get; set; }
        public List<string> Log { get; set; }

        public BuildResult()
        {
            Log = new List<string>();
        }

        public BuildResult(JobInfo jobInfo)
            : this()
        {
            JobInfo = new JobInfo
            {
                JobID = jobInfo.JobID,
                CreatedAt = jobInfo.CreatedAt,
                CreatedFrom = jobInfo.CreatedFrom,
                Request = new CommandData
                {
                    Command = jobInfo.Request.Command,
                    Payload = jobInfo.Request.Payload
                }
            };
        }
    }
}
