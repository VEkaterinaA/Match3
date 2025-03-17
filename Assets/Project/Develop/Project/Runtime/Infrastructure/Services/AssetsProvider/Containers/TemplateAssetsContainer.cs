using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Services.AssetsProvider.Containers.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace Runtime.Infrastructure.Services.AssetsProvider.Containers
{
	[Serializable]
	internal sealed class TemplateAssetsContainer<TEnum, TType> : ITemplateAssetsContainer<TEnum>
	{
		[SerializeField]
		private SerializedDictionary<TEnum, AssetReferenceT<VisualTreeAsset>> _visualTreeAssetsReferencesDictionary;

		private Dictionary<TEnum, AsyncOperationHandle<VisualTreeAsset>> _visualTreeAssetsHandlesDictionary;

		VisualTreeAsset ITemplateAssetsContainer<TEnum>.this[TEnum templateID] => _visualTreeAssetsHandlesDictionary[templateID].Result;

		async UniTask IAssetsContainer<TEnum>.LoadAssetsAsync()
		{
			var assetsCount = _visualTreeAssetsReferencesDictionary.Count;

			_visualTreeAssetsHandlesDictionary = new Dictionary<TEnum, AsyncOperationHandle<VisualTreeAsset>>(assetsCount);
			var uniTasks = new List<UniTask>(assetsCount);

			foreach (var (templateType, assetReference) in _visualTreeAssetsReferencesDictionary)
			{
				var asyncOperationHandle = assetReference.LoadAssetAsync();

				_visualTreeAssetsHandlesDictionary.Add(templateType, asyncOperationHandle);

				uniTasks.Add(asyncOperationHandle.ToUniTask());
			}

			await UniTask.WhenAll(uniTasks);
		}
	}
}