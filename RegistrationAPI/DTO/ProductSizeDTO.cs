using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class ProductSizeDTO
	{
		public int? Id { get; set; }
		public int SizeNumber { get; set; }
		public int Count { get; set; }
		public bool Disabled { get; set; }
		public int ProductId { get; set; }
		public ProductSizeDTO(int Size, int Count, int ProductId, bool Disabled) 
		{
			this.SizeNumber = Size;
			this.Count = Count;
			this.ProductId = ProductId;
			this.Disabled = Disabled;
		}
	}
}
