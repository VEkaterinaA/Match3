using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;

namespace FlyingBears.Runtime.Infrastructure.Factories.PrefabsAssets.Core
{
	internal interface IPrefabsFactory<TEnum, TType> : IObjectResolver where TEnum : Enum where TType : UnityEngine.Object
	{
		internal UniTask<TType> Create(TEnum prefabType, Transform parentTransform);

		internal UniTask<GameObject> CreateGameObjectAsync(TEnum prefabType, Transform parentTransform);

		internal UniTask<GameObject> CreateASingleInstanceOfGameObjectAsync(TEnum prefabType, Transform parentTransform);

		internal UniTask ReleaseAsset(TEnum prefabType);

		internal void ReleaseInstanceAsset(TEnum prefabType);
	}
}