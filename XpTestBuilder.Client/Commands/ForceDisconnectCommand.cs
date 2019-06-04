using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public class ForceDisconnectCommand : ICommand
    {
        private readonly string _username;

        public ForceDisconnectCommand(string username)
        {
            _username = username;
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.FORCE_DISCONNECT,
                Payload = _username
            };
        }
    }
}
