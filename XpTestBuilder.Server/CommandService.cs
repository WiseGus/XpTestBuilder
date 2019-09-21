using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using XpTestBuilder.Common;
using XpTestBuilder.Server.Commands;

namespace XpTestBuilder.Server
{
    [ServiceBehavior(UseSynchronizationContext = true, InstanceContextMode = InstanceContextMode.Single)]
    public class CommandService : ICommandService
    {
        internal readonly CommandParser commandParser;
        internal readonly BuildManager buildsManager;
        internal readonly Dictionary<string, ICommandCallback> clients;

        public CommandService()
        {
            commandParser = new CommandParser(this);
            clients = new Dictionary<string, ICommandCallback>();
            buildsManager = new BuildManager(clients);
        }

        public void RegisterClient(string clientName)
        {
            var connection = OperationContext.Current.GetCallbackChannel<ICommandCallback>();

            if (clients.TryGetValue(clientName, out ICommandCallback existingConnection))
            {
                Console.WriteLine($"Client {clientName} already exists");
                connection.SendCommand(new ClientNameExistsCommand());
                return;
            }

            Console.WriteLine($"New client registered: {clientName}");
            clients[clientName] = connection;

            connection.SendCommand(new ClientRegisterOkCommand(new ClientRegistration(clientName, ConfigurationManager.AppSettings["ServerName"])));
            connection.SendCommand(new GetSolutionsCommand(ConfigurationManager.AppSettings["SourcesFolder"]));
            connection.SendCommand(new JobsAnalysisCommand(buildsManager.GetJobsAnalysis()));
        }

        public void ReceiveCommand(CommandData eventData)
        {
            var connection = OperationContext.Current.GetCallbackChannel<ICommandCallback>();
            var clientName = clients.FirstOrDefault(p => p.Value == connection).Key;
            Console.WriteLine($"{clientName} => [{eventData.Command}]");
            Console.WriteLine($"\t{eventData.Payload}");

            commandParser.ParseCommand(connection, new JobInfo
            {
                CreatedAt = DateTime.Now,
                CreatedFrom = clientName,
                Request = eventData
            });
        }

        public void UnregisterClient()
        {
            var connection = clients.FirstOrDefault(p => p.Value == OperationContext.Current.GetCallbackChannel<ICommandCallback>());
            if (!string.IsNullOrEmpty(connection.Key))
            {
                clients.Remove(connection.Key);
                Console.WriteLine($"Client {connection.Key} unregistered");
            }
        }

        public void ForceDisconnect(string username)
        {
            if (clients.TryGetValue(username, out var connection))
            {
                clients.Remove(username);
                Console.WriteLine($"Client {username} unregistered");
            }
        }

    }
}
