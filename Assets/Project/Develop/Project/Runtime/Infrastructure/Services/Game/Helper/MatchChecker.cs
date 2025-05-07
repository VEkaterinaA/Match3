using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.MonoBehaviours.Game;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class MatchChecker
	{
		private ILevelInfoService _levelInfoService;

		[Inject]
		private void Construct(ILevelInfoService levelInfoService)
		{
			_levelInfoService = levelInfoService;
		}

		internal bool HasAnyPossibleMove(Stone[,] stones)
		{
			for (var x = 0; x < _levelInfoService.LevelInfo.WidthOfBoard; x++)
			{
				for (var y = 0; y < _levelInfoService.LevelInfo.HeightOfBoard; y++)
				{
					if (CanSwapFormMatch(stones, x, y, x + 1, y) || CanSwapFormMatch(stones, x, y, x, y + 1))
					{
						return true;
					}
				}
			}

			return false;
		}

		internal bool IsInsideBoard(int x, int y)
		{
			return x >= 0 && x < _levelInfoService.LevelInfo.WidthOfBoard && y >= 0 && y < _levelInfoService.LevelInfo.HeightOfBoard;
		}

		private bool CanSwapFormMatch(Stone[,] stones, int x1, int y1, int x2, int y2)
		{
			if (!IsInsideBoard(x2, y2))
			{
				return false;
			}

			var gem1 = stones[x1, y1];
			var gem2 = stones[x2, y2];

			if (gem1 == null || gem2 == null)
			{
				return false;
			}

			(stones[x1, y1], stones[x2, y2]) = (stones[x2, y2], stones[x1, y1]);

			var match = HasMatchAt(stones, x1, y1) || HasMatchAt(stones, x2, y2);

			(stones[x1, y1], stones[x2, y2]) = (stones[x2, y2], stones[x1, y1]);

			return match;
		}

		private bool HasMatchAt(Stone[,] stones, int x, int y)
		{
			var type = stones[x, y].StoneType;

			var horizontalMatch = 1;
			for (var i = x - 1; i >= 0 && stones[i, y]?.StoneType == type; i--)
			{
				horizontalMatch++;
			}

			for (var i = x + 1; i < _levelInfoService.LevelInfo.WidthOfBoard && stones[i, y]?.StoneType == type; i++)
			{
				horizontalMatch++;
			}

			if (horizontalMatch >= 3)
			{
				return true;
			}

			var verticalMatch = 1;
			for (var i = y - 1; i >= 0 && stones[x, i]?.StoneType == type; i--)
			{
				verticalMatch++;
			}

			for (var i = y + 1; i < _levelInfoService.LevelInfo.HeightOfBoard && stones[x, i]?.StoneType == type; i++)
			{
				verticalMatch++;
			}

			return verticalMatch >= 3;
		}

		internal bool IsValidGemPlacement(Stone[,] stones, int x, int y, StoneType type)
		{
			return !(HasMatchingHorizontalPair(stones, x, y, type) ||
					 HasMatchingVerticalPair(stones, x, y, type) ||
					 IsSurroundedBySameType(stones, x, y, type));
		}
		internal bool IsValidGemPlacement(Stone[,] stones, Stone gem)
		{
			return !(HasMatchingHorizontalPair(stones, gem.X, gem.Y, gem.StoneType) ||
					 HasMatchingVerticalPair(stones, gem.X, gem.Y, gem.StoneType) ||
					 IsSurroundedBySameType(stones, gem.X, gem.Y, gem.StoneType));
		}


		private bool HasMatchingHorizontalPair(Stone[,] stones, int x, int y, StoneType type)
		{
			return (x >= 2 && (stones[x - 1, y]?.StoneType == type && stones[x - 2, y]?.StoneType == type) ||
				   (x <= _levelInfoService.LevelInfo.WidthOfBoard - 3 && stones[x + 1, y]?.StoneType == type && stones[x + 2, y]?.StoneType == type));
		}

		private bool HasMatchingVerticalPair(Stone[,] stones, int x, int y, StoneType type)
		{
			return ((y >= 2) && (stones[x, y - 1]?.StoneType == type) && (stones[x, y - 2]?.StoneType == type) ||
				   (y <= _levelInfoService.LevelInfo.HeightOfBoard - 3) && (stones[x, y + 1]?.StoneType == type) && (stones[x, y + 2]?.StoneType == type));
		}

		private bool IsSurroundedBySameType(Stone[,] stones, int x, int y, StoneType type)
		{
			return x > 0 && y > 0 && x < _levelInfoService.LevelInfo.WidthOfBoard - 1 && y < _levelInfoService.LevelInfo.HeightOfBoard - 1 &&
				  (stones[x - 1, y]?.StoneType == type && stones[x + 1, y]?.StoneType == type ||
				   stones[x, y - 1]?.StoneType == type && stones[x, y + 1]?.StoneType == type);
		}
	}
}
