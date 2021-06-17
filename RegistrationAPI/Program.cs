using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace RegistrationAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();


			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Joey Tribbiani", "joey@friends.com"));
			message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "chandler@friends.com"));
			message.Subject = "How you doin'?";

			message.Body = new TextPart("plain")
			{
				Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
			};

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.friends.com", 587, false);

				// Note: only needed if the SMTP server requires authentication
				client.Authenticate("joey", "password");

				client.Send(message);
				client.Disconnect(true);
			}
			using (var client = new Pop3Client())
			{
				client.Connect("pop.friends.com", 110, false);

				client.Authenticate("joey", "password");

				for (int i = 0; i < client.Count; i++)
				{
					//const message = client.GetMessage(i);
					Console.WriteLine("Subject: {0}", message.Subject);
				}

				client.Disconnect(true);
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
