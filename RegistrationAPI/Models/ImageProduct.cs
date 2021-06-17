using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class ImageProduct : BaseEntity
	{
		public string ImagePath { get; set; }
		public int Order { get; set; }
		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		public Product Product { get; set; }
	}
}
