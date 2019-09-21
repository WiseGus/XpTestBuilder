namespace XpTestBuilder.Common
{
    public class ClientRegistration
    {
        public string ClientName { get; set; }
        public string ServerName { get; set; }

        public ClientRegistration() { }

        public ClientRegistration(string clientName, string serverName)
        {
            ClientName = clientName;
            ServerName = serverName;
        }

    }
}
