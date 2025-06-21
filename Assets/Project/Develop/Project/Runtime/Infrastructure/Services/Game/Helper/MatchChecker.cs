using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.MonoBehaviours.Game.Core;
using UnityEngine;
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

		internal bool HasAnyPossibleMove(IBoardItem[,] stones)
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
		internal bool IsInsideBoard(Vector2Int pos)
		{
			return pos.x >= 0 && pos.x < _levelInfoService.LevelInfo.WidthOfBoard && pos.y >= 0 && pos.y < _levelInfoService.LevelInfo.HeightOfBoard;
		}

		private bool CanSwapFormMatch(IBoardItem[,] board, int x1, int y1, int x2, int y2)
		{
			if (!IsInsideBoard(x2, y2))
			{
				return false;
			}

			var cellOne = board[x1, y1];
			var cellTwo = board[x2, y2];

			if (cellOne == null || cellTwo == null)
			{
				return false;
			}

			(board[x1, y1], board[x2, y2]) = (board[x2, y2], board[x1, y1]);

			var match = HasMatchAt(board, x1, y1) || HasMatchAt(board, x2, y2);

			(board[x1, y1], board[x2, y2]) = (board[x2, y2], board[x1, y1]);

			return match;
		}

		private bool HasMatchAt(IBoardItem[,] stones, int x, int y)
		{
			var type = stones[x, y].CellType;

			var horizontalMatch = 1;
			for (var i = x - 1; i >= 0 && stones[i, y]?.CellType == type; i--)
			{
				horizontalMatch++;
			}

			for (var i = x + 1; i < _levelInfoService.LevelInfo.WidthOfBoard && stones[i, y]?.CellType == type; i++)
			{
				horizontalMatch++;
			}

			if (horizontalMatch >= 3)
			{
				return true;
			}

			var verticalMatch = 1;
			for (var i = y - 1; i >= 0 && stones[x, i]?.CellType == type; i--)
			{
				verticalMatch++;
			}

			for (var i = y + 1; i < _levelInfoService.LevelInfo.HeightOfBoard && stones[x, i]?.CellType == type; i++)
			{
				verticalMatch++;
			}

			return verticalMatch >= 3;
		}

		internal bool IsMatchFreePlacement(IBoardItem[,] stones, int x, int y, CellType type)
		{
			return !(HasMatchingHorizontalPair(stones, x, y, type) ||
					 HasMatchingVerticalPair(stones, x, y, type) ||
					 IsSurroundedBySameType(stones, x, y, type));
		}
		internal bool IsMatchFreePlacement(IBoardItem[,] stones, IBoardItem gem)
		{
			return !(HasMatchingHorizontalPair(stones, gem.X, gem.Y, gem.CellType) ||
					 HasMatchingVerticalPair(stones, gem.X, gem.Y, gem.CellType) ||
					 IsSurroundedBySameType(stones, gem.X, gem.Y, gem.CellType));
		}


		private bool HasMatchingHorizontalPair(IBoardItem[,] stones, int x, int y, CellType type)
		{
			return (x >= 2 && (stones[x - 1, y]?.CellType == type && stones[x - 2, y]?.CellType == type) ||
				   (x <= _levelInfoService.LevelInfo.WidthOfBoard - 3 && stones[x + 1, y]?.CellType == type && stones[x + 2, y]?.CellType == type));
		}

		private bool HasMatchingVerticalPair(IBoardItem[,] stones, int x, int y, CellType type)
		{
			return ((y >= 2) && (stones[x, y - 1]?.CellType == type) && (stones[x, y - 2]?.CellType == type) ||
				   (y <= _levelInfoService.LevelInfo.HeightOfBoard - 3) && (stones[x, y + 1]?.CellType == type) && (stones[x, y + 2]?.CellType == type));
		}

		private bool IsSurroundedBySameType(IBoardItem[,] stones, int x, int y, CellType type)
		{
			return x > 0 && y > 0 && x < _levelInfoService.LevelInfo.WidthOfBoard - 1 && y < _levelInfoService.LevelInfo.HeightOfBoard - 1 &&
				  (stones[x - 1, y]?.CellType == type && stones[x + 1, y]?.CellType == type ||
				   stones[x, y - 1]?.CellType == type && stones[x, y + 1]?.CellType == type);
		}
	}
}
