using BSChallenger.Server.Models.API.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.API.User
{
	internal class ScanRequest : AuthenticatedRequest
	{
		public ScanRequest(string jwtToken, string ranking) : base(jwtToken)
		{
			Ranking = ranking;
		}
		public string Ranking { get; set; }
	}
}
