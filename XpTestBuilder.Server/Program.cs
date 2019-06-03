using System;
using System.ServiceModel;

namespace XpTestBuilder.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(CommandService)))
            {
                host.Open();

                Console.WriteLine($"Server listening at: {host.BaseAddresses[0].AbsoluteUri}");
                Console.WriteLine();
                Console.ReadLine();

                host.Close();
            }
        }
    }

}
