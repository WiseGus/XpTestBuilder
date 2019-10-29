using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Web.Script.Serialization;
using XpTestBuilder.Common;
using XpTestBuilder.Server.Commands;

namespace XpTestBuilder.Server
{
    [ServiceBehavior(UseSynchronizationContext = true, InstanceContextMode = InstanceContextMode.Single)]
    public class CommandService : ICommandService
    {
        internal readonly CommandParser commandParser;
        internal readonly BuildManager buildsManager;
        internal readonly SolutionsManager solutionsManager;
        internal readonly CopyToPatchesFolderManager copyToPatchesFolderManager;
        internal readonly Dictionary<string, ClientConnectionInfo> clients;

        public CommandService()
        {
            commandParser = new CommandParser(this);
            clients = new Dictionary<string, ClientConnectionInfo>();
            buildsManager = new BuildManager(clients);
            solutionsManager = new SolutionsManager(new JavaScriptSerializer().Deserialize<SourcesFoldersInfo[]>(ConfigurationManager.AppSettings["SourcesFolders"]));
            solutionsManager.InitSolutionInfo();
            copyToPatchesFolderManager = new CopyToPatchesFolderManager(ConfigurationManager.AppSettings["OutputDebugFolder"], ConfigurationManager.AppSettings["PatchesFolder"]);
        }

        public void RefreshClientLastSeen(ICommandCallback connection)
        {
            var foundConnection = clients.FirstOrDefault(p => p.Value.Connection == connection);
            if (foundConnection.Value != null)
            {
                foundConnection.Value.LastSeen = DateTime.Now;
            }
        }

        public void RegisterClient(string clientName)
        {
            var connection = OperationContext.Current.GetCallbackChannel<ICommandCallback>();

            if (clients.TryGetValue(clientName, out ClientConnectionInfo existingConnection))
            {
                Console.WriteLine($"Client {clientName} already exists");
                connection.SendToClientCommand(new ClientNameExistsCommand());
                return;
            }

            Console.WriteLine($"New client registered: {clientName}");
            clients[clientName] = new ClientConnectionInfo { Connection = connection };

            connection.SendToClientCommand(new ClientRegisterOkCommand(new ClientRegistration(clientName, ConfigurationManager.AppSettings["ServerName"])));
            connection.SendToClientCommand(new GetSolutionsCommand(solutionsManager.SolutionInfo));
            connection.SendToClientCommand(new JobsAnalysisCommand(buildsManager.GetJobsAnalysis()));
        }

        public void SendToServerCommand(CommandData eventData)
        {
            var connection = OperationContext.Current.GetCallbackChannel<ICommandCallback>();
            var clientName = clients.FirstOrDefault(p => p.Value.Connection == connection).Key;
            Console.WriteLine($"{clientName} => [{eventData.Command}]");
            Console.WriteLine($"\tPayload: {eventData.Payload}");

            commandParser.ParseCommand(connection, new JobInfo
            {
                CreatedAt = DateTime.Now,
                CreatedFrom = clientName,
                Request = eventData
            });
        }

        public bool ValidateConnection(ICommandCallback connection)
        {
            var foundConnection = clients.FirstOrDefault(p => p.Value.Connection == connection);
            return foundConnection.Value != null;
        }

        public void UnregisterClient()
        {
            var connection = clients.FirstOrDefault(p => p.Value.Connection == OperationContext.Current.GetCallbackChannel<ICommandCallback>());
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
