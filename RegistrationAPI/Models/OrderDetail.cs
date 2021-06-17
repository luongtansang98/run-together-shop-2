using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class OrderDetail:BaseEntity
	{
		public int OrderId { get; set; }
		[ForeignKey("OrderId")]
		public Order Order { get; set; }
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
	}
}
