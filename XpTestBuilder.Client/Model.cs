using System;
using System.Collections.Generic;
using System.Linq;
using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public enum SolutionType { Folder, Solution }
    public enum JobDataInfoType { Error, Pending, Success, BuildStarted }

    public class SolutionInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public SolutionType SolutionType { get; set; }
        public List<SolutionInfo> Solutions { get; set; }
    }

    public class JobData
    {
        public List<JobDataInfo> Data { get; private set; }

        public JobData() {
            Data = new List<JobDataInfo>();
        }

        public JobData(List<BuildResult> buildResults)
        {
            foreach (var buildRes in buildResults.OrderByDescending(p => p.JobInfo.CreatedAt))
            {
                Data.Add(new JobDataInfo(buildRes));
            }
        }
    }

    public class JobDataInfo
    {
        public string JobID { get; set; }
        public string Solution { get; set; }
        public string AddedFrom { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public JobDataInfoType Status { get; set; }
        public List<string> Log { get; set; }

        public JobDataInfo(BuildResult buildRes)
        {
            JobID = buildRes.JobInfo.JobID.ToString();
            AddedAt = buildRes.JobInfo.CreatedAt;
            AddedFrom = buildRes.JobInfo.CreatedFrom;
            FinishedAt = buildRes.FinishedAt;
            Log = new List<string>(buildRes.Log);
            Solution = buildRes.JobInfo.Request.Payload;

            switch (buildRes.Status)
            {
                case BuildResultType.Success: Status = JobDataInfoType.Success; break;
                case BuildResultType.Failure: Status = JobDataInfoType.Error; break;
                case BuildResultType.Started: Status = JobDataInfoType.BuildStarted; break;
                case BuildResultType.Pending: Status = JobDataInfoType.Pending; break;
            }
        }
    }
}
