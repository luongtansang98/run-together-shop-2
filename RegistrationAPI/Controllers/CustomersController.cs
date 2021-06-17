using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using RegistrationAPI.DTO;
using RegistrationAPI.Models;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
		private readonly AuthenticationContext _context;
		IHostingEnvironment env = null;
		public CustomersController(AuthenticationContext context, IHostingEnvironment env)
		{
			_context = context;
			this.env = env;
		}
		[HttpPost]
		[Route("CreateAccount")]
		public IActionResult CreateAccount(CustomerDTO customer)
		{
			try
			{
				if (customer == null)
				{
					return BadRequest("Customer object is null");
				}

				if (!ModelState.IsValid)
				{
					return BadRequest("Invalid model object");
				}
				var entity = new Customer()
				{
					FullName = customer.FullName,
					Phone 	 = customer.Phone , 
					Email 	 = customer.Email,
					Address	 = customer.Address,
					City 	 = customer.City,
					UserName = customer.UserName,
					Password = customer.Password,
				};
				_context.Add(entity);
				_context.SaveChanges();

				return Ok(new { 
				entity.FullName,
				entity.Phone,
				entity.Email
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}

		[HttpPost]
		[Route("SendMail")]
		public IActionResult SendMail(CustomerDTO customer)
		{
			try
			{
				MimeMessage message = new MimeMessage();

				MailboxAddress from = new MailboxAddress("Admin",
				"tansang020798@gmail.com");
				message.From.Add(from);

				MailboxAddress to = new MailboxAddress("User",
				"sangdev@yopmail.com");
				message.To.Add(to);

				message.Subject = "Đơn hàng ";

				BodyBuilder bodyBuilder = new BodyBuilder();
				bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
				bodyBuilder.TextBody = "Hello World!";

				message.Body = bodyBuilder.ToMessageBody();

				SmtpClient client = new SmtpClient();
				client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate("tansang020798@gmail.com", "nhocprokemcoi123");

				client.Send(message);
				client.Disconnect(true);
				client.Dispose();
				return StatusCode(200);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}

		[HttpPost]
		[Route("login")]
		public IActionResult Login(CustomerDTO customer)
		{
			try
			{
				if (customer == null)
				{
					return BadRequest("Customer object is null");
				}

				if (!ModelState.IsValid)
				{
					return BadRequest("Invalid model object");
				}
				var validAccount = _context.Customers.FirstOrDefault(c => c.UserName == customer.UserName && c.Password == customer.Password);
				if (validAccount == null)
					return BadRequest("Tài khoản hoặc mật khẩu không đúng!");
				else
					return Ok(
						new
						{
							customer = new
							{
								customerId = validAccount.Id,
								fullName = validAccount.FullName,
								phone = validAccount.Phone,
								email = validAccount.Email,
								address = validAccount.Address,
							}
						}) ;
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
	}
}