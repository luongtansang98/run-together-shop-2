using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class EmailDetailDTO
	{
		public int? Id { get; set; }
		public string ProductName { get; set; }
		public double NumberOfProduct { get; set; }
		public double UnitPrice { get; set; }
		public double TotalPrice { get; set; }
	}
}
