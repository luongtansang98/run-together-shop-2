using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string ImgPath { get; set; }
        public string PhoneNumber { get; set; }
        public int PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position Position { get; set; }
    }
}
