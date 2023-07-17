using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.API.User
{
	internal class IdentityRequest
	{
		public IdentityRequest(string accessToken)
		{
			AccessToken = accessToken;
		}

		public string AccessToken { get; set; }
	}
}
