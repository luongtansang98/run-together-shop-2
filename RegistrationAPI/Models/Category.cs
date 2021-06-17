using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class Category:BaseEntity
	{
		//public int Id { get; set; }
		//public DateTime CreatedAt { get; set; }
		//public DateTime UpdatedAt { get; set; }
		public string Name { get; set; }
	}
}
