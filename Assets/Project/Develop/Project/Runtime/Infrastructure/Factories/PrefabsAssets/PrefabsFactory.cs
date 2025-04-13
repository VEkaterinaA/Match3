using Cysharp.Threading.Tasks;
using FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets.Core;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Services.AssetsProvider.Containers.Core;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets
{
	internal sealed class PrefabsFactory<TEnum, TType> : ObjectResolver, IInitializationInformer, IInitializable, IPrefabsFactory<TEnum, TType> where TEnum : Enum where TType : UnityEngine.Object
	{
		private IAsyncAssetsContainer<TEnum, TType> _prefabAssetsContainer;

		private Action _initialized;

		private Boolean _isInitialized;

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		[Inject]
		internal void Construct(IAsyncAssetsContainer<TEnum, TType> prefabAssetsContainer)
		{
			_prefabAssetsContainer = prefabAssetsContainer;
		}

		void IInitializable.Initialize()
		{
			_isInitialized = true;
			_initialized?.Invoke();
		}

		async UniTask<TType> IPrefabsFactory<TEnum, TType>.Create(TEnum prefabType, Transform parentTransform)
		{
			var prefab = await _prefabAssetsContainer.LoadAsyncObject(prefabType);
			return Object.Instantiate(prefab, parentTransform);
		}

		async UniTask<GameObject> IPrefabsFactory<TEnum, TType>.CreateGameObjectAsync(TEnum prefabType, Transform parentTransform)
		{
			return await _prefabAssetsContainer.InstantiateGameObjectAsync(prefabType, parentTransform);
		}

		async UniTask<GameObject> IPrefabsFactory<TEnum, TType>.CreateASingleInstanceOfGameObjectAsync(TEnum prefabType, Transform parentTransform)
		{
			return await _prefabAssetsContainer.SingleInstantiateGameObjectAsync(prefabType, parentTransform);
		}

		async UniTask IPrefabsFactory<TEnum, TType>.ReleaseAsset(TEnum prefabType)
		{
			var prefab = await _prefabAssetsContainer.LoadAsyncObject(prefabType);
			Object.Destroy(prefab);
			_prefabAssetsContainer.ReleaseAsset(prefabType);
		}

		void IPrefabsFactory<TEnum, TType>.ReleaseInstanceAsset(TEnum prefabType)
		{
			_prefabAssetsContainer.ReleaseInstanceAsset(prefabType);
		}
	}
}