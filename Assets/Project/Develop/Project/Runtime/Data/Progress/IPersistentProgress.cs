using System;
using Runtime.Infrastructure.Core;
using UnityEngine;

namespace Runtime.Data.Progress
{
	internal interface IReadOnlyProgress : IPrototype<IPersistentProgress>
	{
		internal SavedInventory SavedInventory { get; }

		internal SavedQuests SavedQuests { get; }

		internal SavedLevels SavedLevels { get; }

		internal Texture2D ScreenshotTexture2D { get; set; }

		internal DateTime SavedDateTime { get; set; }

		internal String Name { get; set; }
	}

	internal interface IPersistentProgress : IReadOnlyProgress
	{
	}
}