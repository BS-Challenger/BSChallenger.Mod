using Newtonsoft.Json;
using SongDetailsCache.Structs;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BSChallenger.Utils
{
	internal static class BeatSaverUtils
	{
		private static readonly HttpClient httpClient = new HttpClient();
		internal static void GetMapperID(string name, Action<string> callback)
		{
			Task.Run(async () =>
			{
				var res = await httpClient.GetAsync("https://api.beatsaver.com/users/name/" + name);
				var obj = JsonConvert.DeserializeObject<MapperResult>(await res.Content.ReadAsStringAsync());
				callback(obj.id.ToString());
			});
		}
	}

	internal class MapperResult
	{
		public int id { get; set; }
	}
}