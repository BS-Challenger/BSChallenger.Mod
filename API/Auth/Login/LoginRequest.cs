using BSChallenger.Server.Models.API.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.API.User
{
	internal class LoginRequest : AuthenticatedRequest
	{
		public LoginRequest(string blToken) : base(blToken) { }
	}
}
