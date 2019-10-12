using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using XpTestBuilder.Common;

namespace XpTestBuilder.Server.Builders
{
    public class MSBuilder
    {
        private XpTestBuilder.Common.BuildResult _buildResult;

        public MSBuilder(XpTestBuilder.Common.BuildResult buildResult)
        {
            _buildResult = buildResult;
        }

        public void Execute()
        {
            var memoryLogger = new MemoryLogger(_buildResult.Log);

            var buildParams = new BuildParameters(new ProjectCollection());
            buildParams.DetailedSummary = true;
            buildParams.Loggers = new ILogger[] { memoryLogger };

            var globalProperty = new Dictionary<string, string>();
            globalProperty.Add("Configuration", "Debug");
            globalProperty.Add("Platform", "Any CPU");

            string[] targets = new[] { "Rebuild" };

            var BuildRequest = new BuildRequestData(_buildResult.JobInfo.Request.Payload, globalProperty, null, targets, null);

            var buildResult = Microsoft.Build.Execution.BuildManager.DefaultBuildManager.Build(buildParams, BuildRequest);

            _buildResult.FinishedAt = DateTime.Now;
            _buildResult.Status = buildResult.OverallResult == BuildResultCode.Success ? BuildResultType.Success : BuildResultType.Failure;

            if (buildResult.Exception != null)
            {
                _buildResult.Log.Add(buildResult.Exception.ToString());
            }
        }
    }
}
