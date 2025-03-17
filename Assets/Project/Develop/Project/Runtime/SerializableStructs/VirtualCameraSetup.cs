using Runtime.MonoBehaviours;
using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Runtime.SerializableStructs
{
	[Serializable]
	internal struct VirtualCameraSetup
	{
		[SerializeField]
		private PolygonCollider2D _confinerCollider2D;

		[SerializeField]
		private Vector3 _localPosition;
		[SerializeField]
		private Vector3 _localEulerAngles;
		[SerializeField]
		private Single _damping;

		internal void ApplyInstantly(CinemachineConfiner2D cinemachineConfiner2D, FollowTargetCinemachineComponent followTargetCinemachineComponent)
		{
			followTargetCinemachineComponent.VirtualCamera.transform.localEulerAngles = _localEulerAngles;
			followTargetCinemachineComponent.ForceLocalPosition(_localPosition);

			followTargetCinemachineComponent.Damping = _damping;

			cinemachineConfiner2D.BoundingShape2D = _confinerCollider2D;
			cinemachineConfiner2D.InvalidateCache();
		}
	}
}
