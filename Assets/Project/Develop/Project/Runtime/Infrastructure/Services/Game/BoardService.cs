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
		private BoardItemAnimation _boardItemAnimation;
		private BoosterCreator _boosterCreator;
		private BoardProvider _boardProvider;
		private MatchChecker _matchChecker;
		private ItemCreator _itemCreator;

		private IBoardItem[,] _board;

		private readonly HashSet<IBoardItem> _gemsToDestroy = new();

		private Int32 _countOfBoardItemDestroyed;
		private Int32 _countOfBoardItemToBeDestroy;

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
		private void Construct(IGameConfig gameConfig, ItemCreator stoneCreator, MatchChecker matchChecker, BoardItemAnimation stoneAnimation, BoardProvider boardProvider, ILevelInfoService levelInfoService,
								BoosterCreator boosterCreator)
		{
			_levelInfoService = levelInfoService;
			_boardItemAnimation = stoneAnimation;
			_boosterCreator = boosterCreator;
			_boardProvider = boardProvider;
			_matchChecker = matchChecker;
			_itemCreator = stoneCreator;
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
				_board = new IBoardItem[width,height];
				var offset = _boardProvider.GetBoardOffset();

				for (var x = 0; x < width; x++)
				{
					for (var y = 0; y < height; y++)
					{
						_board[x, y] = await _itemCreator.CreateStone(_board, x, y, offset, boardParent);
					}
				}

				for (var x = 0; x < width; x++)
				{
					for (var y = 0; y < height; y++)
					{
						_boardItemAnimation.PlaySpawnAnimation(_board[x, y]);
					}
				}
			}
			while (!_matchChecker.HasAnyPossibleMove(_board));

			_isInitialized = true;
			_initialized?.Invoke();
		}

		void IBoardService.RunBooster(IBoardItem booster)
		{
			var x = booster.X;
			var y = booster.Y;

			HashSet<IBoardItem> cellsToDestroy = new();

			cellsToDestroy.Add(booster);

			switch (booster.CellType)
			{
				case CellType.HorizontalBomb:
					for (var col = 0; col < _levelInfoService.LevelInfo.WidthOfBoard; col++)
					{
						var cell = _board[col, y];
						if (cell != null && !cell.IsBooster)
						{
							cellsToDestroy.Add(cell);
						}
					}
					break;

				case CellType.VerticalBomb:
					for (var row = 0; row < _levelInfoService.LevelInfo.HeightOfBoard; row++)
					{
						var cell = _board[x, row];
						if (cell != null && !cell.IsBooster)
						{
							cellsToDestroy.Add(cell);
						}
					}
					break;

				case CellType.RadiusBomb:
					for (var dx = -1; dx <= 1; dx++)
					{
						for (var dy = -1; dy <= 1; dy++)
						{
							var nx = x + dx;
							var ny = y + dy;

							if (_matchChecker.IsInsideBoard(nx, ny))
							{
								var cell = _board[nx, ny];
								if (cell != null && !cell.IsBooster)
								{
									cellsToDestroy.Add(cell);
								}
							}
						}
					}
					break;

				default:
					Debug.LogWarning($"Booster of type {booster.CellType} has no effect implementation.");
					return;
			}

			_board[x, y] = null;

			if (cellsToDestroy.Count > 0)
			{
				DestroyMatches(cellsToDestroy);
			}
		}

		void IBoardService.SwapGemsInBoard(IBoardItem cellOne, IBoardItem cellTwo)
		{
			(_board[cellOne.X, cellOne.Y], _board[cellTwo.X, cellTwo.Y]) = (_board[cellTwo.X, cellTwo.Y], _board[cellOne.X, cellOne.Y]);

			(cellOne.X, cellTwo.X) = (cellTwo.X, cellOne.X);
			(cellOne.Y, cellTwo.Y) = (cellTwo.Y, cellOne.Y);
		}


		void IBoardService.HandleMatchesAfterSwap()
		{
			var allMatches = FindAllMatches();

			_boostersToCreate = _boosterCreator.GetBoostersToCreate(allMatches);


			if (allMatches.Count > 0)
			{
				var matchedCells = new HashSet<IBoardItem>();
				foreach (var match in allMatches)
				{
					foreach (var cell in match)
					{
						matchedCells.Add(cell);
					}
				}

				DestroyMatches(matchedCells);
			}
		}

		void IBoardService.TrySwapOrRevert(IBoardItem cellOne, IBoardItem CellTwo)
		{
			if (_matchChecker.IsMatchFreePlacement(_board, cellOne) && _matchChecker.IsMatchFreePlacement(_board, CellTwo))
			{
				Service.SwapGemsInBoard(cellOne, CellTwo);

				_boardItemAnimation.SwapWith(cellOne, CellTwo);
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

		private List<List<IBoardItem>> FindAllMatches()
		{
			var allMatches = new List<List<IBoardItem>>();

			for (var x = 0; x < _levelInfoService.LevelInfo.WidthOfBoard; x++)
			{
				for (var y = 0; y < _levelInfoService.LevelInfo.HeightOfBoard; y++)
				{
					var cell = _board[x, y];
					if (cell == null)
					{
						continue;
					}

					var vertical = GetLineMatch(cell, Vector2Int.right);
					if (vertical.Count >= 3)
					{
						allMatches.Add(vertical);
					}

					var horizontal = GetLineMatch(cell, Vector2Int.up);
					if (horizontal.Count >= 3)
					{
						allMatches.Add(horizontal);
					}
				}
			}

			return allMatches;
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
				if (nextCell == null || nextCell.CellType != startCell.CellType)
				{
					break;
				}

				match.Add(nextCell);
				x += direction.x;
				y += direction.y;
			}

			return match;
		}

		private void DestroyMatches(HashSet<IBoardItem> matchedCells)
		{
			_countOfBoardItemToBeDestroy = matchedCells.Count;

			var targetType = _levelInfoService.LevelInfo.TargetType;

			var cellType = EnumExtensions.ConvertToTargetType(matchedCells.First().CellType);

			foreach (var cell in matchedCells)
			{
				_board[cell.X, cell.Y] = null;

				if(cellType == targetType)
				{
					_levelInfoService.SubsctractFromGoalQuantity();
				}

				cell.CellDestroyComplete += CheckAndHandleBoardItemsDestruction;

				_boardItemAnimation.PlayDestroyAnimation(cell);
			}
		}
		private async void CheckAndHandleBoardItemsDestruction(IBoardItem cell)
		{
			cell.CellDestroyComplete -= CheckAndHandleBoardItemsDestruction;

			_countOfBoardItemDestroyed++;

			if (_countOfBoardItemToBeDestroy == _countOfBoardItemDestroyed)
			{
				_countOfBoardItemToBeDestroy = 0;
				_countOfBoardItemDestroyed = 0;

				foreach (var (x, y, type) in _boostersToCreate)
				{
					var booster = await _itemCreator.CreateBooster(x, y, type, _boardProvider.GetBoardOffset(), _boardParent);
					_board[x, y] = booster;
					_boardItemAnimation.PlaySpawnAnimation(booster);
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
						_board[x, y] = await _itemCreator.CreateStone(_board, x, y, offset, _boardParent);
						_boardItemAnimation.PlaySpawnAnimation(_board[x, y]);
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