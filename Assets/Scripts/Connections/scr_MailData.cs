using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class scr_MailData : MonoBehaviour {

	void SendMail(string Destination, string Subject, string Body)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("youraddress@gmail.com");
        mail.To.Add(Destination);
        mail.Subject = Subject;
        mail.Body = Body;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("youraddress@gmail.com", "yourpassword") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");
    }
}
