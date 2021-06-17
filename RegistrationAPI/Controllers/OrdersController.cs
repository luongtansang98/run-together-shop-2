using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using RegistrationAPI.DTO;
using RegistrationAPI.Models;

namespace RegistrationAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly AuthenticationContext _context;
		public OrdersController(AuthenticationContext context)
		{
			_context = context;
		}
		[HttpPost]
		[Route("CreateOrder")]
		public IActionResult CreateOrder(OrderDTO order)
		{
			try
			{
				if (order == null)
				{
					return BadRequest("Customer object is null");
				}

				if (!ModelState.IsValid)
				{
					return BadRequest("Invalid model object");
				}
				var entity = new Order()
				{
					CustomerId = order.CustomerId,
					OrderDate = DateTime.Now,
					ShipAddress = order.ShipAddress,
					ShipName = order.ShipName,
					ShipPhone = order.ShipPhone,
					ShipEmail = order.ShipEmail,
					DeliveryType = order.DeliveryType,
					PaymentType = order.PaymentType,
					OrderDetails = order.OrderDetails.Select(n => new OrderDetail()
					{
						ProductId = n.ProductId,
						Quantity = n.Quantity,
						Price = n.Price
					}).ToList()
				};
				_context.Add(entity);
				_context.SaveChanges();

				return Ok(entity);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		[HttpPost]
		[Route("CompleteOrder")]
		public IActionResult CompleteOrder(OrderDTO order)
		{
			try
			{
				var orderCode = this.RandomString(8);
				var price = 0;
				Order obj = new Order();
				obj.Code = orderCode;
				obj.CustomerId = order.CustomerId;
				obj.Status = OrderStatus.Shipping;
				obj.DeliveryType = order.DeliveryType;
				obj.PaymentType = order.PaymentType;

				//lấy ra giỏ hàng của KH đó
				var customerCarts = _context.Carts.Where(c => c.CustomerId == order.CustomerId).ToList();
				var productTest = _context.Products.FirstOrDefault().PriceExport;

				int totalBill = 0;

				var emailDetails = new List<EmailDetailDTO>();

				foreach (var item in customerCarts)
				{
					var product = _context.Products.Where(c => c.Id == item.ProductId).FirstOrDefault();

					var detail = new EmailDetailDTO();
					detail.ProductName = product.Name;
					detail.UnitPrice = product.PriceExport;
					detail.NumberOfProduct = item.Quantity;
					detail.TotalPrice = product.PriceExport * item.Quantity;

					emailDetails.Add(detail);

					obj.OrderDetails.Add(new OrderDetail
					{
						ProductId = item.ProductId,
						Quantity = item.Quantity
					});
				}
				_context.Carts.RemoveRange(customerCarts);
				_context.Add(obj);
				_context.SaveChanges();


				MimeMessage message = new MimeMessage();

				MailboxAddress from = new MailboxAddress("Cửa hàng giày Run Together",
				"dulieuao1964@gmail.com");
				message.From.Add(from);

				MailboxAddress to = new MailboxAddress("User",
				order.ShipEmail);
				message.To.Add(to);

				message.Subject = "Mã đơn hàng: " + orderCode;

				BodyBuilder bodyBuilder = new BodyBuilder();
				bodyBuilder.HtmlBody = this.GenerateHTMLEmail(emailDetails);

				message.Body = bodyBuilder.ToMessageBody();

				SmtpClient client = new SmtpClient();
				client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate("dulieuao1964@gmail.com", "dulieuao&&&&1964");

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

		public string RandomString(int length)
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public string GenerateHTMLEmail(List<EmailDetailDTO> details)
		{
			if (details == null) return "";

			string result = "";
			double totalBill = details.Sum(x => x.TotalPrice);
			foreach (var item in details)
			{
				result += "<p>Tên hàng: " + item.ProductName + "</p>" +
				"<p>Số lượng: " + item.NumberOfProduct + "</p >" +
				"<p>Đơn giá: " + item.UnitPrice.ToString("N0") + "</p >" +
				"<p>Thành tiền: " + item.TotalPrice.ToString("N0") + "</p >" +
				"<p>----------------------------------</p>";
			}
			result += "<p>Tổng hóa đơn: " + totalBill.ToString("N0") + "</p >";
			result += "------Cảm ơn quý khách đã ghé shop!-------";
			return result;

		}

	}
}