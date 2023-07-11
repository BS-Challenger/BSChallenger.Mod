using BSChallengeRanking.UI.AuthorizationFlow;
using BSChallengeRanking.UI.AuthorizationFlow.Views;
using BSChallengeRanking.UI.Main;
using BSChallengeRanking.UI.Main.Views;
using Zenject;

namespace BSChallengeRanking.Installers
{
	internal class BSChallengeRankingMenuInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.Bind<MainView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<LevelView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<BSChallengeRankingFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
			Container.Bind<IdleView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<LoginView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<SignupView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<AuthorizationFlow>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}
}