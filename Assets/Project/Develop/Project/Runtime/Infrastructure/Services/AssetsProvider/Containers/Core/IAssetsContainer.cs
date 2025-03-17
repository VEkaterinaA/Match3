using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Infrastructure.Services.AssetsProvider.Containers.Core
{
	internal interface IAssetsContainer<TEnum>
	{
		internal UniTask LoadAssetsAsync();

	}

	internal interface IAsyncAssetsContainer<TEnum,TType> : IAssetsContainer<TEnum> where TType : Object
	{
		internal void ReleaseAsset(TEnum prefabType);

		internal void ReleaseInstanceAsset(TEnum prefabType);

		internal UniTask<TType> LoadAsyncObject(TEnum prefabType);

		internal UniTask<GameObject> InstantiateGameObjectAsync(TEnum prefabType, Transform parent);

		internal UniTask<GameObject> SingleInstantiateGameObjectAsync(TEnum prefabType, Transform parent);
	}
}