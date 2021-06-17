using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class Member:BaseEntity
	{
		//public int Id { get; set; }
		//public DateTime CreatedAt { get; set; }
		//public DateTime UpdatedAt { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Address { get; set; }
	}
}
