using System;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server
{
    public static class Extensions
    {
        public static void SendToServerCommand(this ICommandService commandService, ICommand command)
        {
            commandService.SendToServerCommand(command.Execute());
        }

        public static void SendToClientCommand(this ICommandCallback commandCallback, ICommand command)
        {
            var res = command.Execute();

            Console.WriteLine($"* Server  => [{res.Command}]");
            //Console.WriteLine($"\t{res.Payload}");

            commandCallback.SendToClientCommand(res);
        }
    }
}
