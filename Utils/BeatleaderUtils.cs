using Newtonsoft.Json;
using SongDetailsCache.Structs;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BSChallenger.Utils
{
	internal static class BeatleaderUtils
	{
		private static readonly HttpClient httpClient = new HttpClient();
		internal static void GetLeaderboardID(string hash, Action<string> callback)
		{
			Task.Run(async () =>
			{
				var res = await httpClient.GetAsync("https://api.beatleader.xyz/leaderboards/hash/" + hash);
				var obj = JsonConvert.DeserializeObject<LeaderboardResult>(await res.Content.ReadAsStringAsync());
				callback(obj.song.id);
			});
		}
	}

	internal class LeaderboardResult
	{
		public LeaderboardResultSong song { get; set; }
	}
	internal class LeaderboardResultSong
	{
		public string id { get; set; }
	}
}