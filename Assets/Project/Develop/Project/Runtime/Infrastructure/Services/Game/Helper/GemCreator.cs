using Codice.Client.BaseCommands.Merge.Xml;
using Cysharp.Threading.Tasks;
using FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets;
using FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets.Core;
using Runtime.Data.Configs;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.AssetsProvider.Containers.Core;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.MonoBehaviours.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class GemCreator
	{
		private IPrefabsFactory<GemType, GameObject> _prefabsFactory;
		private IBoardService _boardService;
		private IGameConfig _gameConfig;


		[Inject]
		private void Construct(IGameConfig gameConfig, IPrefabsFactory<GemType, GameObject> prefabsFactory, IBoardService boardService)
		{
			_prefabsFactory = prefabsFactory;
			_boardService = boardService;
			_gameConfig = gameConfig;
		}
		internal async UniTask<Gem> CreateGem(int x, int y, Vector2 offset, Transform boardParent)
		{
			var position = new Vector2(
				x * _gameConfig.CellSize + offset.x,
				(_gameConfig.Height - 1 - y) * _gameConfig.CellSize + offset.y
			);

			var gemType = GetRandomValidGemType(x, y);
			var gemObject = await _prefabsFactory.CreateGameObjectAsync(gemType, boardParent);

			var rectTransform = gemObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = position;

			var gem = gemObject.GetComponent<Gem>();
			gem.Initialize(x,y);

			gem.PlaySpawnAnimation();

			return gem;
		}


		private GemType GetRandomValidGemType(int x, int y)
		{
			var availableTypes = GetAvailableGemTypes(x, y);
			return availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
		}

		private List<GemType> GetAvailableGemTypes(int x, int y)
		{
			var availableTypes = new List<GemType>();

			foreach (var type in _boardService.GemTypes)
			{
				if (IsValidGemPlacement(x, y, type))
				{
					availableTypes.Add(type);
				}
			}

			return availableTypes.Count > 0 ? availableTypes : _boardService.GemTypes;
		}

		internal bool IsValidGemPlacement(int x, int y, GemType type)
		{
			return !(HasMatchingHorizontalPair(x, y, type) ||
					 HasMatchingVerticalPair(x, y, type) ||
					 IsSurroundedBySameType(x, y, type));
		}
		internal bool IsValidGemPlacement(Gem gem)
		{
			return !(HasMatchingHorizontalPair(gem.X, gem.Y, gem.GemType) ||
					 HasMatchingVerticalPair(gem.X, gem.Y, gem.GemType) ||
					 IsSurroundedBySameType(gem.X, gem.Y, gem.GemType));
		}


		private bool HasMatchingHorizontalPair(int x, int y, GemType type)
		{
			return (x >= 2 && (_boardService.Board[x - 1, y]?.GemType == type && _boardService.Board[x - 2, y]?.GemType == type) ||
				   (x <= _gameConfig.Width - 3 && _boardService.Board[x + 1, y]?.GemType == type && _boardService.Board[x + 2, y]?.GemType == type));
		}

		private bool HasMatchingVerticalPair(int x, int y, GemType type)
		{
			return ((y >= 2) && (_boardService.Board[x, y - 1]?.GemType == type) && (_boardService.Board[x, y - 2]?.GemType == type) ||
				   (y <= _gameConfig.Height - 3) && (_boardService.Board[x, y + 1]?.GemType == type) && (_boardService.Board[x, y + 2]?.GemType == type));
		}

		private bool IsSurroundedBySameType(int x, int y, GemType type)
		{
			return x > 0 && y > 0 && x < _gameConfig.Width - 1 && y < _gameConfig.Height - 1 &&
				  (_boardService.Board[x - 1, y]?.GemType == type && _boardService.Board[x + 1, y]?.GemType == type ||
				   _boardService.Board[x, y - 1]?.GemType == type && _boardService.Board[x, y + 1]?.GemType == type);
		}
	}
}
