using Runtime.Infrastructure.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class LevelInfo : IPrototype<LevelInfo>
	{
/*		[SerializeField]
		private List<String> _collectedInventoryItemsBehavioursIDs;

		[SerializeField]
		private SerializableVector3 _playerPosition;
		[SerializeField]
		private Byte _environmentID;

		[SerializeField]
		private LightningPresetType _lightningPresetType;
		[SerializeField]
		private FogType _fogType;

		internal List<String> CollectedInventoryItemsBehavioursIDs => _collectedInventoryItemsBehavioursIDs;

		internal EnvironmentBehaviour ActiveEnvironmentBehaviour { get; set; }

		internal LightningPresetType LightningPresetType
		{
			get => _lightningPresetType;
			set => _lightningPresetType = value;
		}
		internal FogType FogPresetType
		{
			get => _fogType;
			set => _fogType = value;
		}

		internal Vector3 PlayerPosition
		{
			get => _playerPosition;
			set => _playerPosition = value;
		}

		internal Byte EnvironmentID
		{
			get => _environmentID;
			set => _environmentID = value;
		}*/

		LevelInfo IPrototype<LevelInfo>.Clone()
		{
			var levelInfo = new LevelInfo();

/*			levelInfo._collectedInventoryItemsBehavioursIDs = new List<String>(_collectedInventoryItemsBehavioursIDs);
			levelInfo._lightningPresetType = _lightningPresetType;
			levelInfo._playerPosition = _playerPosition;
			levelInfo._environmentID = _environmentID;*/

			return levelInfo;
		}
	}
}