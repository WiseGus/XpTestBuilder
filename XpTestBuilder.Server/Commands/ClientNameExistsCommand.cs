﻿using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class DropClientConnectionCommand : ICommand
    {
        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.DROP_CLIENT_CONNECTION
            };
        }
    }
}
