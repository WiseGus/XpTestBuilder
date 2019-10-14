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

            ICommandCallback existingConnection;
            if (clients.TryGetValue(clientName, out existingConnection))
            {
                Console.WriteLine(string.Format("Client {0} already exists", clientName));
                connection.SendToClientCommand(new ClientNameExistsCommand());
                return;
            }

            Console.WriteLine(string.Format("New client registered: {0}", clientName));
            clients[clientName] = connection;

            connection.SendToClientCommand(new ClientRegisterOkCommand(new ClientRegistration(clientName, ConfigurationManager.AppSettings["ServerName"])));
            connection.SendToClientCommand(new GetSolutionsCommand(new JavaScriptSerializer().Deserialize<SourcesFoldersInfo[]>(ConfigurationManager.AppSettings["SourcesFolders"])));
            connection.SendToClientCommand(new JobsAnalysisCommand(buildsManager.GetJobsAnalysis()));
        }

        public void SendToServerCommand(CommandData eventData)
        {
            var connection = OperationContext.Current.GetCallbackChannel<ICommandCallback>();
            var clientName = clients.FirstOrDefault(p => p.Value == connection).Key;
            Console.WriteLine(string.Format("{0} => [{1}]", clientName, eventData.Command));
            Console.WriteLine(string.Format("\t{0}", eventData.Payload));

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
                Console.WriteLine(string.Format("Client {0} unregistered", connection.Key));
            }
        }

        public void ForceDisconnect(string username)
        {
            ICommandCallback connection;
            if (clients.TryGetValue(username, out connection))
            {
                clients.Remove(username);
                Console.WriteLine(string.Format("Client {0} unregistered", username));
            }
        }

    }
}
