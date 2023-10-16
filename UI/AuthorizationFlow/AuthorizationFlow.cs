using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using BSChallenger.Providers;
using BSChallenger.UI.AuthorizationFlow.Views;
using BSChallenger.UI.Main;
using HMUI;
using System.Threading.Tasks;
using Zenject;

namespace BSChallenger.UI.AuthorizationFlow
{
	internal class AuthorizationFlow : FlowCoordinator
	{
		private BSChallengerFlowCoordinator _rankingFlow;
		private AuthView _authView;

		[Inject]
		internal void Construct(AuthView authViewController, BSChallengerFlowCoordinator rankingFlowCoordinator)
		{
			_authView = authViewController;
			_rankingFlow = rankingFlowCoordinator;
			MenuButtons.instance.RegisterButton(
				new MenuButton("BS Challenger", () => BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(this)
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
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
		}

		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
		}
	}
}