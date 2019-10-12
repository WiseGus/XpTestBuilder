using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using XpTestBuilder.Common;
using XpTestBuilder.Server.Builders;
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
                ProcessBuildJob(job);
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

        private BuildResult ProcessBuildJob(JobInfo jobInfo)
        {
            var buildRes = _jobResults.FirstOrDefault(p => p.JobInfo.JobID == jobInfo.JobID);
            buildRes.Status = BuildResultType.Started;

            var buildJob = new MSBuilder(buildRes);
            buildJob.Execute();

            return buildRes;
        }

        private void Broadcast(ICommand command)
        {
            foreach (var client in _serviceSubscribers)
            {
                try
                {
                    client.Value.SendCommand(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unabled to send command to {client.Key}. Error: {ex.ToString()}");
                }
            }
        }
    }
}