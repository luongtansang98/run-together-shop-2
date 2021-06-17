using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class Product: BaseEntity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int ViewCount { get; set; }
		public ICollection<ImageProduct> ImagesList { get; set; }
		public double PriceImport { get; set; }
		public double PriceExport { get; set; }
		public int ProductGroupId { get; set; } //nhom Nam/Nu/Tre 
		public int ColorId { get; set; }
		public int CategoryId { get; set; }
	}
}
