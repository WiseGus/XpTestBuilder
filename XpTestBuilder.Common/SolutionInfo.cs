using System.Collections.Generic;

namespace XpTestBuilder.Common
{
    public enum SolutionType { Folder, Solution }

    public class SolutionInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public SolutionType SolutionType { get; set; }
        public List<SolutionInfo> Solutions { get; set; }

        public SolutionInfo()
        {
            Solutions = new List<SolutionInfo>();
        }
    }
}
