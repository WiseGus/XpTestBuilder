using System;
using System.IO;
using System.ServiceModel;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server
{
    public class CopyToPatchesFolderManager
    {
        private readonly DirectoryInfo _outputDebugFolderInfo;
        private readonly DirectoryInfo _patchesFolderInfo;

        public CopyToPatchesFolderManager(string outputDebugFolder, string patchesFolder)
        {
            _outputDebugFolderInfo = new DirectoryInfo(outputDebugFolder);
            _patchesFolderInfo = new DirectoryInfo(patchesFolder);
        }

        public void Execute(string solutionFilename)
        {

            var response = new CommandData { Command = CommandsIndex.COPY_TO_PATCHES_FOLDER_RESULT };
            try
            {
                var fileInfo = new FileInfo(solutionFilename);

                var potentialLibName = fileInfo.Name.Replace(fileInfo.Extension, ".dll");
                var foundBuildFile = Directory.GetFiles(_outputDebugFolderInfo.FullName, potentialLibName, SearchOption.AllDirectories);

                if (foundBuildFile.Length > 0)
                {
                    var sourceBuildPath = foundBuildFile[0];
                    var destinationBuildPath = foundBuildFile[0].Replace(_outputDebugFolderInfo.FullName, _patchesFolderInfo.FullName);

                    File.Copy(sourceBuildPath, destinationBuildPath, true);

                    response.Payload = $"Success: Copied {sourceBuildPath} to {destinationBuildPath}";
                }
            }
            catch (Exception ex)
            {
                response.Payload = $"Failure for:{Environment.NewLine}{solutionFilename}:{Environment.NewLine}{Environment.NewLine} {ex.ToString()}";
            }
            finally
            {
                var client = OperationContext.Current.GetCallbackChannel<ICommandCallback>();
                client.SendToClientCommand(response);
            }
        }
    }
}