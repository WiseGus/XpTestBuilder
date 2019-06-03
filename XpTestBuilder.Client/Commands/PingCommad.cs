using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public class PingCommand : ICommand
    {

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.PING
            };
        }
    }
}
