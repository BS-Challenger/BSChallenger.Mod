using BeatSaberMarkupLanguage;
using BSChallenger.API;
using BSChallenger.Providers;
using BSChallenger.UI.Main.Views;
using HMUI;
using Zenject;

namespace BSChallenger.UI.Main
{
	internal class BSChallengerFlowCoordinator : FlowCoordinator
	{
		private MainView _mainView;
		private LevelView _levelView;
		private BSChallengeRankingAPIProvider _apiProvider;
		internal Ranking currentRanking;

		[Inject]
		internal void Construct(MainView mainViewController, LevelView levelViewController, BSChallengeRankingAPIProvider api)
		{
			_mainView = mainViewController;
			_levelView = levelViewController;
			_apiProvider = api;
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			_apiProvider.FetchRankings((results) =>
			{
				currentRanking = results[0];
				DistributeRanking(currentRanking);
			});
			if (firstActivation)
			{
				SetTitle("Beat Saber Challenger");
				showBackButton = true;
				ProvideInitialViewControllers(_mainView, _levelView);
			}
		}

		private void DistributeRanking(Ranking ranking)
		{
			_levelView.SetRanking(ranking);
			_mainView.SetRanking(ranking);
		}

		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
		}
	}
}