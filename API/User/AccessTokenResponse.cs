namespace BSChallenger.API.User
{
	internal class AccessTokenResponse
	{
		public string AccessToken { get; set; }
		public string NewRefreshToken { get; set; }
	}
}