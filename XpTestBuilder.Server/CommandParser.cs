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
            if (!CheckConnection(connection)) return;

            switch (jobInfo.Request.Command)
            {
                case CommandsIndex.PING:
                    Ping(connection, Convert.ToBoolean(jobInfo.Request.Payload));
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

        private bool CheckConnection(ICommandCallback connection)
        {
            if (!_commandService.ValidateConnection(connection))
            {
                connection.SendToClientCommand(new DropClientConnectionCommand());
                return false;
            }
            else
            {
                _commandService.RefreshClientLastSeen(connection);
                return true;
            }
        }

        private void Ping(ICommandCallback connection, bool silent)
        {
            connection.SendToClientCommand(new PongCommand(silent));
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