using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace BSChallenger.UI.AuthorizationFlow.Views
{
	[HotReload(RelativePathToLayout = @"AuthView.bsml")]
	[ViewDefinition("BSChallengeRanking.UI.AuthorizationFlow.Views.AuthView.bsml")]
	internal class AuthView : BSMLAutomaticViewController
	{
		internal enum LoadingType
		{
			SigningUp,
			LoggingIn,
			CheckingForAccount
		}

		[Inject]
		private AuthorizationFlow _authFlow;

		internal LoadingType loadingToUse = LoadingType.CheckingForAccount;

		[UIObject("signupScreen")]
		private GameObject _signup;

		[UIObject("idleScreen")]
		private GameObject _idle;

		[UIComponent("text")]
		private TextMeshProUGUI text;

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
			_idle.SetActive(true);
			_signup.SetActive(false);
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

			GoTo(LoadingType.CheckingForAccount);
		}

		IEnumerator TestCoroutine()
		{
			yield return new WaitForSeconds(5f);
			switch (loadingToUse)
			{
				case LoadingType.CheckingForAccount:
					_signup.SetActive(true);
					_idle.SetActive(false);
					break;
				case LoadingType.LoggingIn:
					_authFlow.GoToRankingFlow();
					break;
				case LoadingType.SigningUp:
					_authFlow.GoToRankingFlow();
					break;
			}
		}

		[UIAction("signup")]
		private void SignUp()
		{
			_signup.SetActive(false);
			_idle.SetActive(true);
			GoTo(LoadingType.SigningUp);
		}

		private void GoTo(LoadingType loadingToUse)
		{
			this.loadingToUse = loadingToUse;
			StartCoroutine(TestCoroutine());
			text.text = loadingToUse switch
			{
				LoadingType.CheckingForAccount => "Checking For Account",
				LoadingType.LoggingIn => "Logging Into Account",
				LoadingType.SigningUp => "Signing Up For Account",
				_ => ""
			};
		}
	}
}
