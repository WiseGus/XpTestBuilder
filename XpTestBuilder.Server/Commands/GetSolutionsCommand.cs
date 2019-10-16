using System.Web.Script.Serialization;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class GetSolutionsCommand : ICommand
    {
        private readonly SolutionInfo _solutionInfo;
        private JavaScriptSerializer _serializer;

        public GetSolutionsCommand(SolutionInfo solutionInfo)
        {
            _solutionInfo = solutionInfo;
            _serializer = new JavaScriptSerializer
            {
                MaxJsonLength = int.MaxValue
            };
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.GET_SOLUTIONS,
                Payload = _serializer.Serialize(_solutionInfo)
            };
        }
    }
}
