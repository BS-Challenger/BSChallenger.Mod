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
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BSChallengeRanking.UI.AuthorizationFlow.Views
{
	[HotReload(RelativePathToLayout = @"IdleView.bsml")]
	[ViewDefinition("BSChallengeRanking.UI.AuthorizationFlow.Views.IdleView.bsml")]
	internal class IdleView : BSMLAutomaticViewController
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

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			StartCoroutine(TestCoroutine());
		}

		IEnumerator TestCoroutine()
		{
			yield return new WaitForSeconds(5f);
			_authFlow.StatusCheckComplete(AuthorizationFlow.StatusResult.Success);
		}
	}
}
