using Runtime.Infrastructure.Core;
using System;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	public class Settings  : IPrototype<Settings>
	{
		[SerializeField]
		[Range(0.0f, 1.0f)]
		public Single MusicVolumeMultiplier;
		[SerializeField]
		[Range(0.0f, 1.0f)]
		public Single SoundVolumeMultiplier;


		Settings IPrototype<Settings>.Clone()
		{
			var settings = new Settings();

			settings.MusicVolumeMultiplier = MusicVolumeMultiplier;
			settings.SoundVolumeMultiplier = SoundVolumeMultiplier;

			return settings;
		}
	}
}
