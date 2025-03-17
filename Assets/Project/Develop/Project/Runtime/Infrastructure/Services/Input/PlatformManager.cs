using System;
using Runtime.MonoBehaviours;
using UnityEngine;
using VContainer;

namespace Runtime.Infrastructure.Services.Input
{
	internal sealed class PlatformManager : MonoBehaviour, IPlatformManager
	{
		[SerializeField]
		private Boolean _isMobilePlatformInEditor;

		private Boolean _isMobilePlatform;

		Boolean IPlatformManager.IsMobilePlatform => _isMobilePlatform;

		[Inject]
		private void Construct()
		{
#if UNITY_EDITOR
			_isMobilePlatform = _isMobilePlatformInEditor;
#else
			_isMobilePlatform = Application.isMobilePlatform;
#endif
		}
	}

	internal interface IPlatformManager
	{
		internal Boolean IsMobilePlatform { get;}
	}
}