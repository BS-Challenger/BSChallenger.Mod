using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace BSChallenger.Providers
{
	//Zips token because it makes me feel better
	internal class RefreshTokenStorageProvider
	{
		private static string TokenPath => Path.Combine(Application.persistentDataPath, "challengertoken");
		internal void StoreRefreshToken(string token)
		{
			Plugin.Log.Info(token);
			var bytes = Encoding.UTF8.GetBytes(token);
			using (var memoryStream = new MemoryStream())
			{
				using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
				}
				var zippedBytes = memoryStream.ToArray();
				File.Delete(TokenPath);
				File.WriteAllBytes(TokenPath, zippedBytes);
				Plugin.Log.Info("Storing: " + token);
			}
		}

		internal string GetRefreshToken()
		{
			var zippedBytes = File.ReadAllBytes(TokenPath);
			using (var memoryStream = new MemoryStream(zippedBytes))
			{
				using (var outputStream = new MemoryStream())
				{
					using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
					{
						decompressStream.CopyTo(outputStream);
					}
					Plugin.Log.Info("Fetched: " + Encoding.UTF8.GetString(outputStream.ToArray()));
					return Encoding.UTF8.GetString(outputStream.ToArray());
				}
			}
		}

		internal bool RefreshTokenExists()
		{
			return File.Exists(TokenPath);
		}
	}
}