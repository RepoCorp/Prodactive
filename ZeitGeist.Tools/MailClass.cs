using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ZeitGeist.Tools
{
    public class MailClass
    {
        private readonly string _acount;
        private readonly string _pass;
        private readonly string _smtpMail;
        private readonly int    _smtpPort;

        public MailClass(string acount, string pass,string smtpMail,int smtpPort)
        {
            _acount   = acount;
            _pass     = pass;
            _smtpMail = smtpMail;
            _smtpPort = smtpPort;
        }

        public bool Send(string destinatarios, string subject, string body)
        {
            SmtpClient client     = new SmtpClient();
            client.Port           = _smtpPort;
            client.Host           = _smtpMail;
            client.EnableSsl      = true;
            client.Timeout        = 60000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials    = new System.Net.NetworkCredential(_acount, _pass);
            client.EnableSsl      = true;
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            MailMessage mm = new MailMessage(_acount, destinatarios);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mm.Subject = subject;
            mm.Body = body;

            client.Send(mm);
            return true;
        }

        //private string GetBody(Lista l, string url)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    int i = 0;
        //    sb.Append(String.Format("<b>Listado Para el Domingo {0}</b></br>", l.Fecha.ToString("yyyy-MM-dd")));
        //    sb.Append("<hr/></br>");
        //    sb.Append("<b>Canciones</b>");
        //    sb.Append("<ol>");
        //    foreach (var a in l.Canciones)
        //    {
        //        sb.Append(String.Format("<li>{0}  -  {1}</li>", a.Tipo, a.Nombre));
        //    }
        //    sb.Append("</ol></br>");
        //    sb.Append("<hr/></br>");
        //    if (l.Sugerencias.Count > 0)
        //    {
        //        sb.Append("<b>sugerencias</b>");
        //        sb.Append("<ol>");
        //        foreach (var a in l.Sugerencias)
        //        {
        //            sb.Append(String.Format("<li>{0}  -  {1}</li>", a.Tipo, a.Nombre));
        //        }
        //        sb.Append("</ol>");
        //    }
        //    //            sb.Append("<hr/></br>");
        //    sb.Append("<b>Ver Pagina</b></br>");
        //    sb.Append(string.Format("<a HREF='{0}'>Listado</a>", url));
        //    return sb.ToString();
        //}

    }
}