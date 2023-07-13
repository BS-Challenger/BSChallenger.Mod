using BSChallenger.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSChallenger.Providers
{
	public class BSChallengeRankingAPIProvider : MonoBehaviour
	{
		private HttpClient httpClient = new HttpClient();
		private const string BASE_URL = "http://localhost:8080/";
		public void FetchRankings(Action<List<Ranking>> callback)
		{
			JsonHttpGetRequest(BASE_URL + "rankings", (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ranking>>(res);
				Plugin.Log.Info(obj[0].name);
				callback(obj);
			});
		}

		public void Signup(string name, string pass, Action callback)
		{
			JsonHttpPostRequest(BASE_URL + "accounts/Signup", new API.User.NamePasswordRequest(name, pass), (res) =>
			{
				Plugin.Log.Info(res);
				callback();
			});
		}

		public void Login(string name, string pass, Action callback)
		{
			JsonHttpPostRequest(BASE_URL + "accounts/Login", new API.User.NamePasswordRequest(name, pass), (res) =>
			{
				Plugin.Log.Info(res);
				callback();
			});
		}

		private void JsonHttpGetRequest(string url, Action<string> callback)
		{
			Task.Run(async () =>
			{
				var res = await httpClient.GetAsync(url);
				var stringRes = await res.Content.ReadAsStringAsync();
				Plugin.Log.Info(stringRes);
				callback(stringRes);
			});
		}

		private void JsonHttpPostRequest(string url, object obj, Action<string> callback)
		{
			Task.Run(async () =>
			{
				var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
				var res = await httpClient.PostAsync(url, content);
				var stringRes = await res.Content.ReadAsStringAsync();
				callback(stringRes);
			});
		}
	}
}
