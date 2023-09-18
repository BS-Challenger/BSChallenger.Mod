using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.Providers;
using BSChallenger.Utils;
using HMUI;
using IPA.Utilities;
using IPA.Utilities.Async;
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
		[Inject] private AuthorizationFlow _authFlow = null;

		[Inject] private TokenStorageProvider _tokenStorageProvider = null;

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

			Task.Run(async () =>
			{
				using (var websocket = new AuthWebsocketProvider())
				{
					websocket.Initialize();
					while (!websocket.started)
					{
						await Task.Delay(50);
					}
					//Application.OpenURL("http://localhost:8081/mod-auth");
					//string token = Console.ReadLine();
					//Console.WriteLine(token);
					//_tokenStorageProvider.StoreToken(token);
					HMMainThreadDispatcher.instance.Enqueue(() =>
					{
						_authFlow.GoToRankingFlow();
					});
				}
			});
		}
	}
}