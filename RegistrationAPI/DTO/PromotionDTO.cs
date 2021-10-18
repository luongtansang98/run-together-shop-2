using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class PromotionDTO
	{
		public int? Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PromotionTypeName { get; set; }
		public int PromotionTypeId { get; set; }
		public string Value { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool CanApplyForAll { get; set; }
		public bool IsDisable { get; set; }
	}
}
