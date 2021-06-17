using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.DTO;
using RegistrationAPI.Models;

namespace RegistrationAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartsController : ControllerBase
	{
		private readonly AuthenticationContext _context;
		public CartsController(AuthenticationContext context)
		{
			_context = context;
		}
		[HttpPost]
		[Route("update")]
		public IActionResult UpdateCart(CartDTO cart)
		{
			try
			{
				if (cart == null)
				{
					return BadRequest("Bị lỗi không có giỏ hàng");
				}

				if (!ModelState.IsValid)
				{
					return BadRequest("Giỏ hàng đang bị lỗi");
				}
				var isAlreadyExists = _context.Carts.Where(c => c.ProductId == cart.ProductId && c.CustomerId == cart.CustomerId).FirstOrDefault();
				if (isAlreadyExists != null)
				{
					isAlreadyExists.Quantity += cart.Quantity;
					_context.Update(isAlreadyExists);
				}
				else
				{
					var entity = new Cart()
					{
						ProductId = cart.ProductId,
						CustomerId = cart.CustomerId,
						Quantity = cart.Quantity,
					};
					_context.Add(entity);
				}

				_context.SaveChanges();

				return StatusCode(201);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		[HttpPost]
		[Route("GetList")]
		public IActionResult GetList(CartRequest req)
		{
			try
			{
				var lstResult = _context.Carts
					.Include(c => c.Product)
					.ThenInclude(p => p.ImagesList)
					.Where(c => c.CustomerId == req.CustomerId)
					.Select(c => new CartDTO()
					{
						Id = c.Id,
						ImagePath = c.Product.ImagesList.FirstOrDefault().ImagePath.ToString(),
						ProductCode = c.Product.Code,
						ProductName = c.Product.Name,
						Price = c.Product.PriceExport,
						Quantity = c.Quantity,
						TotalPrice = c.Quantity * c.Product.PriceExport
					}).ToList();
				return Ok(lstResult);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
		[HttpPost]
		[Route("deleteItem")]
		public IActionResult DeleteItem(CartRequest req)
		{
			try
			{
				var cart = _context.Carts.FirstOrDefault(c => c.CustomerId == req.CustomerId && c.Id == req.CartId);
				if(cart != null)
				{
					_context.Remove(cart);
					_context.SaveChanges();
					return Ok();
				}
				else
					return StatusCode(500, $"Có lỗi khi xóa giỏ hàng.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
	}
	public class CartRequest
	{
		public int CustomerId { get; set; }
		public int CartId { get; set; }
	}
}