using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.DTO;
using RegistrationAPI.Models;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
		private readonly AuthenticationContext _context;
		private IHostingEnvironment _env;

		public EmployeesController(AuthenticationContext context, IHostingEnvironment env)
		{
			_context = context;
			_env = env;
		}
		[HttpPost("createOrUpdate")]
		public IActionResult PostEmployee(EmployeeDTO req)
		{
			var dir = _env.ContentRootPath;
			using (var fileStream = new FileStream(Path.Combine(dir, $"{req.Name}.png"), FileMode.Create, FileAccess.Write))
			{
				req.ImageOfEmployee.CopyTo(fileStream);
			}

			//// Tìm kiếm thiết bị/ phù tùng khách đã mua
			var objCommodity = new EmployeeDTO();
			
			return Ok(objCommodity);
		}
	}
}