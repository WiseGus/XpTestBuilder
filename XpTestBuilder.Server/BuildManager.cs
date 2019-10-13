using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Timers;
using System.Web.Script.Serialization;
using XpTestBuilder.Common;
using XpTestBuilder.Server.Commands;

namespace XpTestBuilder.Server
{
    internal class BuildManager
    {
        //public delegate void BuildComplete(IJobResult buildResult);
        //public event BuildComplete OnBuildComplete;

        private readonly Timer _timer = new Timer();
        private readonly ConcurrentQueue<JobInfo> _jobs = new ConcurrentQueue<JobInfo>();
        private readonly ConcurrentBag<BuildResult> _jobResults = new ConcurrentBag<BuildResult>();
        private readonly Dictionary<string, ICommandCallback> _serviceSubscribers;
        private bool _jobPending = false;

        public BuildManager(Dictionary<string, ICommandCallback> serviceSubscribers)
        {
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            _timer.Interval = 3000;
            _timer.AutoReset = true;
            _timer.Start();
            _serviceSubscribers = serviceSubscribers;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_jobPending) return;

            JobInfo job;
            if (_jobs.TryDequeue(out job))
            {
                _jobPending = true;
                //Task.Factory.StartNew(() => ProcessBuildJob(job))
                //.ContinueWith(p =>
                //{
                //    _jobPending = false;
                //    //OnBuildComplete?.Invoke(p.Result);
                //});
                Broadcast(new JobsStatusCommand(job.JobID, BuildResultType.Started));

                var buildResult = _jobResults.FirstOrDefault(p => p.JobInfo.JobID == job.JobID);
                buildResult.Status = BuildResultType.Started;
                try
                {
                    GetLatestForSolutionProject(buildResult);
                    ProcessBuildJob(buildResult);
                }
                catch (Exception ex)
                {
                    buildResult.Status = BuildResultType.Failure;
                    buildResult.Log.Add(ex.ToString());
                }
                _jobPending = false;

                Broadcast(new JobsAnalysisCommand(_jobResults));
            }
        }

        public IEnumerable<JobInfo> GetJobs()
        {
            return _jobs.ToArray();
        }

        public bool JobExists(JobInfo jobInfo)
        {
            return GetJobs().Any(p => p.Request.Payload == jobInfo.Request.Payload);
        }

        public void AddJob(JobInfo jobInfo)
        {
            if (JobExists(jobInfo)) return;

            _jobResults.Add(new BuildResult(jobInfo));
            _jobs.Enqueue(jobInfo);

            Broadcast(new JobsAnalysisCommand(_jobResults));
        }

        public IEnumerable<BuildResult> GetJobsAnalysis()
        {
            return _jobResults.ToArray();
        }

        private void GetLatestForSolutionProject(BuildResult buildResult)
        {
            var tfsUrl = ConfigurationManager.AppSettings["TfsUrl"];
            var tfsWorkSpace = ConfigurationManager.AppSettings["TfsWorkSpace"];
            var tfsProjects = new JavaScriptSerializer().Deserialize<string[]>(ConfigurationManager.AppSettings["TfsProjectPaths"]);

            var getLatestResult = VersionControl.GetLatestChanges(tfsUrl, tfsWorkSpace, tfsProjects, buildResult.JobInfo.Request.Payload);
            var sb = new StringBuilder();
            sb.AppendLine("TFS get latest results:");
            sb.AppendLine($"{nameof(getLatestResult.HaveResolvableWarnings)}: {getLatestResult.HaveResolvableWarnings}");
            sb.AppendLine($"{nameof(getLatestResult.NoActionNeeded)}: {getLatestResult.NoActionNeeded}");
            sb.AppendLine($"{nameof(getLatestResult.NumBytes)}: {getLatestResult.NumBytes}");
            sb.AppendLine($"{nameof(getLatestResult.NumConflicts)}: {getLatestResult.NumConflicts}");
            sb.AppendLine($"{nameof(getLatestResult.NumFailures)}: {getLatestResult.NumFailures}");
            sb.AppendLine($"{nameof(getLatestResult.NumFiles)}: {getLatestResult.NumFiles}");
            sb.AppendLine($"{nameof(getLatestResult.NumOperations)}: {getLatestResult.NumOperations}");
            sb.AppendLine($"{nameof(getLatestResult.NumResolvedConflicts)}: {getLatestResult.NumResolvedConflicts}");
            sb.AppendLine($"{nameof(getLatestResult.NumUpdated)}: {getLatestResult.NumUpdated}");
            sb.AppendLine($"{nameof(getLatestResult.NumWarnings)}: {getLatestResult.NumWarnings}");
            buildResult.Log.Add(sb.ToString());

            if (getLatestResult.NumFailures > 0)
            {
                var failures = getLatestResult.GetFailures();
                buildResult.Log.AddRange(failures.Select(failure => failure.GetFormattedMessage()));
                buildResult.Status = BuildResultType.Failure;
            }
        }

        private void ProcessBuildJob(BuildResult buildResult)
        {
            var buildJob = new MSBuilder(buildResult);
            buildJob.Execute();
        }

        private void Broadcast(ICommand command)
        {
            foreach (var client in _serviceSubscribers)
            {
                try
                {
                    client.Value.SendToClientCommand(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unabled to send command to {client.Key}. Error: {ex.ToString()}");
                }
            }
        }
    }
}