using BSChallenger.API;
using BSChallenger.API.User;
using BSChallenger.Server.Models.API.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zenject;

namespace BSChallenger.Providers
{
	public class ChallengeRankingApiProvider
	{
		private readonly HttpClient httpClient = new HttpClient();
		private const string BASE_URL = "https://BSChallenger.xyz/api/";

		[Inject]
		private TokenStorageProvider tokenStorageProvider = null;

		public void Rankings(Action<List<Ranking>> callback)
		{
			JsonHttpGetRequest(BASE_URL + "rankings", (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ranking>>(res);
				callback(obj);
			});
		}

		public void Login(string blToken, Action<LoginResponse> callback)
		{
			JsonHttpPostRequest(BASE_URL + "login", new LoginRequest(blToken), (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(res);
				if (!string.IsNullOrEmpty(obj.Token))
					tokenStorageProvider.StoreToken(obj.Token);
				Plugin.Log.Info(res);
				callback(obj);
			});
		}

		public void Identity(string token, Action<IdentityResponse> callback)
		{
			JsonHttpPostRequest(BASE_URL + "Authenticate/Identity", new AuthenticatedRequest(token), (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityResponse>(res);
				Plugin.Log.Info(res);
				callback(obj);
			});
		}

		public void Scan(string jwtToken, string ranking, Action<string> callback)
		{
			JsonHttpPostRequest(BASE_URL + "scan", new ScanRequest(jwtToken, ranking), (res) =>
			{
				Plugin.Log.Info(res);
			});
		}

		private void JsonHttpGetRequest(string url, Action<string> callback)
		{
			Task.Run(async () =>
			{
				var res = await httpClient.GetAsync(url);
				var stringRes = await res.Content.ReadAsStringAsync();
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