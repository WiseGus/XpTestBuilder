﻿using System.ServiceModel;

namespace XpTestBuilder.Common
{
    [ServiceContract(CallbackContract = typeof(ICommandCallback))]
    public interface ICommandService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterClient(string clientName);

        [OperationContract(IsOneWay = true)]
        void SendToServerCommand(CommandData data);

        [OperationContract(IsOneWay = true)]
        void UnregisterClient();
    }
}
