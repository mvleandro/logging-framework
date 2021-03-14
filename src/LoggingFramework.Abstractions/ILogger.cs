using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenJobs.LoggingFramework.Abstractions
{
    /// <summary>
    /// Interface to a Logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Persists the LogEntry in the destination.
        /// </summary>
        /// <param name="logEntry">The LogEntry instance.</param>
        /// <returns>A task with the log action job.</returns>
        Task Log(LogEntry logEntry);
    }
}
