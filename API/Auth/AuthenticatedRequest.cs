namespace BSChallenger.Server.Models.API.Authentication
{
    public class AuthenticatedRequest
    {
		public AuthenticatedRequest(string token)
		{
			Token = token;
		}

		public string Token { get; set; }
    }
}
