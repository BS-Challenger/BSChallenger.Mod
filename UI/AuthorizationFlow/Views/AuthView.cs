using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.Providers;
using BSChallenger.Stores;
using BSChallenger.Utils;
using HMUI;
using IPA.Utilities;
using IPA.Utilities.Async;
using SiraUtil.Logging;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace BSChallenger.UI.AuthorizationFlow.Views
{
	[HotReload(RelativePathToLayout = @"AuthView.bsml")]
	[ViewDefinition("BSChallenger.UI.AuthorizationFlow.Views.AuthView")]
	internal class AuthView : BSMLAutomaticViewController
	{
		private AuthorizationFlow _authFlow = null;
		private SiraLog _logger;
		private TokenStorageProvider _tokenStorageProvider = null;
		private ChallengeRankingApiProvider _apiProvider;

		[Inject]
		private void Construct(SiraLog logger, AuthorizationFlow authorizationFlow, TokenStorageProvider tokenStorageProvider, ChallengeRankingApiProvider apiProvider)
		{
			_logger = logger;
			_authFlow = authorizationFlow;
			_tokenStorageProvider = tokenStorageProvider;
			_apiProvider = apiProvider;
		}

		[UIAction("#post-parse")]
		internal void PostParse()
		{
			if (gameObject.GetComponent<Touchable>() == null)
				gameObject.AddComponent<Touchable>();
			foreach (var x in GetComponentsInChildren<Backgroundable>().Select(x => x.GetComponent<ImageView>()))
			{
				if (!x || x.color0 != Color.white || x.sprite.name != "RoundRect10")
					continue;

				ReflectionUtil.SetField(x, "_skew", 0f);
				x.overrideSprite = null;
				x.SetImage("#RoundRect10BorderFade");
				x.color = new Color(1f, 1f, 1f, 0.4f);
			}
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

			//Action because uhhhh
			var getNewToken = () =>
			{
				var websocket = new AuthWebsocketProvider();
				websocket.Initialize((x) =>
				{
					_tokenStorageProvider.StoreToken(x);
					_apiProvider.ProvideToken(x);
					var userStore = UserStore.Get();
					if (!userStore.IsFailure)
					{
						userStore.Value.SetUser(async _ => {
							await Task.Delay(200);
							HMMainThreadDispatcher.instance.Enqueue(() =>
							{
								_authFlow.GoToRankingFlow();
							});
						}, () => { });
					}
				});
				Application.OpenURL("http://localhost:8081/mod-auth");
			};

			if (_tokenStorageProvider.RefreshTokenExists())
			{
				var token = _tokenStorageProvider.GetToken();

				if (!string.IsNullOrEmpty(token))
				{
					_apiProvider.ProvideToken(token);
					var userStore = UserStore.Get();
					userStore.Value.SetUser(async _ =>
					{
						await Task.Delay(200);
						HMMainThreadDispatcher.instance.Enqueue(() =>
						{
							_logger.Info("x");
							_authFlow.GoToRankingFlow();
						});
					}, () => getNewToken());
				}
				else
				{
					getNewToken();
				}
			}
			else
			{
				getNewToken();
			}
		}
	}
}