using SongDetailsCache.Structs;
using System;

namespace BSChallenger.Utils
{
	internal static class SongDetailsUtils
	{
		internal static async void FetchBeatmap(string hash, Action<Song> callback)
		{
			var details = await SongDetailsCache.SongDetails.Init();
			Song song;
			if (details.songs.FindByHash(hash, out song))
			{
				callback(song);
			}
		}
	}
}