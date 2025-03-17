using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Services.AssetsProvider.Containers.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Infrastructure.Services.AssetsProvider.Containers
{
	[Serializable]
	internal sealed class PrefabAssetsContainer<TEnum, TType> : IDisposable, IAsyncAssetsContainer<TEnum, TType> where TType : UnityEngine.Object
	{
		[SerializeField]
		private SerializedDictionary<TEnum, AssetReferenceT<TType>> _prefabsAssetsReferencesDictionary;

		private readonly Dictionary<TEnum, AsyncOperationHandle> _handles = new();

		private IAsyncAssetsContainer<TEnum, TType> Service => this;

		async UniTask<TType> IAsyncAssetsContainer<TEnum, TType>.LoadAsyncObject(TEnum prefabType)
		{
			var assetReference = _prefabsAssetsReferencesDictionary[prefabType];

			var handle = Addressables.LoadAssetAsync<TType>(assetReference);

			await handle.ToUniTask();

			if (handle.Status != AsyncOperationStatus.Succeeded)
			{
#if UNITY_EDITOR || DEBUG
				Debug.LogError($"{handle.OperationException}");
#endif
				Addressables.Release(handle);
				return default;
			}

			var asset = handle.Result;

			if (asset == null)
			{
#if UNITY_EDITOR || DEBUG
				Debug.LogError($"asset {assetReference.AssetGUID} is null");
#endif
				return default;
			}

			_handles[prefabType] = handle;

			return asset;
		}

		async UniTask<GameObject> IAsyncAssetsContainer<TEnum, TType>.InstantiateGameObjectAsync(TEnum prefabType, Transform parent)
		{
			if (!AssetExists(prefabType))
			{
				Debug.LogError($"asset {prefabType} is null");
				return null;
			}

			var handle = Addressables.InstantiateAsync(_prefabsAssetsReferencesDictionary[prefabType], parent);

			await handle.ToUniTask();

			if (handle.Status != AsyncOperationStatus.Succeeded)
			{
#if UNITY_EDITOR || DEBUG
				Debug.LogError($"{handle.OperationException}");
#endif
				Addressables.Release(handle);
				return default;
			}

			var asset = handle.Result;

			if (asset == null)
			{
#if UNITY_EDITOR || DEBUG
				Debug.LogError($"asset {prefabType} is null");
#endif
				return null;
			}

			_handles[prefabType] = handle;

			return asset;
		}

		async UniTask<GameObject> IAsyncAssetsContainer<TEnum, TType>.SingleInstantiateGameObjectAsync(TEnum prefabType, Transform parent)
		{
			if (_handles.TryGetValue(prefabType, out var existingHandle))
			{
				var existingAsset = existingHandle.Result as GameObject;
				if (existingAsset != null)
				{
					return existingAsset;
				}
			}

			return await Service.InstantiateGameObjectAsync(prefabType, parent);
		}

		void IAsyncAssetsContainer<TEnum, TType>.ReleaseAsset(TEnum prefabType)
		{
			if (_handles.TryGetValue(prefabType, out var handle))
			{
				Addressables.Release(handle);
				_handles.Remove(prefabType);
			}
		}

		void IAsyncAssetsContainer<TEnum, TType>.ReleaseInstanceAsset(TEnum prefabType)
		{
			if (_handles.TryGetValue(prefabType, out var handle))
			{
				Addressables.ReleaseInstance(handle);
				_handles.Remove(prefabType);
			}
		}

		async UniTask IAssetsContainer<TEnum>.LoadAssetsAsync()
		{
			var assetsCount = _prefabsAssetsReferencesDictionary.Count;

			var uniTasks = new List<UniTask>(assetsCount);

			foreach (var (prefabType, assetReference) in _prefabsAssetsReferencesDictionary)
			{
				var asyncOperationHandle = assetReference.LoadAssetAsync();

				_handles.Add(prefabType, asyncOperationHandle);

				uniTasks.Add(asyncOperationHandle.ToUniTask());
			}

			await UniTask.WhenAll(uniTasks);
		}

		void IDisposable.Dispose()
		{
			foreach (var handle in _handles)
			{
				Addressables.Release(handle);
			}
		}

		private bool AssetExists(TEnum prefabType)
		{
			if (!_prefabsAssetsReferencesDictionary.TryGetValue(prefabType, out var assetReference) || assetReference == null)
			{
				return false;
			}

			return true;
		}
	}
}