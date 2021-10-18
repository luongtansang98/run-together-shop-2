
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrationAPI.Models
{
	public class PromotionProduct:BaseEntity
	{
		public int PromotionId { get; set; }
		[ForeignKey("PromotionId")]
		public Promotion Promotion { get; set; }

		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
	}
}
