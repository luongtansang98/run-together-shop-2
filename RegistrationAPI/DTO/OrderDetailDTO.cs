using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class OrderDetailDTO
	{
		public int? Id { get; set; }
		public int OrderId { get; set; }
		public OrderDTO Order { get; set; }
		public int ProductId { get; set; }
		public ProductDTO Product { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public double TotalPrice { get; set; }
	}
}
