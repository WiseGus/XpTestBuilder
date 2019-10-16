using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public class PingCommand : ICommand
    {
        private readonly bool _silent;

        public PingCommand(bool silent)
        {
            _silent = silent;
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.PING,
                Payload = _silent.ToString()
            };
        }
    }
}
