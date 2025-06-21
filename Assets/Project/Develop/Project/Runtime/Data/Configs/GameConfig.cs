using Runtime.Data.Configs.Core;
using System;
using UnityEngine;

namespace Runtime.Data.Configs
{
	[CreateAssetMenu(menuName = "Data/Configs/Game", fileName = nameof(GameConfig))]
	internal sealed class GameConfig : Config, IGameConfig
	{
		[SerializeField]
		[Min(0.1F)]
		private Single _saveCooldown;
		[Header("Board Settings")]
		[SerializeField] private Single _cellSize = 100f;
		[Header("Stone")]
		[SerializeField] private Single _moveDuration = 0.3f;
		[SerializeField] private Single _spawnAnimationDuration = 0.2f;
		[SerializeField] private Single _destroyAnimationDuration = 0.8f;

		Single IGameConfig.DestroyAnimationDuration => _destroyAnimationDuration;
		Single IGameConfig.SpawnAnimationDuration => _spawnAnimationDuration;
		Single IGameConfig.MoveDuration => _moveDuration;
		Single IGameConfig.SaveCooldown => _saveCooldown;
		Single IGameConfig.CellSize => _cellSize;
	}
}
