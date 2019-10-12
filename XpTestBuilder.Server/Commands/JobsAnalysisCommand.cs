using System.Collections.Generic;
using System.Web.Script.Serialization;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class JobsAnalysisCommand : ICommand
    {
        private IEnumerable<BuildResult> _jobsAnalysis;
        private JavaScriptSerializer _serializer;

        public JobsAnalysisCommand(IEnumerable<BuildResult> jobsAnalysis)
        {
            _jobsAnalysis = jobsAnalysis;
            _serializer = new JavaScriptSerializer
            {
                MaxJsonLength = int.MaxValue
            };
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.GET_JOBS,
                Payload = _serializer.Serialize(new List<BuildResult>(_jobsAnalysis))
            };
        }
    }
}
