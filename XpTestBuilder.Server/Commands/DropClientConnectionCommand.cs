using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class ClientNameExistsCommand : ICommand
    {
        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.CLIENT_NAME_EXISTS
            };
        }
    }
}
