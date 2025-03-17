using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Runtime.Infrastructure.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class SavedLevels : IPrototype<SavedLevels>
	{
		[SerializeField]
		private SerializedDictionary<String, LevelInfo> _levelInfosDictionary;

		internal LevelInfo ActiveLevelInfo => this[SceneManager.GetActiveScene().name];

		private LevelInfo this[String levelID]
		{
			get
			{
				if (!_levelInfosDictionary.TryGetValue(levelID, out var levelInfo))
				{
					levelInfo = new LevelInfo();
					_levelInfosDictionary.Add(levelID, levelInfo);
				}

				return levelInfo;
			}
		}

		SavedLevels IPrototype<SavedLevels>.Clone()
		{
			var savedLevels = new SavedLevels();

			var dictionary = _levelInfosDictionary.ToDictionary(pair => pair.Key, pair => ((IPrototype<LevelInfo>)pair.Value).Clone());
			savedLevels._levelInfosDictionary = new SerializedDictionary<String, LevelInfo>(dictionary);

			return savedLevels;
		}
	}
}