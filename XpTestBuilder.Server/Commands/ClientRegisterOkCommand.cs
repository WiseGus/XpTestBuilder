using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class ClientRegisterOkCommand : ICommand
    {
        private readonly string _clientName;

        public ClientRegisterOkCommand(string clientName)
        {
            _clientName = clientName;
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.CLIENT_REGISTER_OK,
                Payload = _clientName
            };
        }
    }
}
