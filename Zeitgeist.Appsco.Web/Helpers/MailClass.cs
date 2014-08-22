using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Zeitgeist.Appsco.Web.Helpers
{
    public class MailClass
    {
        private readonly string _acount;
        private readonly string _pass;

        public MailClass(string acount, string pass)
        {
            _acount = acount;
            _pass = pass;
        }

        public bool Send(string destinatarios,string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 60000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_acount, _pass);
            client.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            MailMessage mm = new MailMessage(_acount, destinatarios);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mm.Subject = "";
            mm.Body = "";

            client.Send(mm);
            return true;
        }
    }
}