using System;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class SavedFlashLight
	{
		[field: SerializeField]
		internal Vector2 PointLightAngles { get; private set; }

		[field: SerializeField]
		internal Vector2 PointLightRadii { get; private set; }

		[field: SerializeField]
		internal Vector2 SpotLightAngles { get; private set; }

		[field: SerializeField]
		internal Vector2 SpotLightRadii { get; private set; }
	}
}