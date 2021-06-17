using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RegistrationAPI.Utilities.Services;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicKeyController : ControllerBase
    {
        private readonly PushNotificationsOptions _options;

        public PublicKeyController(IOptions<PushNotificationsOptions> options)
        {
            _options = options.Value;
        }

        public ContentResult Get()
        {
            return Content(_options.PublicKey, "text/plain");
        }
    }
}