using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace Runtime.Infrastructure.Services.AssetsProvider.Assets
{
	[Serializable]
	internal sealed class UIDocumentInfo
    {
		[SerializeField]
		private AssetReferenceT<VisualTreeAsset> _visualTreeAssetReference;
		[SerializeField]
		private AssetReferenceT<PanelSettings> _panelSettingsReference;

		private AsyncOperationHandle<VisualTreeAsset> _visualTreeAssetHandle;
		private AsyncOperationHandle<PanelSettings> _panelSettingsHandle;

		[SerializeField]
		private Single _sortingOrder;

		internal async UniTask Load()
		{
			_visualTreeAssetHandle = _visualTreeAssetReference.LoadAssetAsync();
			_panelSettingsHandle = _panelSettingsReference.LoadAssetAsync();

			await UniTask.WhenAll(_visualTreeAssetHandle.ToUniTask(), _panelSettingsHandle.ToUniTask());
		}

		internal void ApplyTo(UIDocument uiDocument)
		{
			uiDocument.visualTreeAsset = _visualTreeAssetHandle.Result;
			uiDocument.panelSettings = _panelSettingsHandle.Result;
			uiDocument.sortingOrder = _sortingOrder;
		}

		internal void Release()
		{
			_visualTreeAssetReference.ReleaseAsset();
			_panelSettingsReference.ReleaseAsset();
		}
	}
}