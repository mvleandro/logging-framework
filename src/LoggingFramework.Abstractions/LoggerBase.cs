using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenJobs.LoggingFramework.Abstractions
{
    /// <summary>
    /// A base class for loggers.
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        /// <summary>
        /// Persists the logEntry in the destination.
        /// </summary>
        /// <param name="logEntry"></param>
        /// <returns></returns>
        public abstract Task Log(LogEntry logEntry);
    }
}
