using BSChallenger.API;
using BSChallenger.Utils;
using System;

namespace BSChallenger.Stores
{
	internal class UserStore : Store<UserStore>
	{
		private User CurrentUser { get; set; }

		public Result<User> GetCurentUser()
		{
			if (CurrentUser != null)
			{
				return Result.Ok(CurrentUser);
			}
			else
			{
				return Result.Fail<User>("User is not authenticated!");
			}
		}

		public void SetUser(Action<User> OnResult, Action OnError)
		{
			ApiProvider.GetUser(-1, (x) =>
			{
				CurrentUser = x;
				OnResult(x);
			}, (err) =>
			{
				OnError();
				Logger.Error($"Failed to get user with token because of error: {err?.ToString()}");
			});
		}
	}
}
