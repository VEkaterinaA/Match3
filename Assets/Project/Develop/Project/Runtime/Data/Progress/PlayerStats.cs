using System;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal class PlayerStats
	{
		private Int32 _games_won;
		private Int32 _games_played;
		private Int32 _highest_score;

		internal Int32 Games_won
		{
			get => _games_won;
			set => _games_won = value;
		}

		internal Int32 Games_played 
		{ 
			get => _games_played; 
			set => _games_played = value; 
		}

		internal Int32 Highest_score 
		{ 
			get => _highest_score; 
			set => _highest_score = value; 
		}
	}
}
