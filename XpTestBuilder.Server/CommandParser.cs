using XpTestBuilder.Common;
using XpTestBuilder.Server.Commands;

namespace XpTestBuilder.Server
{
    internal class CommandParser
    {
        private readonly CommandService _commandService;

        public CommandParser(CommandService commandService)
        {
            _commandService = commandService;
        }

        internal void ParseCommand(ICommandCallback connection, JobInfo jobInfo)
        {
            switch (jobInfo.Request.Command)
            {
                case CommandsIndex.PING:
                    connection.SendCommand(new PongCommand());
                    break;
                case CommandsIndex.BUILD_SOLUTION:
                    BuildSolution(jobInfo);
                    break;
            }
        }

        private void BuildSolution(JobInfo jobInfo)
        {
            _commandService.buildsManager.AddJob(jobInfo);
        }
    }
}