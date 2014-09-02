using System;
using Elmah;
using NLog;
using NLog.Targets;

namespace ElmahR.Client
{
    [Target("Elmah")]
    public sealed class ElmahTarget : TargetWithLayout
    {
        private readonly ErrorLog _errorLog;

        public bool LogLevelAsType { get; set; }

        public ElmahTarget()
            : this(ErrorLog.GetDefault(null))
        { }

        public ElmahTarget(ErrorLog errorLog)
        {
            _errorLog = errorLog;
            LogLevelAsType = false;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);

            var error = logEvent.Exception == null ? new Error() : new Error(logEvent.Exception);
            var type = error.Exception != null
                ? error.Exception.GetType().FullName
                : LogLevelAsType ? logEvent.Level.Name : string.Empty;
            error.Type = type;
            error.Message = logMessage;
            error.Time = GetCurrentDateTime == null ? logEvent.TimeStamp : GetCurrentDateTime();
            error.HostName = Environment.MachineName;
            error.Detail = logEvent.Exception == null ? logMessage : logEvent.Exception.StackTrace;

            _errorLog.Log(error);
        }

        public Func<DateTime> GetCurrentDateTime { get; set; }
    }
}