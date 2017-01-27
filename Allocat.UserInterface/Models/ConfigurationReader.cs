using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Added Namespaces

using System.Configuration;

#endregion

namespace Allocat.UserInterface.Models
{
   public class ConfigurationReader
    {
        // Email configuration Keys
        public readonly string EmailTurnOn;
        public readonly string EmailTo;
        public readonly string EmailCC;
        public readonly string EmailBcc;
        public readonly string FailureEmailTo;
        public readonly string FailureEmailCC;
        public readonly string FailureEmailBcc;
        public readonly string EmailFrom;
      

        public ConfigurationReader()
        {
            try
            {
                var emailSection = (System.Collections.Specialized.NameValueCollection)ConfigurationManager.GetSection("EmailSettings");
                if (emailSection != null)
                {
                    
                    EmailTurnOn = emailSection["EmailTurnOn"];
                    EmailTo = emailSection["EmailTo"];
                    EmailCC = emailSection["EmailCC"];
                    EmailBcc = emailSection["EmailBcc"];

                    FailureEmailTo = emailSection["FailureEmailTo"];
                    FailureEmailCC = emailSection["FailureEmailCC"];
                    FailureEmailBcc = emailSection["FailureEmailBcc"];

                    EmailFrom = emailSection["EmailFrom"];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
