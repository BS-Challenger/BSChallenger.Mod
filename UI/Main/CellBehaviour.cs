using BeatSaberMarkupLanguage.Components;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSChallenger.UI.Main
{
	internal class CellBehaviour : MonoBehaviour
	{
		internal CustomCellTableCell cell;
		internal ImageView bg;
		private void Update()
		{
			Color targetColor;
			if (cell.selected)
				targetColor = Color.green;
			else if (cell.highlighted)
				targetColor = new Color(1f, 0.45f, 1f, 0.4f);
			else
				targetColor = new Color(0.25f, 0.25f, 1f, 0.4f);
			bg.color = Color.Lerp(bg.color, targetColor, Time.deltaTime * 5f);
		}
	}
}
