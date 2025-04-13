using System;

namespace Runtime.Data.Configs.Core
{
	internal interface IGameConfig
	{
		internal Single DestroyAnimationDuration { get; }
		internal Single SpawnAnimationDuration { get; }
		internal Single MoveDuration { get; }

		internal Single SaveCooldown { get; }
		internal Single CellSize { get; }
		internal Int32 Height { get; }
		internal Int32 Width { get; }
	}
}
