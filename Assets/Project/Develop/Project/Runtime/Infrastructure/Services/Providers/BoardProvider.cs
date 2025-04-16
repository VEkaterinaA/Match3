using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Providers
{
	internal class BoardProvider
	{
		private IGameConfig _gameConfig;

		internal List<StoneType> StoneTypes { get; }

		[Inject]
		internal BoardProvider(IGameConfig gameConfig)
		{
			_gameConfig = gameConfig;

			StoneTypes = new List<StoneType>((StoneType[]) Enum.GetValues(typeof(StoneType)));
		}

		internal Vector2 GetBoardOffset()
		{
			return new Vector2(
				-(_gameConfig.Width * _gameConfig.CellSize) / 2 + _gameConfig.CellSize / 2,
				-(_gameConfig.Height * _gameConfig.CellSize) / 2 + _gameConfig.CellSize / 2
			);
		}
	}
}
