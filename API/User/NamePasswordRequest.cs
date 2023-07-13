using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.API.User
{
	internal class NamePasswordRequest
	{
		public NamePasswordRequest(string username, string password)
		{
			Username = username;
			Password = password;
		}

		public string Username { get; set; }
		public string Password { get; set; }
	}
}
