using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Infrastructure.Services.Game.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Providers
{
	internal class BoardProvider
	{
		private ILevelInfoService _levelInfoService;
		private IGameConfig _gameConfig;

		internal List<CellType> StoneTypes { get; }

		[Inject]
		internal BoardProvider(IGameConfig gameConfig, ILevelInfoService levelInfoService)
		{
			_levelInfoService = levelInfoService;
			_gameConfig = gameConfig;

			StoneTypes = new List<CellType>((CellType[]) Enum.GetValues(typeof(CellType)));
		}

		internal Vector2 GetBoardOffset()
		{
			return new Vector2(
				-(_levelInfoService.LevelInfo.WidthOfBoard * _gameConfig.CellSize) / 2 + _gameConfig.CellSize / 2,
				-(_levelInfoService.LevelInfo.HeightOfBoard * _gameConfig.CellSize) / 2 + _gameConfig.CellSize / 2
			);
		}
	}
}
