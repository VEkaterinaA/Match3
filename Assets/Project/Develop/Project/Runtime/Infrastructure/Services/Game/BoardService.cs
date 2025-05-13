using Cysharp.Threading.Tasks;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Game.Helper;
using Runtime.Infrastructure.Services.Providers;
using Runtime.MonoBehaviours.Game;
using Runtime.MonoBehaviours.Game.Core;
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

		private IBoardItem[,] _board;
		private List<CellType> _gemTypes;
		private readonly HashSet<IBoardItem> _gemsToDestroy = new();

		private Int32 _countOfGemsDestroyed;
		private Int32 _countOfGemsToBeDestroy;

		private Transform _boardParent;

		private Boolean _isInitialized;
		private Action _initialized;

		private List<(Int32 x, Int32 y, CellType type)> _boostersToCreate = new();

		private IBoardService Service => this;

		IBoardItem[,] IBoardService.Board => _board;

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

		void IBoardService.SwapGemsInBoard(IBoardItem cellOne, IBoardItem cellTwo)
		{
			(_board[cellOne.X, cellOne.Y], _board[cellTwo.X, cellTwo.Y]) = (_board[cellTwo.X, cellTwo.Y], _board[cellOne.X, cellOne.Y]);

			(cellOne.X, cellTwo.X) = (cellTwo.X, cellOne.X);
			(cellOne.Y, cellTwo.Y) = (cellTwo.Y, cellOne.Y);
		}


		void IBoardService.HandleMatchesAfterSwap()
		{
			
			var matches = FindAllMatches();

			if (matches.Count > 0)
			{
				DestroyMatches(matches);
			}
		}

		void IBoardService.TrySwapOrRevert(IBoardItem cellOne, IBoardItem CellTwo)
		{
			if (_matchChecker.IsMatchFreePlacement(_board, cellOne) && _matchChecker.IsMatchFreePlacement(_board, CellTwo))
			{
				Service.SwapGemsInBoard(cellOne, CellTwo);

				_stoneAnimation.SwapWith(cellOne, CellTwo);
			}
			else
			{
				_levelInfoService.SubsctractFromMoveLimit();
				Service.HandleMatchesAfterSwap();
			}
		}

		IBoardItem IBoardService.GetCell(Int32 x, Int32 y)
		{
			if (x >= 0 && x < _levelInfoService.LevelInfo.WidthOfBoard && y >= 0 && y < _levelInfoService.LevelInfo.HeightOfBoard)
			{
				return _board[x, y];
			}
			return null;
		}

		private HashSet<IBoardItem> FindAllMatches()
		{
			_gemsToDestroy.Clear();

			for (var x = 0; x < _levelInfoService.LevelInfo.WidthOfBoard; x++)
			{
				for (var y = 0; y < _levelInfoService.LevelInfo.HeightOfBoard; y++)
				{
					var cell = _board[x, y];
					if (cell == null) continue;

					var horizontalMatch = GetLineMatch(cell, Vector2Int.right);
					if (horizontalMatch.Count >= 3)
					{
						AddToDestroy(horizontalMatch);
						TryCreateBooster(horizontalMatch, Vector2Int.right);
					}

					var verticalMatch = GetLineMatch(cell, Vector2Int.up);
					if (verticalMatch.Count >= 3)
					{
						AddToDestroy(verticalMatch);
						TryCreateBooster(verticalMatch, Vector2Int.up);
					}
				}
			}

			return new HashSet<IBoardItem>(_gemsToDestroy);
		}

		private void TryCreateBooster(List<IBoardItem> matchList, Vector2Int direction)
		{
			if (matchList.Count == 4)
			{
				var middle = matchList[1];
				_boostersToCreate.Add((middle.X, middle.Y,
					direction == Vector2Int.right ? CellType.HorizontalBomb : CellType.VerticalBomb));
			}
			else if (matchList.Count >= 5)
			{
				var center = matchList[2];
				_boostersToCreate.Add((center.X, center.Y, CellType.RadiusBomb));
			}
		}

		private void AddToDestroy(IEnumerable<IBoardItem> cells)
		{
			foreach (var cell in cells)
			{
				_gemsToDestroy.Add(cell);
			}
		}


		private List<IBoardItem> GetLineMatch(IBoardItem startCell, Vector2Int direction)
		{
			List<IBoardItem> match = new() { startCell };
			var x = startCell.X + direction.x;
			var y = startCell.Y + direction.y;

			while (_matchChecker.IsInsideBoard(x, y))
			{
				var nextCell = _board[x, y];
				if (nextCell == null || nextCell.CellType != startCell.CellType) break;

				match.Add(nextCell);
				x += direction.x;
				y += direction.y;
			}

			return match;
		}

		private void DestroyMatches(HashSet<IBoardItem> matchedCells)
		{
			_countOfGemsToBeDestroy = matchedCells.Count;

			var targetType = _levelInfoService.LevelInfo.TargetType;

			var cellType = EnumExtensions.ConvertToTargetType(matchedCells.First().CellType);

			foreach (var cell in matchedCells)
			{
				_board[cell.X, cell.Y] = null;

				if(cellType == targetType)
				{
					_levelInfoService.SubsctractFromGoalQuantity();
				}

				cell.CellDestroyComplete += CheckAndHandleGemsDestruction;

				_stoneAnimation.PlayDestroyAnimation(cell);
			}
		}
		private async void CheckAndHandleGemsDestruction(IBoardItem cell)
		{
			cell.CellDestroyComplete -= CheckAndHandleGemsDestruction;

			_countOfGemsDestroyed++;

			if (_countOfGemsToBeDestroy == _countOfGemsDestroyed)
			{
				_countOfGemsToBeDestroy = 0;
				_countOfGemsDestroyed = 0;

				foreach (var (x, y, type) in _boostersToCreate)
				{
					var booster = await _stoneCreator.CreateBoosterStone(x, y, type, _boardProvider.GetBoardOffset(), _boardParent);
					_board[x, y] = booster;
					_stoneAnimation.PlaySpawnAnimation(booster);
				}

				_boostersToCreate.Clear();

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
						var cell = _board[x, y];
						_board[x, y + emptyCount] = cell;
						_board[x, y] = null;

						cell.Y = y + emptyCount;

						var newPos = GetGemPosition(cell.X, cell.Y);
						cell.RectTransform.anchoredPosition = newPos;
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

			foreach (var cellItem in _board)
			{
				if (cellItem != null)
				{
					UnityEngine.Object.DestroyImmediate(cellItem.GameObject);
				}
			}
		}
	}
}