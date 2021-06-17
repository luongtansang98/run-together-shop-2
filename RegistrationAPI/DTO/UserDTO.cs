using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.DTO
{
	public class UserDTO
	{
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImgPath { get; set; }
        public string PhoneNumber { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public PositionDTO Position { get; set; }
    }
}
