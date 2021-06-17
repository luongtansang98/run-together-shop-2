using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class CustomerDTO
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string FullName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
