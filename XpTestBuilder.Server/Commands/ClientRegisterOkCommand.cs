using System.Web.Script.Serialization;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class ClientRegisterOkCommand : ICommand
    {
        private readonly ClientRegistration _clientRegistration;
        private JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public ClientRegisterOkCommand(ClientRegistration clientRegistration)
        {
            _clientRegistration = clientRegistration;
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.CLIENT_REGISTER_OK,
                Payload = _serializer.Serialize(_clientRegistration)
            };
        }
    }
}
