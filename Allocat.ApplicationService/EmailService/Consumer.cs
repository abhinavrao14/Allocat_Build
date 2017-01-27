using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Allocat.ApplicationService.EmailService
{
    public class Consumer
    {
        public LogEntry logEntry = null;
        bool isLogFailed = default(bool);

        public void WriteLog(LogEntry logObject)
        {
            try
            {
                if (!isLogFailed)
                {
                    Logger.Write(logObject);
                }
            }
            catch (Exception logEx)
            {
                isLogFailed = true;
                logEntry = new LogEntry { Message = logEx.Message + Environment.NewLine + logEx.StackTrace, Severity = TraceEventType.Start };
                WriteLog(logEntry);
            }
        }

    }
}