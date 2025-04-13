using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Services.Game;
using Runtime.Infrastructure.Services.Game.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Runtime.MonoBehaviours.Game
{
	public class BoardInitializer : MonoBehaviour
	{
		private IBoardService _boardService;


		[Header("Board Settings")]
		[SerializeField] private Int32 _width = 8;
		[SerializeField] private Int32 _height = 8;
		[SerializeField] private Single _cellSize = 100f;

		[Header("Prefabs")]
		[SerializeField] private GameObject[] _gemPrefabs;
		[SerializeField] private Transform _boardParent;

		private Int32 _countOfGemsDestroyed;
		private Int32 _countOfGemsToBeDestroy;


		private Gem[,] _board;
		private readonly HashSet<Gem> _gemsToDestroy = new();

		internal Int32 Width => _width;
		internal Int32 Height => _height;

		internal Single CellSize => _cellSize;

		[Inject]
		private void Construct(IBoardService boardService)
		{
			_boardService = boardService;
		}

		private void Start()
		{
			_boardService.InitializeBoard(_boardParent);
		}


		internal void HandleMatchesAfterSwap()
		{
			var matches = FindAllMatches();
			if (matches.Count > 0)
			{
				DestroyMatches(matches);
			}
		}

		internal void CollapseAndRefillBoard()
		{
			CollapseBoard();
			FillBoard();
			HandleMatchesAfterSwap();
		}


		private HashSet<Gem> FindAllMatches()
		{
			_gemsToDestroy.Clear();

			for (var x = 0; x < _width; x++)
			{
				for (var y = 0; y < _height; y++)
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

			return new HashSet<Gem>(_gemsToDestroy);
		}

		private List<Gem> GetLineMatch(Gem startGem, Vector2Int direction)
		{
			List<Gem> match = new() { startGem };
			var x = startGem.X + direction.x;
			var y = startGem.Y + direction.y;

			while (IsInsideBoard(x, y))
			{
				var nextGem = _board[x, y];
				if (nextGem == null || nextGem.Type != startGem.Type) break;

				match.Add(nextGem);
				x += direction.x;
				y += direction.y;
			}

			return match;
		}

		private void CollapseBoard()
		{
			for (var x = 0; x < _width; x++)
			{
				var emptyCount = 0;
				for (var y = _height - 1; y >= 0; y--)
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

		private void FillBoard()
		{
			var offset = GetBoardOffset();

			for (var x = 0; x < _width; x++)
			{
				for (var y = 0; y < _height; y++)
				{
					if (_board[x, y] == null)
					{
						CreateGem(x, y, offset);
					}
				}
			}
		}

		private Vector2 GetGemPosition(int x, int y)
		{
			var offset = GetBoardOffset();
			return new Vector2(
				x * _cellSize + offset.x,
				(_height - 1 - y) * _cellSize + offset.y
			);
		}

		private void DestroyMatches(HashSet<Gem> matchedGems)
		{
			_countOfGemsToBeDestroy = matchedGems.Count;

			foreach (var gem in matchedGems)
			{
				_board[gem.X, gem.Y] = null;
				gem.GemDestroyComplete += CheckAndHandleGemsDestruction;
				gem.PlayDestroyAnimation();
			}
		}
		private void CheckAndHandleGemsDestruction(Gem gem)
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
	}
}
