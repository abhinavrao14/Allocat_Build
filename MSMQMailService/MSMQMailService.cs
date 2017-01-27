using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

#region Added Namespaces

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Threading;
using System.Messaging;
using System.Net;
using System.Net.Mail;
using MSMQ_MailRely;

#endregion

namespace MSMQMailService
{
    public partial class MSMQMailService : ServiceBase
    {
        #region Global Variables
        private LogEntry logEntry = null;
        private ConfigurationReader configurationReader = null;
        private MessageQueue msgQueue;
        bool isLogFailed = default(bool);
        private Thread MSMQMessageProcessor;
        
        internal enum RetryReason
        {
            NoRetry,
            Network,
            Postmaster
        };

        #endregion

        public MSMQMailService()
        {
            this.ServiceName = "MSMQ Mail Service";
            this.AutoLog = true;
            this.EventLog.Log = "Application";
            this.EventLog.Source = this.ServiceName;
            this.CanShutdown = true;
            InitializeComponent();           
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //Starting the service 
                isLogFailed = false;
                logEntry = new LogEntry { Message = "Service is starting up", Severity = TraceEventType.Information };
                WriteLog(logEntry);

                logEntry = new LogEntry { Message = "Configurations Reading start...", Severity = TraceEventType.Information };
                WriteLog(logEntry);
                configurationReader = new ConfigurationReader();
                logEntry = new LogEntry { Message = "Configurations Read Successfully", Severity = TraceEventType.Information };
                WriteLog(logEntry); 
            }
            catch(Exception loggingEx)
            {
                //================================================================================================
                logEntry = new LogEntry { Message = "***************** Exception *****************", Severity = TraceEventType.Error };
                WriteLog(logEntry);
                logEntry = new LogEntry { Message = "Exception in Configuration Read : " + loggingEx.Message, Severity = TraceEventType.Critical };
                WriteLog(logEntry);
                logEntry = new LogEntry { Message = "Inner Exception : " + loggingEx.InnerException, Severity = TraceEventType.Error };
                WriteLog(logEntry);
                logEntry = new LogEntry { Message = "Stack Trace : " + loggingEx.StackTrace, Severity = TraceEventType.Error };
                WriteLog(logEntry);               
                //================================================================================================
            }
            try
            {                
                if (!MessageQueue.Exists(configurationReader.QueueName))
                {
                    logEntry = new LogEntry { Message = "MSM Queue '{0}' does not exist, trying to create it." + configurationReader.QueueName };
                    WriteLog(logEntry);
                    
                    msgQueue = MessageQueue.Create(configurationReader.QueueName);
                    msgQueue.Authenticate = Convert.ToBoolean(configurationReader.UseAuthentication);
                    msgQueue.Label = "MSMQ Mail Rely message queue";

                    logEntry = new LogEntry { Message = "Successfully created MSM queue: " + configurationReader.QueueName };
                    WriteLog(logEntry);
                }
                else
                {
                    msgQueue = new MessageQueue(configurationReader.QueueName);                   
                    logEntry = new LogEntry { Message = "Successfully attached to MSM queue: " + configurationReader.QueueName };
                    WriteLog(logEntry);
                }

                msgQueue.Formatter = new BinaryMessageFormatter();
                msgQueue.MessageReadPropertyFilter.SetAll();
                msgQueue.DenySharedReceive = true;

                MSMQMessageProcessor = new Thread(new ThreadStart(this.ProcessMSMQMessages));
                MSMQMessageProcessor.Start();              
            }
            catch (Exception loggingEx)
            {
                
                //================================================================================================
                logEntry = new LogEntry { Message = "***************** Exception *****************", Severity = TraceEventType.Error };
                WriteLog(logEntry);
                logEntry = new LogEntry { Message = "Could not start the service : " + loggingEx.Message, Severity = TraceEventType.Critical };
                WriteLog(logEntry);
                logEntry = new LogEntry { Message = "Inner Exception : " + loggingEx.InnerException, Severity = TraceEventType.Error };
                WriteLog(logEntry);
                logEntry = new LogEntry { Message = "Stack Trace : " + loggingEx.StackTrace, Severity = TraceEventType.Error };
                WriteLog(logEntry);
                //================================================================================================
                throw loggingEx;
            }
        }

        protected override void OnStop()
        {
            logEntry = new LogEntry { Message = "Service is stopping now ..... " };
            WriteLog(logEntry);
            MSMQMessageProcessor.Abort();
            MSMQMessageProcessor.Join();
            base.OnStop();
        }

        private void ProcessMSMQMessages()
        {
            try
            {
                while (true)
                {
                    Message message = msgQueue.Peek(); //wait for available messages
                    String ID = msgQueue.GetScheduledMessageID(); //is there any scheduled message
                    if (ID != null)
                    {
                        message = msgQueue.ReceiveById(ID);
                        MailMessage mailMessage = (message.Body as SerializeableMailMessage).GetMailMessage();

                        logEntry = new LogEntry { Message = "Message received : " + message.Label, Severity = TraceEventType.Information };
                        WriteLog(logEntry);
                        Exception CachedException = null;

                        RetryReason retry = RetryReason.NoRetry;
                        try
                        {
                            using (var smtpClient = new SmtpClient())
                            {
                                // Do not forget to set up SMTP parameters on app.config
                                smtpClient.Send(mailMessage);               
                                logEntry = new LogEntry { Message = "Message '{0}' sent at attempt {1}." + message.Label + message.AppSpecific, Severity = TraceEventType.Information };
                                WriteLog(logEntry);
                            }
                        }
                        catch (SmtpFailedRecipientsException ex)
                        {
                            CachedException = ex;
                            for (int i = 0; i < ex.InnerExceptions.Length; i++)
                            {
                                SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                                if (status == SmtpStatusCode.MailboxBusy ||
                                    status == SmtpStatusCode.MailboxUnavailable ||
                                    status == SmtpStatusCode.InsufficientStorage)
                                {
                                    retry = RetryReason.Postmaster;
                                }
                            }
                        }
                        catch (SmtpException ex)
                        {
                            CachedException = ex;
                            if (ex.InnerException != null)
                            {
                                WebExceptionStatus status = (ex.InnerException as WebException).Status;
                                if (status == System.Net.WebExceptionStatus.NameResolutionFailure ||
                                        status == System.Net.WebExceptionStatus.ConnectFailure)
                                {
                                    retry = RetryReason.Network;
                                }
                            }
                        }
                        catch (Exception loggingEx)
                        {
                            CachedException = loggingEx;                
                            //================================================================================================
                            logEntry = new LogEntry { Message = "***************** Exception *****************", Severity = TraceEventType.Error };
                            WriteLog(logEntry);
                            logEntry = new LogEntry { Message = "General error sending email : " + loggingEx.Message, Severity = TraceEventType.Critical };
                            WriteLog(logEntry);
                            logEntry = new LogEntry { Message = "Inner Exception : " + loggingEx.InnerException, Severity = TraceEventType.Error };
                            WriteLog(logEntry);
                            logEntry = new LogEntry { Message = "Stack Trace : " + loggingEx.StackTrace, Severity = TraceEventType.Error };
                            WriteLog(logEntry);
                            //================================================================================================
                            throw loggingEx;
                        }

                        if (CachedException != null)
                        {
                            if (retry != RetryReason.NoRetry)
                            {
                                if (message.AppSpecific > 0)
                                {
                                    // update schedule time
                                    DateTime OriginalScheduledTime = DateTime.FromBinary(BitConverter.ToInt64(message.Extension, 0));
                                    int retryDelaySeconds;
                                    if (retry == RetryReason.Network)
                                    {
                                        retryDelaySeconds = Convert.ToInt32(configurationReader.NetworkRetryDelay_s);
                                    }
                                    else
                                    {
                                        retryDelaySeconds = Convert.ToInt32(configurationReader.PostmasterRetryDelay_s);
                                    }
                                    message.Extension = System.BitConverter.GetBytes(DateTime.UtcNow.ToUniversalTime().AddSeconds(retryDelaySeconds).ToBinary());
                                    // update TTL
                                    message.AppSpecific--;
                                    // postpone message
                                    msgQueue.Send(message);
                                    logEntry = new LogEntry{Message = "Failed to deliver, but there are {0} more chances... retrying {1} seconds later" + message.AppSpecific + retryDelaySeconds, Severity = TraceEventType.Warning};
                                    WriteLog(logEntry);
                                }
                                else
                                {
                                  
                                    //================================================================================================
                                    logEntry = new LogEntry { Message = "***************** Exception *****************", Severity = TraceEventType.Error };
                                    WriteLog(logEntry);
                                    logEntry = new LogEntry { Message = "Failed to deliver, no more attempts : " + CachedException.Message, Severity = TraceEventType.Critical };
                                    WriteLog(logEntry);
                                    logEntry = new LogEntry { Message = "Inner Exception : " + CachedException.InnerException, Severity = TraceEventType.Error };
                                    WriteLog(logEntry);
                                    logEntry = new LogEntry { Message = "Stack Trace : " + CachedException.StackTrace, Severity = TraceEventType.Error };
                                    WriteLog(logEntry);
                                    //================================================================================================
                                }
                            }
                            else
                            {
                      
                                //================================================================================================
                                logEntry = new LogEntry { Message = "***************** Exception *****************", Severity = TraceEventType.Error };
                                WriteLog(logEntry);
                                logEntry = new LogEntry { Message = "Failed to deliver, but no use to retry : " + CachedException.Message, Severity = TraceEventType.Critical };
                                WriteLog(logEntry);
                                logEntry = new LogEntry { Message = "Inner Exception : " + CachedException.InnerException, Severity = TraceEventType.Error };
                                WriteLog(logEntry);
                                logEntry = new LogEntry { Message = "Stack Trace : " + CachedException.StackTrace, Severity = TraceEventType.Error };
                                WriteLog(logEntry);
                                //================================================================================================
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(Convert.ToInt32(configurationReader.SleepInterval));
                    }
                }
            }
            catch (ThreadAbortException)
            {
                logEntry = new LogEntry { Message = "Thread aborted", Severity = TraceEventType.Error };
                WriteLog(logEntry);

            }
        }

        /// <summary>
        /// Writing log entry into log file and send email in case of log failure
        /// </summary>
        /// <param name="logObject"></param>
        private void WriteLog(LogEntry logObject)
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
