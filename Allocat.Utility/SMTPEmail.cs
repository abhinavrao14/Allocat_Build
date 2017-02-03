using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Allocat.Utility
{
    public class SMTPEmail
    {
        public void sendMail(MailBody mb)
        {
            try
            {
                string html, Subject = "";

                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(mb.ContactPersonEmailId, mb.ContactPersonName));

                // From
                mailMsg.From = new MailAddress("sachan.amita33@gmail.com", "David Hill");

                // Subject and multipart/alternative Body
                
                GetMailBody(mb,out html,out Subject);

                mailMsg.Subject = Subject;

                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("sindhut", "Sindhu@1234");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetMailBody(MailBody mb, out string html, out string Subject)
        {
            html = "";
            Subject = "";
            if (mb.MailType == "SendPassword")
            {
                //var appDomain = System.AppDomain.CurrentDomain;
                //var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
                //StreamReader reader = new StreamReader(Path.Combine(basePath, "Template", "PO.html"));
                //body = reader.ReadToEnd();

                html = @"<p>Your password is " + mb.Password + ".Please click on following link to login.<br /> http://localhost:63729/Account </p>";
                Subject = "Allocat - Password Changed.";
            }
            else if (mb.MailType == "VerifyUserRegistration")
            {
                html = @"<p>Your password is " + mb.Password + ".Please click on following link to verify your account.<br />  http://localhost:63729/Response/TissueBank_Verification_Successful?response=true&UserId=" + mb.UserId + "</p>";
                Subject = "Allocat - Email Verification.";
            }
        }
    }

    public class MailBody
    {
        public string Password { get; set; }
        public string ContactPersonEmailId { get; set; }
        public string ContactPersonName { get; set; }
        public int UserId { get; set; }
        public string MailType { get; set; }
    }
}
