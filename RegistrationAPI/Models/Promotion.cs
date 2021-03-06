using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class Promotion:BaseEntity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public int PromotionTypeId { get; set; }
		[ForeignKey("PromotionTypeId")]
		public PromotionType PromotionType { get; set; }

		public string Value { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool CanApplyForAll { get; set; }
		public bool IsDisable { get; set; }
	}
}
