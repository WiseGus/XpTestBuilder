using System;
using System.Web.Script.Serialization;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class JobsStatusCommand : ICommand
    {
        private readonly JobStatus _jobStatus;
        private readonly JavaScriptSerializer _serializer;

        public JobsStatusCommand(Guid jobId, BuildResultType jobStatusType)
        {
            _jobStatus = new JobStatus(jobId, jobStatusType);
            _serializer = new JavaScriptSerializer();
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.JOB_STATUS,
                Payload = _serializer.Serialize(_jobStatus)
            };
        }
    }
}
