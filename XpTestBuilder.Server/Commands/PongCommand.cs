using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class PongCommand : ICommand
    {
        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.PONG
            };
        }
    }
}
