using System.ServiceModel;

namespace XpTestBuilder.Common
{
    public interface ICommandCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendCommand(CommandData data);
    }
}
