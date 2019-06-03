using System;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server
{
    public static class Extensions
    {
        public static void ReceiveCommand(this ICommandService commandService, ICommand command)
        {
            commandService.ReceiveCommand(command.Execute());
        }

        public static void SendCommand(this ICommandCallback commandCallback, ICommand command)
        {
            var res = command.Execute();

            Console.WriteLine($"* Server  => [{res.Command}]");
            //Console.WriteLine($"\t{res.Payload}");

            commandCallback.SendCommand(res);
        }
    }
}
