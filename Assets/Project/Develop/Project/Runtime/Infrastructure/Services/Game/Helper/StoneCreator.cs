using Cysharp.Threading.Tasks;
using FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets.Core;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Providers;
using Runtime.MonoBehaviours.Game;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class StoneCreator
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


		internal async UniTask<Stone> CreateStone(Stone[,] stones, int x, int y, Vector2 offset, Transform boardParent)
		{
			var position = new Vector2(
				x * _gameConfig.CellSize + offset.x,
				(_levelInfoService.LevelInfo.HeightOfBoard - 1 - y) * _gameConfig.CellSize + offset.y
			);

			var stoneType = GetRandomValidStoneType(stones, x, y);
			var stoneObject = await _prefabsFactory.CreateGameObjectAsync(EnumExtensions.ConvertToPrefabType(stoneType), boardParent);

			var rectTransform = stoneObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = position;

			var stone = stoneObject.GetComponent<Stone>();
			stone.Initialize(x, y);

			return stone;
		}


		private StoneType GetRandomValidStoneType(Stone[,] stones, int x, int y)
		{
			var availableTypes = GetAvailableStoneTypes(stones, x, y);
			return availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
		}

		private List<StoneType> GetAvailableStoneTypes(Stone[,] stones, int x, int y)
		{
			var availableTypes = new List<StoneType>();

			foreach (var type in _stoneTypeProvider.StoneTypes)
			{
				if (_matchChecker.IsValidGemPlacement(stones, x, y, type))
				{
					availableTypes.Add(type);
				}
			}

			return availableTypes.Count > 0 ? availableTypes : _stoneTypeProvider.StoneTypes;
		}
	}
}
