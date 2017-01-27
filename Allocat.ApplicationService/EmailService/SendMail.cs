using MailSenderLibrary;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Diagnostics;
using System.Net.Mail;

namespace Allocat.ApplicationService.EmailService
{
    public class Email
    {
        public void SendEmail(string EmailTo)
        {
            Consumer objectConsumer = new Consumer();
            try
            {
                ConfigurationReader configurationReader = new ConfigurationReader();

                if (string.Equals(configurationReader.EmailTurnOn.ToLower(), "true"))
                {
                    EmailSender objEmailSender = new EmailSender(@".\Private$\EmailQueue");
                    MailMessage objMailMessage = new MailMessage();
                    objectConsumer.logEntry = new LogEntry { Message = "Configuration read : ", Severity = TraceEventType.Information };
                    objectConsumer.WriteLog(objectConsumer.logEntry);
                    objMailMessage.From = new MailAddress(configurationReader.EmailFrom);
                    char[] delimiters = new[] { ',', ';' };
                    bool mailSent = default(bool);

                    // EmailTo list
                    if (!string.IsNullOrEmpty(EmailTo))
                    {
                        if (EmailTo.Contains(",") || EmailTo.Contains(";"))
                        {
                            string[] emailToList = EmailTo.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var emailToItem in emailToList)
                            {
                                objMailMessage.To.Add(emailToItem);
                            }
                        }
                        else
                        {
                            objMailMessage.To.Add(EmailTo);
                        }
                    }

                    //EmailCC List
                    if (!string.IsNullOrEmpty(configurationReader.EmailCC))
                    {
                        if (configurationReader.EmailCC.Contains(",") || configurationReader.EmailCC.Contains(";"))
                        {
                            string[] emailCCList = configurationReader.EmailCC.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var emailCCItem in emailCCList)
                            {
                                objMailMessage.CC.Add(emailCCItem);
                            }
                        }
                        else
                        {
                            objMailMessage.CC.Add(configurationReader.EmailCC);
                        }
                    }

                    //EmailBCC List 
                    if (!string.IsNullOrEmpty(configurationReader.EmailBcc))
                    {
                        if (configurationReader.EmailBcc.Contains(",") || configurationReader.EmailBcc.Contains(";"))
                        {
                            string[] emailBCCList = configurationReader.EmailBcc.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var emailBCCItem in emailBCCList)
                            {
                                objMailMessage.To.Add(emailBCCItem);
                            }
                        }
                        else
                        {
                            objMailMessage.Bcc.Add(configurationReader.EmailBcc);
                        }
                    }

                    objMailMessage.Subject = "Sample message";
                    objMailMessage.IsBodyHtml = true;
                    objMailMessage.Body = @"<h1>This is sample</h1><a href=""http://http://www.codeproject.com"">See this link</a>";

                    objEmailSender.QueueMessage(objMailMessage);
                    objectConsumer.logEntry = new LogEntry { Message = "Successfully queued", Severity = TraceEventType.Information };
                    objectConsumer.WriteLog(objectConsumer.logEntry);
                }
            }
            catch (Exception loggingEx)
            {
                //================================================================================================
                objectConsumer.logEntry = new LogEntry { Message = "***************** Exception *****************", Severity = TraceEventType.Error };
                objectConsumer.WriteLog(objectConsumer.logEntry);
                objectConsumer.logEntry = new LogEntry { Message = "Could not start the service : " + loggingEx.Message, Severity = TraceEventType.Critical };
                objectConsumer.WriteLog(objectConsumer.logEntry);
                objectConsumer.logEntry = new LogEntry { Message = "Inner Exception : " + loggingEx.InnerException, Severity = TraceEventType.Error };
                objectConsumer.WriteLog(objectConsumer.logEntry);
                objectConsumer.logEntry = new LogEntry { Message = "Stack Trace : " + loggingEx.StackTrace, Severity = TraceEventType.Error };
                objectConsumer.WriteLog(objectConsumer.logEntry);
                //================================================================================================
            }
        }
    }
}
