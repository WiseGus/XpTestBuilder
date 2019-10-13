using System.ServiceModel;

namespace XpTestBuilder.Common
{
    public interface ICommandCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendToClientCommand(CommandData data);
    }
}
