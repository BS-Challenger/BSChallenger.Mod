using BeatSaberMarkupLanguage;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace BSChallenger.Utils
{
	internal static class BSMLUtils
	{
		internal static void FixStringSetting(Transform obj)
		{
			obj.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
			obj.GetChild(1).GetChild(1).GetChild(0).GetComponent<ImageView>().SetImage("#RoundRect10");
			var editIcon = obj.GetChild(1).GetChild(1).GetChild(1).GetComponent<ImageView>();
			editIcon.SetField("_skew", 0f);
			editIcon.gameObject.SetActive(false);
			editIcon.gameObject.SetActive(true);
		}
	}
}