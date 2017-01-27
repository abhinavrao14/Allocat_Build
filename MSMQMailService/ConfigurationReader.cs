using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MSMQMailService
{
    public class ConfigurationReader
    {
        // Email configuration Keys
        //public readonly string EmailTurnOn;
        //public readonly string EmailTo;
        //public readonly string EmailCC;
        //public readonly string EmailBcc;
        //public readonly string FailureEmailTo;
        //public readonly string FailureEmailCC;
        //public readonly string FailureEmailBcc;
        //public readonly string EmailFrom;
        //public readonly string SmptServer;
        //public readonly string SmtpPort;

        // Service Properties Settings Keys
        public readonly string QueueName;
        public readonly string SleepInterval;
        public readonly string UseAuthentication;
        public readonly string NetworkRetryDelay_s;
        public readonly string PostmasterRetryDelay_s;

         public ConfigurationReader()
        {
           try
            {
                //var emailSection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("EmailSettings");
                //if (emailSection != null)
                //{
                //    EmailTurnOn = emailSection["EmailTurnOn"];
                //    EmailTo = emailSection["EmailTo"];
                //    EmailCC = emailSection["EmailCC"];
                //    EmailBcc = emailSection["EmailBcc"];

                //    FailureEmailTo = emailSection["FailureEmailTo"];
                //    FailureEmailCC = emailSection["FailureEmailCC"];
                //    FailureEmailBcc = emailSection["FailureEmailBcc"];

                //    EmailFrom = emailSection["EmailFrom"];
                //    SmptServer = emailSection["SmptServer"];
                //    SmtpPort = emailSection["SmtpPort"];                   
                //}

               var servicePropertiesSettingsSection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("ServicePropertiesSettings");
               if (servicePropertiesSettingsSection != null)
               {
                   QueueName = servicePropertiesSettingsSection["QueueName"];
                   SleepInterval = servicePropertiesSettingsSection["SleepInterval"];
                   UseAuthentication = servicePropertiesSettingsSection["QueueName"];
                   NetworkRetryDelay_s = servicePropertiesSettingsSection["NetworkRetryDelay_s"];
                   PostmasterRetryDelay_s = servicePropertiesSettingsSection["PostmasterRetryDelay_s"];
               }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
