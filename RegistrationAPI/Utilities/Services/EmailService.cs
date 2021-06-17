using RegistrationAPI.Utilities.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Utilities.Services
{
	public class EmailService: IEmailService
	{
		//private readonly AppSettings _appSettings;

        public void Send(string from, string to, string subject, string html)
        {
            // create message
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse(from));
            //email.To.Add(MailboxAddress.Parse(to));
            //email.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = html };

            //// send email
            //using var smtp = new SmtpClient();
            //smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            //smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            //smtp.Send(email);
            //smtp.Disconnect(true);
        }
    }
}
