namespace BSChallenger.API.Scan
{
	internal class ScanRequest
	{
		public ScanRequest(string ranking)
		{
			Ranking = ranking;
		}
		public string Ranking { get; set; }
	}
}
