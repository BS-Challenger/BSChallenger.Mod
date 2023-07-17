using IPA.Config.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallenger.API
{
	public class Level
	{
		public int levelNumber;
		public int mapsReqForPass;
		public string iconURL;
		public List<Map> availableForPass;
		public string color;
	}
}
