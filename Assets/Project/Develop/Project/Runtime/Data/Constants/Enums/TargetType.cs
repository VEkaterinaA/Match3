using System.Collections.Generic;

namespace Runtime.Data.Constants.Enums
{
	internal enum TargetType
	{
		Unknown = -1,
		ScorePoints = 0,
		RedStone = 1,
		BlueStone = 2,
		GreenStone = 3,
		YellowStone = 4,
		ActivateBooster = 5,

	}

	internal static class TargetTypeDisplayNames
	{
		internal static Dictionary<TargetType, string> TargetNames = new()
		{
			{ TargetType.ScorePoints, "Score points" },
			{ TargetType.RedStone, "Red stones" },
			{ TargetType.BlueStone, "Blue stones" },
			{ TargetType.GreenStone, "Green stones" },
			{ TargetType.YellowStone, "Yellow stones" },
			{ TargetType.ActivateBooster, "Activate boosters" },       
		};
	}

}
