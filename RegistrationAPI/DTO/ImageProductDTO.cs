using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class ImageProductDTO
	{
		public int? Id { get; set; }
		public string ImagePath { get; set; }
		public int Order { get; set; }
		public int ProductId { get; set; }

		public ProductDTO Product { get; set; }
	}
}
