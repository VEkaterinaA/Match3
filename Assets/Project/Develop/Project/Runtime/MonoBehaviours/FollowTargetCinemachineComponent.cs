using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Runtime.MonoBehaviours
{
	[AddComponentMenu("")]
	[SaveDuringPlay]
	internal sealed class FollowTargetCinemachineComponent : CinemachineComponentBase
	{
		private Boolean _isForcePositionAndRotation;
		private Vector3 _previousTargetPosition;
		[SerializeField]
		private Single _damping;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		public override Boolean IsValid => (enabled && (FollowTarget != null));

		internal Single Damping
		{
			set => _damping = value;
		}

		public override Single GetMaxDampTime()
		{
			return _damping;
		}

		public override void MutateCameraState(ref CameraState cameraState, Single deltaTime)
		{
			if (!IsValid)
			{
				return;
			}

			var targetPosition = FollowTargetPosition;

			if ((!_isForcePositionAndRotation) && (deltaTime >= 0.0F))
			{
				targetPosition = (_previousTargetPosition + VirtualCamera.DetachedFollowTargetDamp((targetPosition - _previousTargetPosition), _damping, deltaTime));
			}

			_previousTargetPosition = targetPosition;

			cameraState.RawPosition = targetPosition;

			_isForcePositionAndRotation = false;
		}

		internal void ForceLocalPosition(Vector3 localPosition)
		{
			if (!IsValid)
			{
				return;
			}

			FollowTarget.transform.localPosition = localPosition;

			_previousTargetPosition = FollowTargetPosition;
			_isForcePositionAndRotation = true;
		}
	}
}
