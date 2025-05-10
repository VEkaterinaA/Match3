using Cysharp.Threading.Tasks;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Game.Helper;
using Runtime.Infrastructure.Services.Providers;
using Runtime.MonoBehaviours.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using VContainer;

namespace Runtime.Infrastructure.Services.Game
{
	internal class BoardService : IBoardService, IInitializationInformer
	{
		private ILevelInfoService _levelInfoService;
		private IGameConfig _gameConfig;
		private StoneAnimation _stoneAnimation;
		private BoardProvider _boardProvider;
		private MatchChecker _matchChecker;
		private StoneCreator _stoneCreator;

		private Stone[,] _board;
		private List<StoneType> _gemTypes;
		private readonly HashSet<Stone> _gemsToDestroy = new();

		private Int32 _countOfGemsDestroyed;
		private Int32 _countOfGemsToBeDestroy;

		private Transform _boardParent;

		private Boolean _isInitialized;
		private Action _initialized;

		private IBoardService Service => this;

		Stone[,] IBoardService.Board => _board;

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		[Inject]
		private void Construct(IGameConfig gameConfig, StoneCreator stoneCreator, MatchChecker matchChecker, StoneAnimation stoneAnimation, BoardProvider boardProvider, ILevelInfoService levelInfoService)
		{
			_levelInfoService = levelInfoService;
			_stoneAnimation = stoneAnimation;
			_boardProvider = boardProvider;
			_matchChecker = matchChecker;
			_stoneCreator = stoneCreator;
			_gameConfig = gameConfig;
		}

		async UniTask IBoardService.InitializeBoard(Transform boardParent)
		{
			_boardParent = boardParent;
			do
			{
				var width = _levelInfoService.LevelInfo.WidthOfBoard;
				var height = _levelInfoService.LevelInfo.HeightOfBoard;
				ClearBoard();
				_board = new Stone[width,height];
				var offset = _boardProvider.GetBoardOffset();

				for (var x = 0; x < width; x++)
				{
					for (var y = 0; y < height; y++)
					{
						_board[x, y] = await _stoneCreator.CreateStone(_board, x, y, offset, boardParent);
					}
				}

				for (var x = 0; x < width; x++)
				{
					for (var y = 0; y < height; y++)
					{
						_stoneAnimation.PlaySpawnAnimation(_board[x, y]);
					}
				}
			}
			while (!_matchChecker.HasAnyPossibleMove(_board));

			_isInitialized = true;
			_initialized?.Invoke();
		}

		void IBoardService.SwapGemsInBoard(Stone gem1, Stone gem2)
		{
			(_board[gem1.X, gem1.Y], _board[gem2.X, gem2.Y]) = (_board[gem2.X, gem2.Y], _board[gem1.X, gem1.Y]);

			(gem1.X, gem2.X) = (gem2.X, gem1.X);
			(gem1.Y, gem2.Y) = (gem2.Y, gem1.Y);
		}


		void IBoardService.HandleMatchesAfterSwap()
		{
			
			var matches = FindAllMatches();

			if (matches.Count > 0)
			{
				DestroyMatches(matches);
			}
		}

		void IBoardService.TrySwapOrRevert(Stone stoneOne, Stone stoneTwo)
		{
			if (_matchChecker.IsMatchFreePlacement(_board, stoneOne) && _matchChecker.IsMatchFreePlacement(_board, stoneTwo))
			{
				Service.SwapGemsInBoard(stoneOne, stoneTwo);

				_stoneAnimation.SwapWith(stoneOne, stoneTwo);
			}
			else
			{
				_levelInfoService.SubsctractFromMoveLimit();
				Service.HandleMatchesAfterSwap();
			}
		}

		Stone IBoardService.GetStone(Int32 x, Int32 y)
		{
			if (x >= 0 && x < _levelInfoService.LevelInfo.WidthOfBoard && y >= 0 && y < _levelInfoService.LevelInfo.HeightOfBoard)
			{
				return _board[x, y];
			}
			return null;
		}

		private HashSet<Stone> FindAllMatches()
		{
			_gemsToDestroy.Clear();

			for (var x = 0; x < _levelInfoService.LevelInfo.WidthOfBoard; x++)
			{
				for (var y = 0; y < _levelInfoService.LevelInfo.HeightOfBoard; y++)
				{
					var gem = _board[x, y];
					if (gem == null) continue;

					var horizontalMatch = GetLineMatch(gem, Vector2Int.right);
					if (horizontalMatch.Count >= 3)
					{
						foreach (var matchGem in horizontalMatch)
							_gemsToDestroy.Add(matchGem);
					}

					var verticalMatch = GetLineMatch(gem, Vector2Int.up);
					if (verticalMatch.Count >= 3)
					{
						foreach (var matchGem in verticalMatch)
							_gemsToDestroy.Add(matchGem);
					}
				}
			}

			return new HashSet<Stone>(_gemsToDestroy);
		}

		private List<Stone> GetLineMatch(Stone startGem, Vector2Int direction)
		{
			List<Stone> match = new() { startGem };
			var x = startGem.X + direction.x;
			var y = startGem.Y + direction.y;

			while (_matchChecker.IsInsideBoard(x, y))
			{
				var nextGem = _board[x, y];
				if (nextGem == null || nextGem.StoneType != startGem.StoneType) break;

				match.Add(nextGem);
				x += direction.x;
				y += direction.y;
			}

			return match;
		}

		private void DestroyMatches(HashSet<Stone> matchedStones)
		{
			_countOfGemsToBeDestroy = matchedStones.Count;

			var targetType = _levelInfoService.LevelInfo.TargetType;

			var stoneType = EnumExtensions.ConvertToTargetType(matchedStones.First().StoneType);

			foreach (var stone in matchedStones)
			{
				_board[stone.X, stone.Y] = null;

				if(stoneType == targetType)
				{
					_levelInfoService.SubsctractFromGoalQuantity();
				}

				stone.GemDestroyComplete += CheckAndHandleGemsDestruction;

				_stoneAnimation.PlayDestroyAnimation(stone);
			}
		}
		private void CheckAndHandleGemsDestruction(Stone gem)
		{
			gem.GemDestroyComplete -= CheckAndHandleGemsDestruction;

			_countOfGemsDestroyed++;

			if (_countOfGemsToBeDestroy == _countOfGemsDestroyed)
			{
				_countOfGemsToBeDestroy = 0;
				_countOfGemsDestroyed = 0;

				CollapseAndRefillBoard();
			}
		}

		private void CollapseAndRefillBoard()
		{
			CollapseBoard();
			FillBoard();
			Service.HandleMatchesAfterSwap();
		}

		private void CollapseBoard()
		{
			for (var x = 0; x < _levelInfoService.LevelInfo.WidthOfBoard; x++)
			{
				var emptyCount = 0;
				for (var y = _levelInfoService.LevelInfo.HeightOfBoard - 1; y >= 0; y--)
				{
					if (_board[x, y] == null)
					{
						emptyCount++;
					}
					else if (emptyCount > 0)
					{
						var gem = _board[x, y];
						_board[x, y + emptyCount] = gem;
						_board[x, y] = null;

						gem.Y = y + emptyCount;

						var newPos = GetGemPosition(gem.X, gem.Y);
						gem.GetComponent<RectTransform>().anchoredPosition = newPos;
					}
				}
			}
		}

		private Vector2 GetGemPosition(int x, int y)
		{
			var offset = _boardProvider.GetBoardOffset();
			return new Vector2(
				x * _gameConfig.CellSize + offset.x,
				(_levelInfoService.LevelInfo.HeightOfBoard - 1 - y) * _gameConfig.CellSize + offset.y
			);
		}

		private async UniTask FillBoard()
		{
			var offset = _boardProvider.GetBoardOffset();

			for (var x = 0; x < _levelInfoService.LevelInfo.WidthOfBoard; x++)
			{
				for (var y = 0; y < _levelInfoService.LevelInfo.HeightOfBoard; y++)
				{
					if (_board[x, y] == null)
					{
						_board[x, y] = await _stoneCreator.CreateStone(_board, x, y, offset, _boardParent);
						_stoneAnimation.PlaySpawnAnimation(_board[x, y]);
					}
				}
			}
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