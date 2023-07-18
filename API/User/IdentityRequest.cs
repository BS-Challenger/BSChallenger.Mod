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