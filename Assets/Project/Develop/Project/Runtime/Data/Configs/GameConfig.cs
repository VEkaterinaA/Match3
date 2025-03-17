using Runtime.Data.Configs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runtime.Data.Configs
{
	[CreateAssetMenu(menuName = "Data/Configs/Game", fileName = nameof(GameConfig))]
	internal sealed class GameConfig : Config, IGameConfig
	{
		[SerializeField]
		[Min(0.1F)]
		private Single _saveCooldown;

		Single IGameConfig.SaveCooldown => _saveCooldown;
	}
}
