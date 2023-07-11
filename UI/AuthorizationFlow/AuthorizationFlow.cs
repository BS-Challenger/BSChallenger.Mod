using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using BSChallengeRanking.UI.AuthorizationFlow.Views;
using BSChallengeRanking.UI.Main;
using HMUI;
using Zenject;

namespace BSChallengeRanking.UI.AuthorizationFlow
{
	internal class AuthorizationFlow : FlowCoordinator
	{
		private BSChallengeRankingFlowCoordinator _rankingFlow;
		private IdleView _idleView;
		private LoginView _loginView;
		private SignupView _signupView;
		private ViewController _activeView;

		[Inject]
		internal void Construct(IdleView idleViewController, LoginView loginViewController, SignupView signUpViewController, BSChallengeRankingFlowCoordinator rankingFlowCoordinator)
		{
			_idleView = idleViewController;
			_loginView = loginViewController;
			_signupView = signUpViewController;
			_rankingFlow = rankingFlowCoordinator;
			MenuButtons.instance.RegisterButton(
				new MenuButton("BSChallengeRanking", () =>
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
				_activeView = _idleView;
				ProvideInitialViewControllers(_idleView);
			}
		}

		internal void StatusCheckComplete(StatusResult result)
		{
			switch(result)
			{
				case StatusResult.Success:
					BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal, true);
					BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(_rankingFlow, null, ViewController.AnimationDirection.Horizontal, true);
					break;
				case StatusResult.NeedsLogin:
					PresentViewController(_loginView);
					break;
				case StatusResult.NoAccount:
					PresentViewController(_signupView);
					break;
			}
		}

		protected override void BackButtonWasPressed(ViewController topViewController)
		{
			BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
		}

		internal enum StatusResult
		{
			NoAccount,
			NeedsLogin,
			Success
		}
	}
}