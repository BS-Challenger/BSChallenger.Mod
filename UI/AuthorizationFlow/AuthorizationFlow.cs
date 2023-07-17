using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using BSChallenger.Providers;
using BSChallenger.UI.AuthorizationFlow.Views;
using BSChallenger.UI.Main;
using HMUI;
using Zenject;

namespace BSChallenger.UI.AuthorizationFlow
{
	internal class AuthorizationFlow : FlowCoordinator
	{
		private BSChallengerFlowCoordinator _rankingFlow;
		private AuthView _authView;
		internal BSChallengeRankingAPIProvider _apiProvider;

		[Inject]
		internal void Construct(AuthView authViewController, BSChallengerFlowCoordinator rankingFlowCoordinator, BSChallengeRankingAPIProvider apiProvider)
		{
			_authView = authViewController;
			_rankingFlow = rankingFlowCoordinator;
			_apiProvider = apiProvider;
			MenuButtons.instance.RegisterButton(
				new MenuButton("BSChallenger", () =>
				{
					BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(this);
				}
			));
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			if (firstActivation)
			{
				SetTitle("Authorizing...");
				showBackButton = true;
				ProvideInitialViewControllers(_authView);
			}
		}

		internal void GoToRankingFlow()
		{
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal, true);
			BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(_rankingFlow, null, ViewController.AnimationDirection.Horizontal, true);
		}

		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
		}
	}
}