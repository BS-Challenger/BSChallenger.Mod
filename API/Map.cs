using System.Collections.Generic;

namespace BSChallenger.API
{
	public class Map
	{
		public string hash;
		public string characteristic;
		public string difficulty;
		public List<string> features = new List<string>();
	}
}