using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public class CopyToPatchesFolderCommand : ICommand
    {
        private string _solutionPath;

        public CopyToPatchesFolderCommand(string solutionPath)
        {
            _solutionPath = solutionPath;
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.COPY_TO_PATCHES_FOLDER,
                Payload = _solutionPath
            };
        }
    }
}
