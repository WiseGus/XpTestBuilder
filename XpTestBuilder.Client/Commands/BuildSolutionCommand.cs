using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public class BuildSolutionCommand : ICommand
    {
        private string _solutionPath;

        public BuildSolutionCommand(string solutionPath)
        {
            _solutionPath = solutionPath;
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.BUILD_SOLUTION,
                Payload = _solutionPath
            };
        }
    }
}
