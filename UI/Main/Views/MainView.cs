using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.API;
using HMUI;
using IPA.Utilities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace BSChallenger.UI.Main.Views
{
	[HotReload(RelativePathToLayout = @"MainView.bsml")]
	[ViewDefinition("BSChallenger.UI.Main.Views.MainView")]
	internal class MainView : BSMLAutomaticViewController
	{
		[Inject]
		private LevelView levelView;

		[UIComponent("levelSelectListObj")]
		public CustomCellListTableData levelSelectListObj;
		[UIValue("levelSelectList")]
		internal List<object> levelSelection = new List<object>();

		[UIComponent("levelProgListObj")]
		public CustomCellListTableData levelProgListObj;
		[UIValue("levelProgList")]
		internal List<object> levelsProgress = new List<object>();

		[UIComponent("currentRankingImg")]
		internal ImageView currentRankingImg;
		[UIComponent("currentRankingTxt")]
		internal TextMeshProUGUI currentRankingtxt;

		internal Ranking currentRanking;

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
			if (currentRanking != null)
			{
				currentRankingImg.SetImage(currentRanking.iconURL);
				currentRankingtxt.text = currentRanking.name;
			}
		}

		internal void SetRanking(Ranking ranking)
		{
			currentRanking = ranking;
			var levels = ranking.levels.OrderBy(x => x.levelNumber);
			levelSelection = levels.Select(x => (object)new LeveSelectlUI(x)).ToList();
			levelSelectListObj.data = levelSelection;
			levelSelectListObj.tableView.ReloadData();
		}

		[UIAction("level-selected")]
		private void LevelSelected(TableView view, LeveSelectlUI cell)
		{
			levelView.SetLevel(cell.level);
		}
	}

	internal class LevelProgressUI
	{
		[UIValue("text")]
		private string Text => "Lvl: " + lvl;
		[UIValue("count")]
		private string CountText => count + "/" + totalCount;

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
		internal Level level;

		[UIObject("cell")]
		private GameObject cell;
		private CustomCellTableCell tableCell;
		private ImageView bg;

		[UIComponent("coverImg")]
		private ImageView cover;

		[UIValue("coverURL")]
		private string coverURL => level.iconURL == "Default" ? "BSChallenger.UI.Resources.Pentagon.png" : level.iconURL;

		[UIValue("text")]
		private string Text => "Lvl " + level.levelNumber;

		[UIAction("#post-parse")]
		internal void PostParse()
		{
			tableCell = cell.GetComponentInParent<CustomCellTableCell>();
			var images = cell.GetComponentsInChildren<Backgroundable>().Select(x => x.GetComponent<ImageView>());
			foreach (var x in images)
			{
				if (!x || x.color0 != Color.white || x.sprite.name != "RoundRect10")
					continue;

				ReflectionUtil.SetField(x, "_skew", 0f);
				x.overrideSprite = null;
				x.SetImage("#RoundRect10BorderFade");
				x.color = new Color(0.25f, 0.25f, 1f, 0.4f);
				bg = x;
			}
			foreach (var x in images)
			{
				if ((x != null) && x.sprite.name == "RoundRect10")
				{
					x.color0 = new Color(0.5f, 0.25f, 1f);
					x.color1 = new Color(0.23f, 0f, 0.58f);
				}
			}
			if (cell.GetComponent<CellBehaviour>() == null)
			{
				var behaviour = cell.AddComponent<CellBehaviour>();
				behaviour.bg = bg;
				behaviour.cell = tableCell;
			}
			var colorHex = "#" + level.color;
			Color color = Color.white;
			ColorUtility.TryParseHtmlString(colorHex, out color);
			cover.color0 = color;
			cover.color1 = (color * 0.6f).ColorWithAlpha(1f);
			cover.gradient = true;
			cover.SetField("_skew", 0f);
			cover.SetAllDirty();
		}

		public LeveSelectlUI(Level lvl)
		{
			level = lvl;
		}
	}
}