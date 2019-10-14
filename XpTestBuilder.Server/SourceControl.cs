using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace XpTestBuilder.Server
{
    internal static class VersionControl
    {
        public static GetStatus GetLatestChanges(string tfsName, string tfsWorkSpace, string[] tfsProjects, string fileName)
        {
            Workspace workspace = null;
            var folders = GetWorkingFolders(tfsName, tfsWorkSpace, tfsProjects, ref workspace);
            if (folders.Count() != 0)
            {
                WorkingFolder folder;
                if (TryMatchFile(fileName, folders, out folder))
                {
                    var fileInfo = new FileInfo(fileName);
                    if (fileInfo.Name.Replace(fileInfo.Extension, "") == fileInfo.Directory.Name) {
                        fileName = fileInfo.Directory.FullName;
                    }

                    string fn = fileName.Remove(0, folder.LocalItem.Length);
                    fn = folder.ServerItem + '/' + fn.Replace('\\', '/');

                    return workspace.Get(new GetRequest(fn, RecursionType.Full, VersionSpec.Latest), GetOptions.Overwrite);
                }
                else
                {
                    throw new Exception("TFS working folder not match");
                }
            }
            else
            {
                throw new Exception("TFS working folder not found");
            }
        }

        private static IEnumerable<WorkingFolder> FindWorkingFolders(string[] tfsProjects, List<WorkingFolder> folders)
        {
            return folders.Where(p => tfsProjects.Contains(p.ServerItem) || tfsProjects.Contains("$/" + p.ServerItem));
        }

        private static bool TryMatchFile(string fileName, IEnumerable<WorkingFolder> folders, out WorkingFolder matchedFolder)
        {
            matchedFolder = null;
            foreach (var folder in folders)
            {
                if (fileName.ToUpper().StartsWith(folder.LocalItem.ToUpper()))
                {
                    matchedFolder = folder;
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<WorkingFolder> GetWorkingFolders(string tfsName, string tfsWorkSpace, string[] tfsProjects, ref Workspace workspace)
        {
            TfsTeamProjectCollection tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfsName));
            tfs.Authenticate();

            VersionControlServer versionControl = tfs.GetService<VersionControlServer>();
            if (string.IsNullOrEmpty(tfsWorkSpace))
            {
                tfsWorkSpace = Environment.MachineName;
            }
            Workspace[] workSpaces = versionControl.QueryWorkspaces(tfsWorkSpace, versionControl.AuthorizedUser, Environment.MachineName);
            if (workSpaces.Length == 0)
            {
                throw new Exception(string.Format("TFS WorkSpace '{0}' not found", tfsWorkSpace));
            }

            workspace = workSpaces[0];
            List<WorkingFolder> folders = new List<WorkingFolder>(workspace.Folders);

            return FindWorkingFolders(tfsProjects, folders);
        }
    }
}