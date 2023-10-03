using BSChallenger.Providers;
using BSChallenger.Utils;
using SiraUtil.Logging;
using System;
using Zenject;

namespace BSChallenger.Stores
{
	internal class Store<T> where T : Store<T>
	{
		private static T Instance { get; set; }
		protected ChallengeRankingApiProvider ApiProvider { get; private set; }
		protected SiraLog Logger { get; private set; }

		[Inject]
		private void Construct(ChallengeRankingApiProvider apiProvider, SiraLog logger)
		{
			Instance = (T)this;
			ApiProvider = apiProvider;
			Logger = logger;
		}

		public static Result<T> Get()
		{
			if (Instance == null)
			{
				Console.Error.WriteLine($"Failed to get {typeof(T).Name} as it has not been constructed yet!");
				return Result.Fail<T>("Uninitialized");
			}
			return Result.Ok(Instance);
		}
	}
}
