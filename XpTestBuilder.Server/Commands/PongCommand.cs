using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class PongCommand : ICommand
    {
        private readonly bool _silent;

        public PongCommand(bool silent)
        {
            _silent = silent;
        }
        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.PONG,
                Payload = _silent.ToString()
            };
        }
    }
}
