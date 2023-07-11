﻿using BSChallenger.UI.AuthorizationFlow;
using BSChallenger.UI.AuthorizationFlow.Views;
using BSChallenger.UI.Main;
using BSChallenger.UI.Main.Views;
using Zenject;

namespace BSChallenger.Installers
{
	internal class BSChallengerMenuInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.Bind<MainView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<LevelView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<BSChallengerFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
			Container.Bind<AuthView>().FromNewComponentAsViewController().AsSingle().NonLazy();
			Container.Bind<AuthorizationFlow>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}
}