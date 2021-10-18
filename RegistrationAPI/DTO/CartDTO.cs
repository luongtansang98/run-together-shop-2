using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class CartDTO
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ImagePath { get; set; }
		public string ProductCode { get; set; }
		public double Price { get; set; }
		public double TotalPrice { get; set;} 
		public int Quantity { get; set; }
		public int CustomerId { get; set; }
		public Double? PriceWithDiscount { get; set; }
	}
}
