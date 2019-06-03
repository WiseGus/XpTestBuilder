using System.IO;
using System.Web.Script.Serialization;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class GetSolutionsCommand : ICommand
    {
        private readonly string _sourcesFolder;
        private JavaScriptSerializer _serializer;

        public GetSolutionsCommand(string sourcesFolder)
        {
            _sourcesFolder = sourcesFolder;
            _serializer = new JavaScriptSerializer();
        }

        public CommandData Execute()
        {
            return new CommandData
            {
                Command = CommandsIndex.GET_SOLUTIONS,
                Payload = _serializer.Serialize(GetSolutions())
            };
        }

        private SolutionInfo GetSolutions()
        {
            var res = new SolutionInfo
            {
                Name = "Root",
                Path = _sourcesFolder,
                SolutionType = SolutionType.Folder
            };

            GetSolutions(_sourcesFolder, res);

            //var solutions = Directory.EnumerateFiles(_sourcesFolder, "*.sln", SearchOption.AllDirectories);
            //FileInfo fileInfo;
            //foreach (var solution in solutions)
            //{
            //    fileInfo = new FileInfo(solution);
            //    res.Solutions.Add(new SolutionInfo
            //    {
            //        Name = fileInfo.Name,
            //        Path = fileInfo.FullName,
            //        SolutionType = SolutionType.Solution
            //    });
            //}

            return res;
        }

        private void GetSolutions(string path, SolutionInfo parent)
        {
            bool solutionExist = false;

            var solutions = Directory.EnumerateFiles(path, "*.sln");
            FileInfo fileInfo;
            foreach (var solution in solutions)
            {
                solutionExist = true;
                fileInfo = new FileInfo(solution);
                parent.Solutions.Add(new SolutionInfo
                {
                    Name = fileInfo.Name,
                    Path = fileInfo.FullName,
                    SolutionType = SolutionType.Solution
                });
            }

            if (solutionExist)
            {
                return;
            }

            var dirs = Directory.EnumerateDirectories(path);
            DirectoryInfo dirInfo;
            foreach (var dir in dirs)
            {
                dirInfo = new DirectoryInfo(dir);
                if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden)) continue;

                var solutionInfo = new SolutionInfo
                {
                    Name = dirInfo.Name,
                    Path = dirInfo.FullName,
                    SolutionType = SolutionType.Folder
                };
                GetSolutions(dir, solutionInfo);
                parent.Solutions.Add(solutionInfo);
            }
        }
    }
}
