using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class EmployeeDTO
	{
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int? PositionId { get; set; }
        public PositionDTO Position { get; set; }
        public double Salary { get; set; }
        public IFormFile ImageOfEmployee { get; set; }
       // public string ImageOfEmployee { get; set; }
    }
}
