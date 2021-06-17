using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class Cart:BaseEntity
	{
		//public int Id { get; set; }
		//public DateTime CreatedAt { get; set; }
		//public DateTime UpdatedAt { get; set; }
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
		public int Quantity { get; set; }
		public int CustomerId { get; set; }
		[ForeignKey("CustomerId")]
		public Customer Customer { get; set; }
	}
}
