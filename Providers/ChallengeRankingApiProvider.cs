using BSChallenger.API;
using BSChallenger.API.User;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace BSChallenger.Providers
{
	public class ChallengeRankingApiProvider
	{
		private readonly HttpClient httpClient = new HttpClient();
		private const string BASE_URL = "http://localhost:8081/";

		[Inject]
		private RefreshTokenStorageProvider refreshTokenStorageProvider = null;
		public void FetchRankings(Action<List<Ranking>> callback)
		{
			JsonHttpGetRequest(BASE_URL + "rankings", (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ranking>>(res);
				callback(obj);
			});
		}

		public void Signup(string name, string pass, Action<AuthResponse> callback)
		{
			JsonHttpPostRequest(BASE_URL + "Authenticate/Signup", new NamePasswordRequest(name, pass), (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthResponse>(res);
				if (!string.IsNullOrEmpty(obj.RefreshToken))
					refreshTokenStorageProvider.StoreRefreshToken(obj.RefreshToken);
				Plugin.Log.Info(res);
				callback(obj);
			});
		}

		public void Login(string name, string pass, Action<AuthResponse> callback)
		{
			JsonHttpPostRequest(BASE_URL + "Authenticate/Login", new NamePasswordRequest(name, pass), (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthResponse>(res);
				if (!string.IsNullOrEmpty(obj.RefreshToken))
					refreshTokenStorageProvider.StoreRefreshToken(obj.RefreshToken);
				Plugin.Log.Info(res);
				callback(obj);
			});
		}

		public void Identity(string token, Action<IdentityResponse> callback)
		{
			JsonHttpPostRequest(BASE_URL + "Authenticate/Identity", new IdentityRequest(token), (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityResponse>(res);
				Plugin.Log.Info(res);
				callback(obj);
			});
		}

		public void AccessToken(string refreshToken, Action<string> callback)
		{
			JsonHttpPostRequest(BASE_URL + "Authenticate/AccessToken", new AccessTokenRequest(refreshToken), (res) =>
			{
				var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessTokenResponse>(res);
				Plugin.Log.Info("Balls:" + obj.NewRefreshToken + ":");

				if (!string.IsNullOrEmpty(obj.NewRefreshToken))
				{
					refreshTokenStorageProvider.StoreRefreshToken(obj.NewRefreshToken);
				}
				callback(obj.AccessToken);
			});
		}

		public void Authenticate(string accessToken, Action<string> callback)
		{
			JsonHttpPostRequest(BASE_URL + "Authenticate/GetBLToken", new IdentityRequest(accessToken), (res) =>
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