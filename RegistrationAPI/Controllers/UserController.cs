using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.DTO;
using RegistrationAPI.Models;
using RegistrationAPI.Utilities.Paging;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthenticationContext _context;

        public UserController(AuthenticationContext context)
        {
            _context = context;
        }

        [Route("GetList")]
        [HttpPost]
        public IActionResult GetAllUsers(SearchModel searchModel)
        {
            try
            {
                var users = _context.Users.Include(n => n.Position).Select(n => new UserDTO()
                {
                    Id = n.Id,
                    Name = n.Name,
                    ImgPath = n.ImgPath,
                    Address = n.Address,
                    PositionId = n.Position.Id,
                    PositionName = n.Position.Name,
                    PhoneNumber = n.PhoneNumber
                }).Where(n => n.Name.Contains(searchModel.CustomerName)
                        && (searchModel.PositionId.HasValue && searchModel.PositionId.Value > 0 ? n.PositionId == searchModel.PositionId : true));
                var results = PagingMethod.GetPaged(users, searchModel.Page);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [Route("GetPosition")]
        [HttpGet]
        public IActionResult GetAllPositions()
        {
            try
            {
                var positions = _context.Positions.Select(n => new 
                {
                    Id = n.Id,
                    Name = n.Name
                });
                return Ok(positions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                _context.Add(user);
                _context.SaveChanges();

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost]
        public IActionResult UpdateUser([FromBody]User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Nhân viên không tồn tại");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Dữ liệu không hợp lệ");
                }
                var userInDb = _context.Users.FirstOrDefault(n => n.Id == user.Id);
                if (userInDb == null)
                    return BadRequest("Nhân viên không tồn tại");
                userInDb.Name = user.Name;
                userInDb.Address = user.Address;
                userInDb.PhoneNumber = user.PhoneNumber;
                userInDb.PositionId = user.PositionId;
                userInDb.ImgPath = user.ImgPath;
                _context.Users.Update(userInDb);
                _context.SaveChanges();

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var userInDb = _context.Users.FirstOrDefault(n => n.Id == id);
            if (userInDb == null)
                return BadRequest("Nhân viên không tồn tại");
            var result = new UserDTO()
            {
                Id = userInDb.Id,
                Name = userInDb.Name,
                Address = userInDb.Address,
                PhoneNumber = userInDb.PhoneNumber,
                PositionId = userInDb.PositionId
            };
            return Ok(result);
        }
        public class SearchModel
        {
            public int Page { get; set; }
            public string CustomerName { get; set; }
            public int? PositionId { get; set; }
        }
        
    }
}