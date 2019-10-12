using System.IO;
using System.Web.Script.Serialization;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Commands
{
    public class SourcesFoldersInfo {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class GetSolutionsCommand : ICommand
    {
        private readonly SourcesFoldersInfo[] _sourcesFolders;
        private JavaScriptSerializer _serializer;

        public GetSolutionsCommand(SourcesFoldersInfo[] sourcesFolders)
        {
            _sourcesFolders = sourcesFolders;
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
                Payload = _serializer.Serialize(GetSolutions())
            };
        }

        private SolutionInfo GetSolutions()
        {
            var res = new SolutionInfo
            {
                Name = "Root",
                Path = string.Empty,
                SolutionType = SolutionType.Folder
            };

            foreach (var sourcesFolder in _sourcesFolders)
            {
                var parent = new SolutionInfo
                {
                    Name = sourcesFolder.Name,
                    Path = string.Empty,
                    SolutionType = SolutionType.Folder
                };
                res.Solutions.Add(parent);
                GetSolutions(sourcesFolder.Path, parent);
            }

            //foreach (var sourcesFolder in _sourcesFolders)
            //{
            //    var parent = new SolutionInfo
            //    {
            //        Name = sourcesFolder.Name,
            //        Path = string.Empty,
            //        SolutionType = SolutionType.Folder
            //    };
            //    res.Solutions.Add(parent);

            //    var solutions = Directory.EnumerateFiles(sourcesFolder.Path, "*.sln", SearchOption.AllDirectories);
            //    FileInfo fileInfo;
            //    foreach (var solution in solutions)
            //    {
            //        fileInfo = new FileInfo(solution);
            //        parent.Solutions.Add(new SolutionInfo
            //        {
            //            Name = fileInfo.Name,
            //            Path = fileInfo.FullName,
            //            SolutionType = SolutionType.Solution
            //        });
            //    }
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
