using OpenJobs.LoggingFramework.Abstractions;
using System;
using System.Threading.Tasks;

namespace OpenJobs.LoggingFramework.Aws.Sns
{
    /// <summary>
    /// An implementation of ILogger to publish logs in a SNS topic.
    /// </summary>
    public class Logger : LoggerBase
    {
        /// <summary>
        /// Sends the logEntry to a SNS topic.
        /// </summary>
        /// <param name="logEntry">The logEntry instance.</param>
        public override async Task Log(LogEntry logEntry)
        {
            throw new NotImplementedException();
        }
    }
}
