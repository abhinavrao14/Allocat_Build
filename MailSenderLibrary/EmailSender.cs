using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Added Namespaces
using System.Messaging;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using MSMQ_MailRely;
using System.Diagnostics;
#endregion

namespace MailSenderLibrary
{
    public class EmailSender
    {
        private MessageQueue msgQueue;
        private LogEntry logEntry = null;
        bool isLogFailed = default(bool);
        private bool useAuthentication;

        public EmailSender(string queueName, bool useAuthentication = false)
        {
            this.useAuthentication = useAuthentication;
            try
            {
                if (!MessageQueue.Exists(queueName))
                {
                    throw new ArgumentException("Cannot instantiate. MSM queue does not exist.");
                }

                msgQueue = new MessageQueue(queueName, QueueAccessMode.Send);
                msgQueue.Formatter = new BinaryMessageFormatter();
                msgQueue.DefaultPropertiesToSend.Recoverable = true;

                
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public void QueueMessage(MailMessage emailMessage, int deliveryAttempts = 3)
        {
            if (emailMessage == null)
            {               
                throw new ArgumentNullException("Cannot send empty message");
            }
            if (msgQueue == null)
            {
                throw new NullReferenceException("Message queue not attached");
            }

            try
            {
                Message message = new Message();
                message.Body = new SerializeableMailMessage(emailMessage);
                message.Recoverable = true;
                message.Formatter = new BinaryMessageFormatter();
                message.AppSpecific = deliveryAttempts; //used as TTL
                message.Extension = System.BitConverter.GetBytes(DateTime.UtcNow.ToBinary()); //scheduled delivery time, 'now' for the start
                message.Label = Guid.NewGuid().ToString();
                message.UseAuthentication = useAuthentication;
                msgQueue.Send(message);                
            }
            catch (Exception ex)
            {
              throw ex;
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
