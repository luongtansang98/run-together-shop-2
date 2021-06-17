using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;

namespace RegistrationAPI.Utilities.Services.Interface
{
	public interface IPushSubscriptionsService
	{
		IEnumerable<PushSubscription> GetAll();

		void Insert(PushSubscription subscription);

		void Delete(string endpoint);
	}
}
