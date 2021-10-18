using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class ProductQueryDTO
	{
		public int? Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Double PriceImport { get; set; }
		public Double PriceExport { get; set; }
		public int ProductGroupId { get; set; } //nhom Nam/Nu/Tre 
		public int ColorId { get; set; }
		public int CategoryId { get; set; }
		public int CountImage { get; set; }
		public string FirstImagePath { get; set; }
		public int? PromotionId { get; set; }
		public string Value { get; set; }
		public int? PromotionTypeId { get; set; }
		[NotMapped]
		public Double? PriceWithDiscount { get; set; }
	}
}
