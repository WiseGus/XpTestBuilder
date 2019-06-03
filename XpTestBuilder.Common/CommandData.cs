using System;
using System.Runtime.Serialization;

namespace XpTestBuilder.Common
{
    [DataContract]
    public class CommandData : ICloneable
    {
        [DataMember]
        public string Command { get; set; }
        [DataMember]
        public string Payload { get; set; }

        public object Clone()
        {
            return new CommandData { Command = Command, Payload = Payload };
        }
    }
}
