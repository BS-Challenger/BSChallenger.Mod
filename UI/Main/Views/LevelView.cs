using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.API;
using HMUI;
using IPA.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSChallenger.UI.Main.Views
{
	[HotReload(RelativePathToLayout = @"LevelView.bsml")]
	[ViewDefinition("BSChallengeRanking.UI.Main.Views.LevelView.bsml")]
	internal class LevelView : BSMLAutomaticViewController
	{
		[UIValue("maps")]
		private List<object> maps = new List<object>()
		{
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI(),
			new MapUI()
		};

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
				if(x.color == Color.black)
				{
					x.color = Color.gray;
				}
				else
				{
					x.color = new Color(1f, 1f, 1f, 0.4f);
				}
			}
		}
	}

	internal class MapUI
	{
		private Map map = null;

		[UIObject("cell")]
		private GameObject cell;

		private CustomCellTableCell tableCell;

		private ImageView bg;

		[UIAction("#post-parse")]
		internal void PostParse()
		{
			tableCell = cell.GetComponentInParent<CustomCellTableCell>();
			foreach (var x in cell.GetComponentsInChildren<Backgroundable>().Select(x => x.GetComponent<ImageView>()))
			{
				if (!x || x.color0 != Color.white || x.sprite.name != "RoundRect10")
					continue;

				ReflectionUtil.SetField(x, "_skew", 0f);
				x.overrideSprite = null;
				x.SetImage("#RoundRect10BorderFade");
				x.color = new Color(0.25f, 0.25f, 1f, 0.4f);
				bg = x;
			}
			if(cell.GetComponent<CellBehaviour>() == null)
			{
				var behaviour = cell.AddComponent<CellBehaviour>();
				behaviour.bg = bg;
				behaviour.cell = tableCell;
			}
		}
	}
}
