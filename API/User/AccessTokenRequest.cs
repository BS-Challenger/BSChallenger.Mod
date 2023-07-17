using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.API.User
{
	internal class AccessTokenRequest
	{
		public AccessTokenRequest(string refreshToken)
		{
			RefreshToken = refreshToken;
		}

		public string RefreshToken { get; set; }
	}
}
