using BeatSaverSharp;
using BeatSaverSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSChallenger.Utils
{
	internal static class BeatSaverAPI
	{
		private static BeatSaver beatSaver = new BeatSaver("BeatSaber Challenger", new Version(1, 0, 0));
		internal static void FetchBeatmap(string hash, Action<Beatmap> callback)
		{
			Task.Run(async () =>
			{
				var beatmaps = await beatSaver.BeatmapByHash(hash);
				callback(beatmaps);
			});
		}
	}
}
