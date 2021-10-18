using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class PromotionType: BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}
}
