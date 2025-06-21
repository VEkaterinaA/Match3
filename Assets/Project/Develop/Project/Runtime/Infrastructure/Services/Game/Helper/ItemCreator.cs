using Cysharp.Threading.Tasks;
using FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets.Core;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Providers;
using Runtime.MonoBehaviours.Game.Core;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class ItemCreator
	{
		private IPrefabsFactory<PrefabType, GameObject> _prefabsFactory;
		private ILevelInfoService _levelInfoService;
		private BoardProvider _stoneTypeProvider;
		private MatchChecker _matchChecker;
		private IGameConfig _gameConfig;


		[Inject]
		private void Construct(IGameConfig gameConfig, IPrefabsFactory<PrefabType, GameObject> prefabsFactory, MatchChecker matchChecker, BoardProvider stoneTypeProvider,
								ILevelInfoService levelInfoService)
		{
			_stoneTypeProvider = stoneTypeProvider;
			_levelInfoService = levelInfoService;
			_prefabsFactory = prefabsFactory;
			_matchChecker = matchChecker;
			_gameConfig = gameConfig;
		}


		internal async UniTask<IBoardItem> CreateStone(IBoardItem[,] stones, int x, int y, Vector2 offset, Transform boardParent)
		{
			var position = new Vector2(
				x * _gameConfig.CellSize + offset.x,
				(_levelInfoService.LevelInfo.HeightOfBoard - 1 - y) * _gameConfig.CellSize + offset.y
			);

			var stoneType = GetRandomValidStoneType(stones, x, y);
			var stoneObject = await _prefabsFactory.CreateGameObjectAsync(EnumExtensions.ConvertToPrefabType(stoneType), boardParent);

			var stone = stoneObject.GetComponent<IBoardItem>();
			stone.RectTransform.anchoredPosition = position;

			stone.Initialize(x, y);

			return stone;
		}

		internal async UniTask<IBoardItem> CreateBooster(int x, int y, CellType boosterType, Vector2 offset, Transform boardParent)
		{
			var position = new Vector2(
				x * _gameConfig.CellSize + offset.x,
				(_levelInfoService.LevelInfo.HeightOfBoard - 1 - y) * _gameConfig.CellSize + offset.y);

			var boosterObject = await _prefabsFactory.CreateGameObjectAsync(EnumExtensions.ConvertToPrefabType(boosterType), boardParent);

			var booster = boosterObject.GetComponent<IBoardItem>();
			booster.RectTransform.anchoredPosition = position;

			booster.Initialize(x, y);

			return booster;
		}

		private CellType GetRandomValidStoneType(IBoardItem[,] stones, int x, int y)
		{
			var availableTypes = GetAvailableStoneTypes(stones, x, y);
			return availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
		}

		private List<CellType> GetAvailableStoneTypes(IBoardItem[,] stones, int x, int y)
		{
			var availableTypes = new List<CellType>();

			foreach (var type in _stoneTypeProvider.StoneTypes)
			{
				if (_matchChecker.IsMatchFreePlacement(stones, x, y, type))
				{
					availableTypes.Add(type);
				}
			}

			return availableTypes.Count > 0 ? availableTypes : _stoneTypeProvider.StoneTypes;
		}
	}
}
