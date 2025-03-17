using System;
using Runtime.Infrastructure.Core;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class PersistentProgress : IPersistentProgress
	{
		[SerializeField]
		private SavedInventory _savedInventory;
		[SerializeField]
		private SavedQuests _savedQuests;
		[SerializeField]
		private SavedLevels _savedLevels;

		[SerializeField]
		private Int64 _savedDateTimeTicks;
		[SerializeField]
		private String _name;

		SavedInventory IReadOnlyProgress.SavedInventory => _savedInventory;

		SavedQuests IReadOnlyProgress.SavedQuests => _savedQuests;

		SavedLevels IReadOnlyProgress.SavedLevels => _savedLevels;

		Texture2D IReadOnlyProgress.ScreenshotTexture2D { get; set; }

		DateTime IReadOnlyProgress.SavedDateTime
		{
			get => new DateTime(_savedDateTimeTicks);
			set => _savedDateTimeTicks = value.Ticks;
		}

		String IReadOnlyProgress.Name
		{
			get => _name;
			set => _name = value;
		}

		IPersistentProgress IPrototype<IPersistentProgress>.Clone()
		{
			var progress = new PersistentProgress();

			progress._savedInventory = ((IPrototype<SavedInventory>)_savedInventory).Clone();
			progress._savedQuests = ((IPrototype<SavedQuests>)_savedQuests).Clone();
			progress._savedLevels = ((IPrototype<SavedLevels>)_savedLevels).Clone();

			return progress;
		}
	}
}