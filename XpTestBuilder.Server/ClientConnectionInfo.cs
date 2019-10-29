using System;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server
{
    internal sealed class ClientConnectionInfo
    {
        public DateTime LastSeen { get; set; } = DateTime.Now;
        public ICommandCallback Connection { get; set; }
    }
}
