using Cysharp.Threading.Tasks;
using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Services.Core;
using Runtime.MonoBehaviours;
using Runtime.SerializableStructs;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Runtime.Infrastructure.Services
{
	internal sealed class CamerasService : ICamerasService, IInitializationInformer
	{

		private Dictionary<VirtualCameraSetupType, VirtualCameraSetup> _virtualCamerasSetups;
		private FollowTargetCinemachineComponent _followTargetCinemachineComponent;
		private CinemachineCamera _cinemachineVirtualCamera;
		private CinemachineConfiner2D _cinemachineConfiner2D;
		private readonly RenderTexture _activeRenderTexture;
		private CinemachineBrain _cinemachineBrain;
		private Camera _uiVirtualCamera;
		private Camera _camera;

		private Action _virtualCameraUpdated;
		private Action _initialized;

		private Boolean _isInitialized;


		private VirtualCameraSetupType _activeVirtualCameraSetupType;

		VirtualCameraSetupType ICamerasService.ActiveVirtualCameraSetupType => _activeVirtualCameraSetupType;

		CinemachineCamera ICamerasService.CinemachineVirtualCamera => _cinemachineVirtualCamera;

		Camera ICamerasService.UIVirtualCamera => _uiVirtualCamera;

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		Boolean ICamerasService.CameraIsAttached
		{
			get => _followTargetCinemachineComponent.enabled;
			set
			{
				_followTargetCinemachineComponent.enabled = value;
				_cinemachineConfiner2D.enabled = value;
			}
		}

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}


		void ICamerasService.Setup(Dictionary<VirtualCameraSetupType, VirtualCameraSetup> virtualCamerasSetups, CinemachineConfiner2D cinemachineConfiner2D,
			CinemachineCamera cinemachineVirtualCamera, CinemachineBrain cinemachineBrain, Camera uiVirtualCamera, Camera camera)
		{
			_cinemachineVirtualCamera = cinemachineVirtualCamera;
			_cinemachineConfiner2D = cinemachineConfiner2D;

			_followTargetCinemachineComponent = (FollowTargetCinemachineComponent) _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

			_virtualCamerasSetups = virtualCamerasSetups;
			_cinemachineBrain = cinemachineBrain;
			_uiVirtualCamera = uiVirtualCamera;
			_camera = camera;

			_virtualCameraUpdated?.Invoke();

			_isInitialized = true;
			_initialized?.Invoke();
		}

		void ICamerasService.ApplySetupInstantly(VirtualCameraSetupType virtualCameraSetupType)
		{
			_activeVirtualCameraSetupType = virtualCameraSetupType;

			_virtualCamerasSetups[virtualCameraSetupType].ApplyInstantly(_cinemachineConfiner2D, _followTargetCinemachineComponent);
		}

		void ICamerasService.SetUIVirtualCameraEnable(Boolean isEnable)
		{
			_uiVirtualCamera.enabled = isEnable;
		}

		Ray ICamerasService.ScreenPointToRay(Vector3 screenPoint)
		{
			return _camera.ScreenPointToRay(screenPoint);
		}

		Vector3 ICamerasService.WorldToScreenPoint(Vector3 position)
		{
			return _camera.WorldToScreenPoint(position);
		}

		Vector3 ICamerasService.ScreenToWorldPoint(Vector3 position)
		{
			return _camera.ScreenToWorldPoint(position);
		}

		async void ICamerasService.TakeScreenshot(Action<Texture2D> completionAction)
		{
			await UniTask.WaitUntil(() => _cinemachineBrain);
			await UniTask.WaitForEndOfFrame();

			_camera.targetTexture = _activeRenderTexture;
			_camera.Render();
			_camera.targetTexture = null;

			var texture2D = new Texture2D(_activeRenderTexture.width, _activeRenderTexture.height, TextureFormat.ARGB32, false);

			texture2D.ReadPixels(new Rect(0, 0, _activeRenderTexture.width, _activeRenderTexture.height), 0, 0);
			texture2D.Apply();

			completionAction?.Invoke(texture2D);
		}
	}
}
