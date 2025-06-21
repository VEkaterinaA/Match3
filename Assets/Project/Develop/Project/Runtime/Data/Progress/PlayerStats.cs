using Runtime.Infrastructure.Core;
using System;

namespace Runtime.Data.Progress
{
	[Serializable]
	public class PlayerStats : IPrototype<PlayerStats>
	{
		public Int32 Games_won;
		public Int32 Games_played;
		public Int32 Highest_score;


		PlayerStats IPrototype<PlayerStats>.Clone()
		{
			var playerStats = new PlayerStats();

			playerStats.Games_won = Games_won;
			playerStats.Games_played = Games_played;
			playerStats.Highest_score = Highest_score;

			return playerStats;
		}

	}
}
