using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class ProductDTO
	{
		public int? Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int ViewCount { get; set; }
		public List<ImageProductDTO> ImagesList { get; set; }
		public List<ProductSizeDTO> Sizes { get; set; }
		public double PriceImport { get; set; }
		public double PriceExport { get; set; }
		public int ProductGroupId { get; set; } //nhom Nam/Nu/Tre 
		public int ColorId { get; set; }
		public int CategoryId { get; set; }
		public int CountImage { get; set; }
		public string FirseImagePath { get; set; }
	}
}
