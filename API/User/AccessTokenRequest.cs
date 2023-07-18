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