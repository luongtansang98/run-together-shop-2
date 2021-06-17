using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class Promotion:BaseEntity
	{
		public string Name { get; set; }
		public string Status { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public bool ApplyForAll { get; set; }
		public int? DiscountPercent { get; set; }
		public double? DiscountAmount { get; set; }
		public int? GroupId { get; set; }
		public int? CategoryId { get; set; }
		public int? ProductId { get; set; }


	}
}
