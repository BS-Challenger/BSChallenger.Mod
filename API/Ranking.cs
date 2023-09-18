using System.Collections.Generic;

namespace BSChallenger.API
{
	public class Ranking
	{
		public string Identifier { get; set; }
		public int GuildId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string IconURL { get; set; }
		public bool Private { get; set; }
		public bool Partnered { get; set; }
		public List<Level> Levels { get; set; }
	}
}