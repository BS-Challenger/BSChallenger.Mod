using BeatSaberMarkupLanguage;
using BSChallenger.UI.Main.Views;
using HMUI;
using Zenject;

namespace BSChallenger.UI.Main
{
	internal class BSChallengerFlowCoordinator : FlowCoordinator
	{
		private MainView _mainView;
		private LevelView _levelView;

		[Inject]
		internal void Construct(MainView mainViewController, LevelView levelViewController)
		{
			_mainView = mainViewController;
			_levelView = levelViewController;
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			if (firstActivation)
			{
				SetTitle("Beat Saber Challenger");
				showBackButton = true;
				ProvideInitialViewControllers(_mainView, _levelView);
			}
		}

		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
		}
	}
}