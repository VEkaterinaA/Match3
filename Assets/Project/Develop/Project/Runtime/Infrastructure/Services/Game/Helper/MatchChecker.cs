using Runtime.Data.Configs;
using Runtime.Data.Configs.Core;
using Runtime.Infrastructure.Services.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class MatchChecker
	{
		private IBoardService _boardService;
		private IGameConfig _gameConfig;

		[Inject]
		private void Construct(IBoardService boardService, IGameConfig gameConfig)
		{
			_boardService = boardService;
			_gameConfig = gameConfig;
		}

		internal bool HasAnyPossibleMove()
		{
			for (var x = 0; x < _gameConfig.Width; x++)
			{
				for (var y = 0; y < _gameConfig.Height; y++)
				{
					if (CanSwapFormMatch(x, y, x + 1, y) || CanSwapFormMatch(x, y, x, y + 1))
					{
						return true;
					}
				}
			}

			return false;
		}

		private bool CanSwapFormMatch(int x1, int y1, int x2, int y2)
		{
			if (!IsInsideBoard(x2, y2))
			{
				return false;
			}

			var gem1 = _boardService.Board[x1, y1];
			var gem2 = _boardService.Board[x2, y2];

			if (gem1 == null || gem2 == null)
			{
				return false;
			}

			(_boardService.Board[x1, y1], _boardService.Board[x2, y2]) = (_boardService.Board[x2, y2], _boardService.Board[x1, y1]);

			var match = HasMatchAt(x1, y1) || HasMatchAt(x2, y2);

			(_boardService.Board[x1, y1], _boardService.Board[x2, y2]) = (_boardService.Board[x2, y2], _boardService.Board[x1, y1]);

			return match;
		}

		private bool HasMatchAt(int x, int y)
		{
			var type = _boardService.Board[x, y].GemType;

			var horizontalMatch = 1;
			for (var i = x - 1; i >= 0 && _boardService.Board[i, y]?.GemType == type; i--)
			{
				horizontalMatch++;
			}

			for (var i = x + 1; i < _gameConfig.Width && _boardService.Board[i, y]?.GemType == type; i++)
			{
				horizontalMatch++;
			}

			if (horizontalMatch >= 3)
			{
				return true;
			}

			var verticalMatch = 1;
			for (var i = y - 1; i >= 0 && _boardService.Board[x, i]?.GemType == type; i--)
			{
				verticalMatch++;
			}

			for (var i = y + 1; i < _gameConfig.Height && _boardService.Board[x, i]?.GemType == type; i++)
			{
				verticalMatch++;
			}

			return verticalMatch >= 3;
		}

		private bool IsInsideBoard(int x, int y)
		{
			return x >= 0 && x < _gameConfig.Width && y >= 0 && y < _gameConfig.Height;
		}
	}
}
