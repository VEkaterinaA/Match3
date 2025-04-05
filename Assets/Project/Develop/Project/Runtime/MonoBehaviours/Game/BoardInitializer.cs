using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.MonoBehaviours.Game
{
	public class BoardInitializer : MonoBehaviour
	{
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

		private void Start()
		{
			InitializeBoard();
		}

		internal Gem GetGem(Int32 x, Int32 y)
		{
			if (x >= 0 && x < _width && y >= 0 && y < _height)
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

		private void InitializeBoard()
		{
			do
			{
				ClearBoard();
				_board = new Gem[_width, _height];
				var offset = GetBoardOffset();

				for (var x = 0; x < _width; x++)
				{
					for (var y = 0; y < _height; y++)
					{
						CreateGem(x, y, offset);
					}
				}
			}
			while (!HasAnyPossibleMove());
		}


		internal Vector2 GetBoardOffset()
		{
			return new Vector2(
				-(_width * _cellSize) / 2 + _cellSize / 2,
				-(_height * _cellSize) / 2 + _cellSize / 2
			);
		}

		private void CreateGem(int x, int y, Vector2 offset)
		{
			var position = new Vector2(
				x * _cellSize + offset.x,
				(_height - 1 - y) * _cellSize + offset.y
			);

			var gemType = GetRandomValidGemType(x, y);
			var gemObject = Instantiate(_gemPrefabs[gemType], _boardParent);

			var rectTransform = gemObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = position;

			var gem = gemObject.GetComponent<Gem>();
			gem.Type = gemType;
			gem.X = x;
			gem.Y = y;

			gem.PlaySpawnAnimation();

			_board[x, y] = gem;
		}

		private int GetRandomValidGemType(int x, int y)
		{
			var availableTypes = GetAvailableGemTypes(x, y);
			return availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
		}

		private List<int> GetAvailableGemTypes(int x, int y)
		{
			var availableTypes = new List<int>();

			for (var type = 0; type < _gemPrefabs.Length; type++)
			{
				if (IsValidGemPlacement(x, y, type))
				{
					availableTypes.Add(type);
				}
			}

			return availableTypes.Count > 0 ? availableTypes : new List<int>(_gemPrefabs.Length);
		}

		internal bool IsValidGemPlacement(int x, int y, int type)
		{
			return !(HasMatchingHorizontalPair(x, y, type) ||
					 HasMatchingVerticalPair(x, y, type) ||
					 IsSurroundedBySameType(x, y, type));
		}
		internal bool IsValidGemPlacement(Gem gem)
		{
			return !(HasMatchingHorizontalPair(gem.X, gem.Y, gem.Type) ||
					 HasMatchingVerticalPair(gem.X, gem.Y, gem.Type) ||
					 IsSurroundedBySameType(gem.X, gem.Y, gem.Type));
		}


		private bool HasMatchingHorizontalPair(int x, int y, int type)
		{
			return (x >= 2 && (_board[x - 1, y]?.Type == type && _board[x - 2, y]?.Type == type) || (x <= _width - 3 && _board[x + 1, y]?.Type == type && _board[x + 2, y]?.Type == type));
		}

		private bool HasMatchingVerticalPair(int x, int y, int type)
		{
			return (y >= 2 && (_board[x, y - 1]?.Type == type && _board[x, y - 2]?.Type == type) || (y <= _height - 3 && _board[x, y + 1]?.Type == type && _board[x, y + 2]?.Type == type));
		}

		private bool IsSurroundedBySameType(int x, int y, int type)
		{
			return x > 0 && y > 0 && x < _width - 1 && y < _height - 1 &&
				   _board[x - 1, y]?.Type == type && _board[x + 1, y]?.Type == type &&
				   _board[x, y - 1]?.Type == type && _board[x, y + 1]?.Type == type;
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
					DestroyImmediate(gem.gameObject);
				}
			}
		}

		private bool HasAnyPossibleMove()
		{
			for (var x = 0; x < _width; x++)
			{
				for (var y = 0; y < _height; y++)
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

			var gem1 = _board[x1, y1];
			var gem2 = _board[x2, y2];

			if (gem1 == null || gem2 == null)
			{
				return false;
			}

			(_board[x1, y1], _board[x2, y2]) = (_board[x2, y2], _board[x1, y1]);

			var match = HasMatchAt(x1, y1) || HasMatchAt(x2, y2);

			(_board[x1, y1], _board[x2, y2]) = (_board[x2, y2], _board[x1, y1]);

			return match;
		}

		private bool HasMatchAt(int x, int y)
		{
			var type = _board[x, y].Type;

			var horizontalMatch = 1;
			for (var i = x - 1; i >= 0 && _board[i, y]?.Type == type; i--)
			{
				horizontalMatch++;
			}

			for (var i = x + 1; i < _width && _board[i, y]?.Type == type; i++)
			{
				horizontalMatch++;
			}

			if (horizontalMatch >= 3)
			{
				return true;
			}

			var verticalMatch = 1;
			for (var i = y - 1; i >= 0 && _board[x, i]?.Type == type; i--)
			{
				verticalMatch++;
			}

			for (var i = y + 1; i < _height && _board[x, i]?.Type == type; i++)
			{
				verticalMatch++;
			}

			return verticalMatch >= 3;
		}

		private bool IsInsideBoard(int x, int y)
		{
			return x >= 0 && x < _width && y >= 0 && y < _height;
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
