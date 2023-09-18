using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSChallenger.API;
using BSChallenger.Providers;
using BSChallenger.Utils;
using HMUI;
using IPA.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace BSChallenger.UI.Main.Views
{
	[HotReload(RelativePathToLayout = @"LevelView.bsml")]
	[ViewDefinition("BSChallenger.UI.Main.Views.LevelView")]
	internal class LevelView : BSMLAutomaticViewController
	{
		//Dependencies
		ChallengeRankingApiProvider _apiProvider = null;

		//List
		[UIComponent("mapsList")]
		private CustomCellListTableData list = null;
		[UIValue("maps")]
		private List<object> maps = new List<object>();

		//TopSection
		[UIComponent("topText")]
		private TextMeshProUGUI topText = null;

		//Right Section
		[UIComponent("coverBig")]
		private ImageView coverBig = null;
		[UIComponent("mapNameBig")]
		private TextMeshProUGUI mapNameBig = null;
		[UIComponent("mapperNameBig")]
		private TextMeshProUGUI mapperNameBig = null;

		private Map currentMap;

		[Inject]
		private void Construct(ChallengeRankingApiProvider challengeRankingApiProvider)
		{
			_apiProvider = challengeRankingApiProvider;
		}

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

		internal void SetLevel(Level level, string rankingName)
		{
			topText.text = "Level: " + level.levelNumber;

			HMMainThreadDispatcher.instance.Enqueue(() =>
			{
				maps = level.availableForPass.ConvertAll(x => (object)new MapUI(x));
				list.data = maps;
				list.tableView.ReloadData();
				list.tableView.SelectCellWithIdx(0);
				SetMap(((MapUI)maps[0]).map);
			});
		}

		private void SetMap(Map map)
		{
			currentMap = map;
			SongDetailsUtils.FetchBeatmap(map.hash, (x) =>
			{
				coverBig.SetImage(x.coverURL);
				mapNameBig.text = x.songName;
				mapperNameBig.text = "[<size=80%><color=#ff69b4>" + x.uploaderName + "</color></size>]";
			});
		}

		[UIAction("go-to-mapper")]
		private void GoToMapper()
		{
			if (currentMap != null)
			{
				SongDetailsUtils.FetchBeatmap(currentMap.hash, (x) =>
				{
					BeatSaverUtils.GetMapperID(x.uploaderName, (id) =>
					{
						Application.OpenURL("https://beatsaver.com/profile/" + id);
					});
				});
			}
		}

		[UIAction("go-to-beatleader")]
		private void GoToBeatleader()
		{
			if (currentMap != null)
			{
				BeatleaderUtils.GetLeaderboardID(currentMap.hash, (id) =>
				{
					Application.OpenURL("https://www.beatleader.xyz/leaderboard/global/" + id);
				});
			}
		}

		[UIAction("go-to-beatsaver")]
		private void GoToBeatsaver()
		{
			if (currentMap != null)
			{
				SongDetailsUtils.FetchBeatmap(currentMap.hash, (x) =>
				{
					Application.OpenURL("https://beatsaver.com/maps/" + x.key);
				});
			}
		}

		[UIAction("map-selected")]
		private void SelectMap(TableView view, MapUI cell)
		{
			SetMap(cell.map);
		}

		private IEnumerator SetRankingCoroutine(Ranking ranking)
		{
			yield return new WaitUntil(() => list != null);
			if (ranking.Levels.Count > 0)
			{
				SetLevel(ranking.Levels[0], ranking.Name);
			}
		}
	}

	internal class MapUI
	{
		internal readonly Map map = null;

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