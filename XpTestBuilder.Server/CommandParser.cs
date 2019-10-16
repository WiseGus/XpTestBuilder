using System;
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
                    connection.SendToClientCommand(new PongCommand(Convert.ToBoolean(jobInfo.Request.Payload)));
                    break;
                case CommandsIndex.FORCE_DISCONNECT:
                    ForceDisconnect(jobInfo.Request.Payload);
                    break;
                case CommandsIndex.BUILD_SOLUTION:
                    BuildSolution(jobInfo);
                    break;
                case CommandsIndex.COPY_TO_PATCHES_FOLDER:
                    CopyToPatchesFolder(jobInfo.Request.Payload);
                    break;
            }
        }

        private void ForceDisconnect(string username)
        {
            _commandService.ForceDisconnect(username);
        }

        private void BuildSolution(JobInfo jobInfo)
        {
            _commandService.buildsManager.AddJob(jobInfo);
        }

        private void CopyToPatchesFolder(string solutionFilename)
        {
            _commandService.copyToPatchesFolderManager.Execute(solutionFilename);
        }
    }
}