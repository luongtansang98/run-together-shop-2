using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Utilities.Services.Interface
{
	public interface IEmailService
	{
		void Send(string from, string to, string subject, string html);
	}
}
