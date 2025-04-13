using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Game.Helper;
using Runtime.MonoBehaviours.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Game
{
	internal class BoardService : IBoardService
	{
		private MatchChecker _matchChecker;
		private GemCreator _gemCreator;

		private IGameConfig _gameConfig;



		private Gem[,] _board;
		private List<GemType> _gemTypes;

		Gem[,] IBoardService.Board => _board;

		List<GemType> IBoardService.GemTypes => _gemTypes;

		[Inject]
		private void Construct(IGameConfig gameConfig, GemCreator gemCreator, MatchChecker matchChecker)
		{
			_matchChecker = matchChecker;
			_gameConfig = gameConfig;
			_gemCreator = gemCreator;

			_gemTypes = new List<GemType>((GemType[]) Enum.GetValues(typeof(GemType)));
		}

		async UniTask IBoardService.InitializeBoard(Transform boardParent)
		{
			do
			{
				ClearBoard();
				_board = new Gem[_gameConfig.Width, _gameConfig.Height];
				var offset = GetBoardOffset();

				for (var x = 0; x < _gameConfig.Width; x++)
				{
					for (var y = 0; y < _gameConfig.Height; y++)
					{
						_board[x, y] = await _gemCreator.CreateGem(x,y, offset, boardParent);
					}
				}
			}
			while (!_matchChecker.HasAnyPossibleMove());
		}

		internal Gem GetGem(Int32 x, Int32 y)
		{
			if (x >= 0 && x < _gameConfig.Width && y >= 0 && y < _gameConfig.Height)
			{
				return _board[x, y];
			}
			return null;
		}

		internal void SwapGemsInBoard(Gem gem1, Gem gem2)
		{
			(_board[gem1.X, gem1.Y], _board[gem2.X, gem2.Y]) = (_board[gem2.X, gem2.Y], _board[gem1.X, gem1.Y]);

			(gem1.X, gem2.X) = (gem2.X, gem1.X);
			(gem1.Y, gem2.Y) = (gem2.Y, gem1.Y);
		}

		internal Vector2 GetBoardOffset()
		{
			return new Vector2(
				-(_gameConfig.Width * _gameConfig.CellSize) / 2 + _gameConfig.CellSize / 2,
				-(_gameConfig.Height * _gameConfig.CellSize) / 2 + _gameConfig.CellSize / 2
			);
		}

		private void ClearBoard()
		{
			if (_board == null)
			{
				return;
			}

			foreach (var gem in _board)
			{
				if (gem != null)
				{
                    UnityEngine.Object.DestroyImmediate(gem.gameObject);
				}
			}
		}
	}
}