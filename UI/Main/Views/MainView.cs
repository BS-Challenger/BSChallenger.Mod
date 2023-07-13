using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.API;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSChallenger.UI.Main.Views
{
	[HotReload(RelativePathToLayout = @"MainView.bsml")]
	[ViewDefinition("BSChallenger.UI.Main.Views.MainView")]
	internal class MainView : BSMLAutomaticViewController
	{
		[UIComponent("levelSelectList")]
		internal List<object> levels = new List<object>();

		[UIComponent("levelProgList")]
		internal List<object> levelsList = new List<object>();

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

		internal void SetRanking(Ranking ranking)
		{
			var levels = ranking.levels.OrderBy(x => x.levelNumber);

		}
	}

	internal class LevelProgressUI
	{
		[UIValue("text")]
		private string text => "Lvl: " + lvl;
		[UIValue("count")]
		private string countText => count + "/" + totalCount;

		private int lvl;
		private int totalCount;

		public LevelProgressUI(int lvl, int totalCount)
		{
			this.lvl = lvl;
			this.totalCount = totalCount;
		}

		private int count => new System.Random().Next(0, totalCount);
	}

	internal class LeveSelectlUI
	{
		private Level level;

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
			if (cell.GetComponent<CellBehaviour>() == null)
			{
				var behaviour = cell.AddComponent<CellBehaviour>();
				behaviour.bg = bg;
				behaviour.cell = tableCell;
			}
		}

		public LeveSelectlUI(Level lvl)
		{
			level = lvl;
		}
	}
}
