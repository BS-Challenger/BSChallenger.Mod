using BSChallenger.API;
using BSChallenger.API.Scan;
using BSChallenger.API.Web;
using Newtonsoft.Json;
using SiraUtil.Logging;
using SiraUtil.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.Providers
{
	public class ChallengeRankingApiProvider
	{
		private const string BASE_URL = "https://localhost:8081/";
		private readonly SiraLog _logger;
		private readonly IHttpService _httpService;
		public ChallengeRankingApiProvider(SiraLog siraLog, IHttpService httpService)
		{
			_logger = siraLog;
			_httpService = httpService;
		}

		public void Rankings(Action<List<Ranking>> callback)
		{
			JsonHttpGetRequest(BASE_URL + "rankings", (res) =>
			{
				var obj = JsonConvert.DeserializeObject<List<Ranking>>(res);
				callback(obj);
			}, (err) =>
			{
				_logger.Error("Failed to fetch rankings: " + err.Message);
			});
		}

		public void GetUser(long user, Action<User> callback, Action<ErrorResponseModel> errorCallback)
		{
			string url = user != -1 ? BASE_URL + "profile/" + user : BASE_URL + "profile/mod";
			JsonHttpGetRequest(url, (res) =>
			{
				Console.WriteLine(res);
				var userObj = JsonConvert.DeserializeObject<User>(res);
				callback(userObj);
			}, errorCallback);
		}

		public void Scan(string ranking, Action<string> callback)
		{
			HttpPostRequest<ErrorResponseModel>(BASE_URL + "scan", new ScanRequest(ranking), () =>
			{
				callback("");
			}, (err) =>
			{
				_logger.Error("Scan request failed: " + err.Message);
			});
		}

		public void ProvideToken(string token)
		{
			_httpService.Token = token;
		}

		private void JsonHttpGetRequest(string url, Action<string> callback, Action<ErrorResponseModel> errorCallback)
		{
			Task.Run(async () =>
			{
				var res = await _httpService.GetAsync(url);
				if (!res.Successful)
				{
					Console.WriteLine(res.Error().Result);
					errorCallback.Invoke(new ErrorResponseModel(res));
				}
				var stringRes = await res.ReadAsStringAsync();
				callback(stringRes);
			});
		}

		private void HttpPostRequest<T, T1>(string url, object obj, Action<T> callback, Action<T1> errorCallback)
		{
			Task.Run(async () =>
			{
				var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
				var res = await _httpService.PostAsync(url, content);
				var stringRes = await res.ReadAsStringAsync();
				if (!res.Successful)
				{
					errorCallback(JsonConvert.DeserializeObject<T1>(stringRes));
				}
				else
				{
					callback(JsonConvert.DeserializeObject<T>(stringRes));
				}
			});
		}

		private void HttpPostRequest<T1>(string url, object obj, Action callback, Action<T1> errorCallback)
		{
			Task.Run(async () =>
			{
				var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
				var res = await _httpService.PostAsync(url, content);
				var stringRes = await res.ReadAsStringAsync();
				if (!res.Successful)
				{
					errorCallback(JsonConvert.DeserializeObject<T1>(stringRes));
				}
				else
				{
					callback();
				}
			});
		}
	}
}