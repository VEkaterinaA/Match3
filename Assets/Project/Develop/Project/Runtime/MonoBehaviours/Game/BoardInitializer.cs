using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

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

		private Gem[,] _board;

		public Int32 Width => _width;
		public Int32 Height => _height;

		public Single CellSize => _cellSize;

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

		private void InitializeBoard()
		{
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

			var randomGemType = UnityEngine.Random.Range(0, _gemPrefabs.Length);
			var gemObject = Instantiate(_gemPrefabs[randomGemType], _boardParent);
			
			var rectTransform = gemObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = position;
			
			var gem = gemObject.GetComponent<Gem>();
			gem.Type = randomGemType;
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

		private bool IsValidGemPlacement(int x, int y, int type)
		{
			return !(HasMatchingHorizontalPair(x, y, type) ||
					 HasMatchingVerticalPair(x, y, type) ||
					 IsSurroundedBySameType(x, y, type));
		}

		private bool HasMatchingHorizontalPair(int x, int y, int type)
		{
			return x >= 2 && _board[x - 1, y]?.Type == type && _board[x - 2, y]?.Type == type;
		}

		private bool HasMatchingVerticalPair(int x, int y, int type)
		{
			return y >= 2 && _board[x, y - 1]?.Type == type && _board[x, y - 2]?.Type == type;
		}

		private bool IsSurroundedBySameType(int x, int y, int type)
		{
			return x > 0 && y > 0 && x < _width - 1 && y < _height - 1 &&
				   _board[x - 1, y]?.Type == type && _board[x + 1, y]?.Type == type &&
				   _board[x, y - 1]?.Type == type && _board[x, y + 1]?.Type == type;
		}

	}
}
