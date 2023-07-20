using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.API;
using BSChallenger.Utils;
using HMUI;
using IPA.Utilities;
using SongDetailsCache.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BSChallenger.UI.Main.Views
{
	[HotReload(RelativePathToLayout = @"LevelView.bsml")]
	[ViewDefinition("BSChallenger.UI.Main.Views.LevelView")]
	internal class LevelView : BSMLAutomaticViewController
	{
		[UIComponent("mapsList")]
		private CustomCellListTableData list = null;
		[UIValue("maps")]
		private List<object> maps = new List<object>();
		[UIComponent("topText")]
		private TextMeshProUGUI topText = null;

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
				if (x.color == Color.black)
				{
					x.color = Color.gray;
				}
				else
				{
					x.color = new Color(1f, 1f, 1f, 0.4f);
				}
			}
		}

		internal void SetRanking(Ranking ranking)
		{
			StartCoroutine(SetRankingCoroutine(ranking));
		}

		internal void SetLevel(Level level)
		{
			topText.text = "Level: " + level.levelNumber;
			maps = level.availableForPass.Select(x => (object)new MapUI(x)).ToList();
			list.data = maps;
			list.tableView.ReloadData();
			list.tableView.SelectCellWithIdx(0);
		}

		private IEnumerator SetRankingCoroutine(Ranking ranking)
		{
			yield return new WaitUntil(() => list != null);
			SetLevel(ranking.levels[0]);
		}
	}

	internal class MapUI
	{
		private readonly Map map = null;

		internal MapUI(Map map)
		{
			this.map = map;
		}

		[UIObject("cell")]
		private GameObject cell = null;

		private ImageView bg;

		[UIComponent("Cover")]
		private ImageView cover = null;

		[UIComponent("MapName")]
		private TextMeshProUGUI MapName = null;

		[UIComponent("MapperName")]
		private TextMeshProUGUI MapperName = null;

		[UIAction("#post-parse")]
		internal void PostParse()
		{
			SongDetailsUtils.FetchBeatmap(map.hash, (x) =>
			{
				cover.SetImage(x.coverURL);
				MapName.text = x.songName;
				MapperName.text = "[<size=80%><color=#ff69b4>" + x.uploaderName + "</color></size>]";
			});
			CustomCellTableCell tableCell = cell.GetComponentInParent<CustomCellTableCell>();
			foreach (var x in cell.GetComponentsInChildren<Backgroundable>().Select(x => x.GetComponent<ImageView>()))
			{
				if (!x || x.color0 != Color.white || x.sprite.name != "RoundRect10")
					continue;

				x.SetField("_skew", 0f);
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
	}
}