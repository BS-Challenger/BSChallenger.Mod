using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallengeRanking.UI.Main;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Zenject;

namespace BSChallengeRanking.UI.AuthorizationFlow.Views
{
	[HotReload(RelativePathToLayout = @"SignupView.bsml")]
	[ViewDefinition("BSChallengeRanking.UI.AuthorizationFlow.Views.SignupView.bsml")]
	internal class SignupView : BSMLAutomaticViewController
	{
		[Inject]
		private AuthorizationFlow _authFlow;

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

		[UIAction("signup")]
		private void SignUp()
		{
			_authFlow.StatusCheckComplete(AuthorizationFlow.StatusResult.Success);
		}
	}
}
