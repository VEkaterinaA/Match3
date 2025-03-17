using Runtime.Data.Constants.Enums;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Cinemachine;
using Runtime.SerializableStructs;

namespace Runtime.Infrastructure.Services.Core
{
	internal interface ICamerasService
	{
		internal VirtualCameraSetupType ActiveVirtualCameraSetupType { get; }

		internal CinemachineCamera CinemachineVirtualCamera { get; }

		internal Camera UIVirtualCamera { get; }

		internal Boolean CameraIsAttached { get; set; }

		internal void Setup(Dictionary<VirtualCameraSetupType, VirtualCameraSetup> virtualCamerasSetups, CinemachineConfiner2D cinemachineConfiner2D, CinemachineCamera cinemachineVirtualCamera, CinemachineBrain cinemachineBrain, Camera uiVirtualCamera, Camera camera);

		internal Vector3 ScreenToWorldPoint(Vector3 inputServicePointerPosition);

		internal void ApplySetupInstantly(VirtualCameraSetupType virtualCameraSetupType);

		internal void TakeScreenshot(Action<Texture2D> completionAction);

		internal Vector3 WorldToScreenPoint(Vector3 transformPosition);

		internal void SetUIVirtualCameraEnable(Boolean isActive);

		internal Ray ScreenPointToRay(Vector3 screenPoint);
	}
}
