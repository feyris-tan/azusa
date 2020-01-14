using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql.Logging;

namespace moe.yo3explorer.azusa.Control.DatabaseIO.Drivers
{
    [DebuggerStepThrough]
    class PostgresLogProvider : INpgsqlLoggingProvider
    {
        private Dictionary<string, NpgsqlLogger> loggers;
        private Queue<PostgresLogEvent> logEvents;

        [DebuggerStepThrough]
        public NpgsqlLogger CreateLogger(string name)
        {
            if (loggers == null)
                loggers = new Dictionary<string, NpgsqlLogger>();

            if (loggers.ContainsKey(name))
                return loggers[name];

            loggers[name] = new PostgresLogger(name, PerformAddEvent);
            return loggers[name];
        }

        [DebuggerStepThrough]
        private void PerformAddEvent(PostgresLogEvent theEvent)
        {
            if (logEvents == null)
                logEvents = new Queue<PostgresLogEvent>();
            logEvents.Enqueue(theEvent);
        }
        delegate void AddEvent(PostgresLogEvent theEvent);

        [DebuggerStepThrough]
        class PostgresLogger : NpgsqlLogger
        {
            [DebuggerStepThrough]
            public PostgresLogger(string name,AddEvent addEvent)
            {
                this.name = name;
                this.addEvent = addEvent;
            }

            private string name;
            private AddEvent addEvent;

            [DebuggerStepThrough]
            public override bool IsEnabled(NpgsqlLogLevel level)
            {
                return true;
            }

            [DebuggerStepThrough]
            public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception exception = null)
            {
                PostgresLogEvent theEvent = new PostgresLogEvent(DateTime.Now, name, level, connectorId, msg, exception);
                addEvent(theEvent);
            }
        }

        [Serializable]
        [DebuggerStepThrough]
        class PostgresLogEvent
        {
            [DebuggerStepThrough]
            public PostgresLogEvent(DateTime now, string name, NpgsqlLogLevel level, int connectorId, string msg,
                Exception exception)
            {
                this.now = now;
                this.level = level;
                this.connectorId = connectorId;
                this.msg = msg;
                this.exception = exception;
                this.loggerName = name;
            }

            private DateTime now;
            private NpgsqlLogLevel level;
            private int connectorId;
            private string msg;
            private Exception exception;
            private string loggerName;

            public DateTime Now
            {
                get => now;
                set => now = value;
            }

            public NpgsqlLogLevel Level
            {
                get => level;
                set => level = value;
            }

            public int ConnectorId
            {
                get => connectorId;
                set => connectorId = value;
            }

            public string Msg
            {
                get => msg;
                set => msg = value;
            }

            public Exception Exception
            {
                get => exception;
                set => exception = value;
            }

            public string LoggerName
            {
                get => loggerName;
                set => loggerName = value;
            }
        }
    }
}