using Microsoft.Build.Framework;
using System.Collections.Generic;

namespace XpTestBuilder.Server
{
    public class MemoryLogger : ILogger
    {
        public LoggerVerbosity Verbosity { get; set; }
        public string Parameters { get; set; }

        private List<string> _messages;
        private IEventSource _eventSource;

        public MemoryLogger(List<string> messages)
        {
            Verbosity = LoggerVerbosity.Normal;
            _messages = messages;
        }

        public void Initialize(IEventSource eventSource)
        {
            _eventSource = eventSource;
            _eventSource.ErrorRaised += _eventSource_ErrorRaised;
            _eventSource.BuildFinished += _eventSource_BuildFinished;
        }

        private void _eventSource_BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            //_messages.Add($"{e.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")}, {e.Message}");
            _messages.Add(e.Message);
        }

        private void _eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            //_messages.Add($"{e.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")}, {e.Message}");
            _messages.Add(e.Message);
        }

        public void Shutdown()
        {
            _eventSource.ErrorRaised -= _eventSource_ErrorRaised;
            _eventSource.BuildFinished -= _eventSource_BuildFinished;
        }
    }
}
