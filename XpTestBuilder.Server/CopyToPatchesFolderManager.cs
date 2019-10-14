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

                    response.Payload = string.Format("Success: Copied {0} to {1}", sourceBuildPath, destinationBuildPath);
                }
            }
            catch (Exception ex)
            {
                response.Payload = string.Format("Failure for:{0}{1}:{0}{0} {2}", Environment.NewLine, solutionFilename, ex.ToString());
            }
            finally
            {
                var client = OperationContext.Current.GetCallbackChannel<ICommandCallback>();
                client.SendToClientCommand(response);
            }
        }
    }
}