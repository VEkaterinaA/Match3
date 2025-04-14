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
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class StoneCreator
	{
		private IPrefabsFactory<StoneType, GameObject> _prefabsFactory;
		private StoneAnimation _stoneAnimation;
		private IBoardService _boardService;
		private MatchChecker _matchChecker;
		private IGameConfig _gameConfig;


		[Inject]
		private void Construct(IGameConfig gameConfig, IPrefabsFactory<StoneType, GameObject> prefabsFactory, IBoardService boardService, MatchChecker matchChecker, StoneAnimation stoneAnimation)
		{
			_prefabsFactory = prefabsFactory;
			_stoneAnimation = stoneAnimation;
			_boardService = boardService;
			_matchChecker = matchChecker;
			_gameConfig = gameConfig;
		}


		internal async UniTask<Stone> CreateStone(int x, int y, Vector2 offset, Transform boardParent)
		{
			var position = new Vector2(
				x * _gameConfig.CellSize + offset.x,
				(_gameConfig.Height - 1 - y) * _gameConfig.CellSize + offset.y
			);

			var stoneType = GetRandomValidStoneType(x, y);
			var stoneObject = await _prefabsFactory.CreateGameObjectAsync(stoneType, boardParent);

			var rectTransform = stoneObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = position;

			var stone = stoneObject.GetComponent<Stone>();
			stone.Initialize(x,y);

			_stoneAnimation.PlaySpawnAnimation(stone);

			return stone;
		}


		private StoneType GetRandomValidStoneType(int x, int y)
		{
			var availableTypes = GetAvailableStoneTypes(x, y);
			return availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
		}

		private List<StoneType> GetAvailableStoneTypes(int x, int y)
		{
			var availableTypes = new List<StoneType>();

			foreach (var type in _boardService.GemTypes)
			{
				if (_matchChecker.IsValidGemPlacement(x, y, type))
				{
					availableTypes.Add(type);
				}
			}

			return availableTypes.Count > 0 ? availableTypes : _boardService.GemTypes;
		}		
	}
}
