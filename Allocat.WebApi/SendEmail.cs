using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;

/// <summary>
/// Summary description for SendEmail
/// Article by vithal wadje http://www.c-sharpcorner.com/Authors/0c1bb2/
/// Facebook Profile:   www.facebook.com/vithal.wadje
//twitter Profile      :https://twitter.com/vithalwadjeC97
//LinedIn Profile    : http://www.linkedin.com/pub/vithal-wadje/69/83a/330

/// </summary>
public static class SendEmail
{
    public static string Pass, FromEmailid, HostAdd;

    public static void Email_Without_Attachment(String ToEmail, String Subj, string Message)
    {
        //Reading sender Email credential from web.config file
        HostAdd = ConfigurationManager.AppSettings["Host"].ToString();
        FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
        Pass = ConfigurationManager.AppSettings["Password"].ToString();

        //creating the object of MailMessage
        MailMessage mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(FromEmailid); //From Email Id
        mailMessage.Subject = Subj; //Subject of Email
        mailMessage.Body = Message; //body or message of Email
        mailMessage.IsBodyHtml = true;
        mailMessage.To.Add(new MailAddress(ToEmail)); //reciver's Email Id

        SmtpClient smtp = new SmtpClient(); // creating object of smptpclient
        smtp.Host = HostAdd; //host of emailaddress for example smtp.gmail.com etc

        //network and security related credentials

        smtp.EnableSsl = true;
        NetworkCredential NetworkCred = new NetworkCredential();
        NetworkCred.UserName = mailMessage.From.Address;
        NetworkCred.Password = Pass;
        smtp.UseDefaultCredentials = true;
        smtp.Credentials = NetworkCred;
        smtp.Port = 587;
        smtp.Send(mailMessage); //sending Email
    }

}