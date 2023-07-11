using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSChallenger.UI.Main.Views
{
	[HotReload(RelativePathToLayout = @"MainView.bsml")]
	[ViewDefinition("BSChallengeRanking.UI.Main.Views.MainView.bsml")]
	internal class MainView : BSMLAutomaticViewController
	{
		[UIValue("levels")]
		internal List<object> levels = new List<object>()
		{
			new ProgressUI(0, 8),
			new ProgressUI(1, 14),
			new ProgressUI(2, 11),
			new ProgressUI(3, 26),
			new ProgressUI(4, 25),
			new ProgressUI(5, 28),
			new ProgressUI(6, 35),
			new ProgressUI(7, 17),
			new ProgressUI(8, 19),
			new ProgressUI(9, 15),
			new ProgressUI(10, 38),
			new ProgressUI(11, 22),
			new ProgressUI(12, 15),
			new ProgressUI(13, 13),
			new ProgressUI(14, 22),
			new ProgressUI(15, 14)
		};

		[UIValue("levelsList")]
		internal List<object> levelsList = new List<object>()
		{
			new LevelUI(0),
			new LevelUI(1),
			new LevelUI(2),
			new LevelUI(3),
			new LevelUI(4),
			new LevelUI(5),
			new LevelUI(6),
			new LevelUI(7),
			new LevelUI(8),
			new LevelUI(9),
			new LevelUI(10),
			new LevelUI(11),
			new LevelUI(12),
			new LevelUI(13),
			new LevelUI(14),
			new LevelUI(15)
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
				x.color = new Color(1f, 1f, 1f, 0.4f);
			}
		}
	}

	internal class ProgressUI
	{
		[UIValue("text")]
		private string text => "Lvl: " + lvl;
		[UIValue("count")]
		private string countText => count + "/" + totalCount;

		private int lvl;
		private int totalCount;

		public ProgressUI(int lvl, int totalCount)
		{
			this.lvl = lvl;
			this.totalCount = totalCount;
		}

		private int count => new System.Random().Next(0, totalCount);
	}

	internal class LevelUI
	{
		private int lvl;

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

		public LevelUI(int lvl)
		{
			this.lvl = lvl;
		}
	}
}
