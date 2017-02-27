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
                mailMsg.From = new MailAddress("sachan.amita33@gmail.com", "Allocat Support");

                // Subject and multipart/alternative Body
                
                GetMailBody(mb,out html,out Subject);
                mailMsg.IsBodyHtml = true;
                mailMsg.Subject = Subject;

                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("XXXXX", "XXXXXXX");
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
            if (mb.MailType == "changePass")
            {
               // html = @"<p>Your password is " + mb.Password + ".<br /> <a href='http://localhost:63729/Account'> Please click here to login.</a></p>";
                html = @"<p>Dear " + mb.ContactPersonName + ",<br/>Your password has been modified.Your modified password is :" + mb.Password + ".</p>";
                Subject = "Allocat - Password Changed.";
            }
            else if (mb.MailType == "VerifyUserRegistration")
            {
               // html = @"<p>Your password is " + mb.Password + ".<br />  <a href='http://localhost:63729/Response/TissueBank_Verification_Successful?response=true&UserId=" + mb.UserId + ">Please click here to verify your account.</a></p>";
                html = @"<p><a href='http://allocat.net/Response/TissueBank_Verification_Successful?response=true&UserId=" + mb.UserId + "'>Please click here to verify your account.</a><br/>Once you  verify your account after clicking on above link,use " + mb.Password + " as your temporary password to login.</p>";
                Subject = "Allocat - Email Verification.";
            }
            else if (mb.MailType == "UpdateTissueBankDetail")
            {
                html = @"<p>Dear " + mb.ContactPersonName + ",<br/>Your tissue bank detail is updated.Please check.</p>";
                Subject = "Allocat - Tissue Bank Detail Updated.";
            }
            else if (mb.MailType == "UpdateBillingDetail")
            {
                html = @"<p>Dear " + mb.ContactPersonName + ",<br/>Billing information of your tissue Bank is updated.Please check.</p>";
                Subject = "Allocat - Billig Detail of Tissue Bank Updated.";
            }
            else if (mb.MailType == "TissueBank_Add")
            {
                html = @"<p>Dear "+mb.ContactPersonName+",<br/> Your transaction is successful and $25 is deducted from your account as registration fees.</p>";
                Subject = "Allocat - Registration Confirmed!";
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
