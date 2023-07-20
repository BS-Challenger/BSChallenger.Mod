using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.API;
using BSChallenger.Providers;
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
		private LevelView _levelView = null;
		[Inject]
		private BSChallengerFlowCoordinator _flow = null;
		[Inject]
		private RefreshTokenStorageProvider _refreshTokenStorageProvider = null;

		[UIComponent("levelSelectListObj")]
		public CustomCellListTableData levelSelectListObj = null;
		[UIValue("levelSelectList")]
		internal List<object> levelSelection = new List<object>();

		[UIComponent("levelProgListObj")]
		public CustomCellListTableData levelProgListObj = null;
		[UIValue("levelProgList")]
		internal List<object> levelsProgress = new List<object>();

		[UIComponent("currentRankingImg")]
		internal ImageView currentRankingImg = null;
		[UIComponent("currentRankingTxt")]
		internal TextMeshProUGUI currentRankingtxt = null;

		internal Ranking currentRanking;
		internal int rankingIdx = 0;

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
			currentRanking = ranking;
			var levels = ranking.levels.OrderBy(x => x.levelNumber);
			levelSelection = levels.Select(x => (object)new LeveSelectlUI(x)).ToList();
			levelSelectListObj.data = levelSelection;
			levelSelectListObj.tableView.ReloadData();
			levelSelectListObj.tableView.SelectCellWithIdx(0);
			if (currentRanking != null)
			{
				currentRankingImg.SetImage(currentRanking.iconURL);
				currentRankingtxt.text = currentRanking.name;
			}
		}

		[UIAction("level-selected")]
		private void LevelSelected(TableView view, LeveSelectlUI cell)
		{
			_levelView.SetLevel(cell.level);
		}

		[UIAction("LeftRanking")]
		private void LeftRanking()
		{
			rankingIdx = Mathf.Max(rankingIdx - 1, 0);
			_flow.DistributeRanking(_flow.allRankings[rankingIdx]);
		}

		[UIAction("RightRanking")]
		private void RightRanking()
		{
			rankingIdx = Mathf.Min(rankingIdx + 1, _flow.allRankings.Count - 1);
			_flow.DistributeRanking(_flow.allRankings[rankingIdx]);
		}

		[UIAction("OpenBeatleader")]
		private void BeatLeader()
		{
			string token = _refreshTokenStorageProvider.GetRefreshToken();
			_flow._apiProvider.AccessToken(token, (x) =>
			{
				_flow._apiProvider.Authenticate(x, (x) => {
					Plugin.Log.Info(x);
				});
			});
		}
	}

	internal class LevelProgressUI
	{
		[UIValue("text")]
		private string Text => "Lvl: " + lvl;
		[UIValue("count")]
		private string CountText => Count + "/" + totalCount;

		private readonly int lvl;
		private readonly int totalCount;

		public LevelProgressUI(int lvl, int totalCount)
		{
			this.lvl = lvl;
			this.totalCount = totalCount;
		}

		private int Count => new System.Random().Next(0, totalCount);
	}

	internal class LeveSelectlUI
	{
		internal Level level;

		[UIObject("cell")]
		private GameObject cell = null;
		private ImageView bg;

		[UIComponent("coverImg")]
		private ImageView cover = null;

		[UIValue("coverURL")]
		private string coverURL => level.iconURL == "Default" ? "BSChallenger.UI.Resources.Pentagon.png" : level.iconURL;

		[UIValue("text")]
		private string Text => "Lvl " + level.levelNumber;

		public LeveSelectlUI(Level lvl)
		{
			level = lvl;
		}

		[UIAction("#post-parse")]
		internal void PostParse()
		{
			CustomCellTableCell tableCell = cell.GetComponentInParent<CustomCellTableCell>();
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
			if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
			{
				cover.color0 = color;
				cover.color1 = (color * 0.6f).ColorWithAlpha(1f);
				cover.gradient = true;
			}
		}
	}
}