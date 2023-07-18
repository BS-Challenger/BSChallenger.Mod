namespace BSChallenger.API.User
{
	public class AuthResponse
	{
		public string Response { get; set; }
		public bool IsValid { get; set; }
		public string RefreshToken { get; set; }
	}
}