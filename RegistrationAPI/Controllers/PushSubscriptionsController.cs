using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Utilities.Services.Interface;

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushSubscriptionsController : ControllerBase
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;

        public PushSubscriptionsController(IPushSubscriptionsService pushSubscriptionsService)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
        }

        [HttpPost]
        public void Post([FromBody] PushSubscription subscription)
        {
            _pushSubscriptionsService.Insert(subscription);
        }

        [HttpDelete("{endpoint}")]
        public void Delete(string endpoint)
        {
            _pushSubscriptionsService.Delete(WebUtility.UrlDecode(endpoint));
        }
    }
}