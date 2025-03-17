using UnityEngine;

namespace Runtime.Infrastructure.Services.AssetsProvider.Containers.Core
{
	internal interface IPrefabAssetsContainer<TEnum, TType> : IAsyncAssetsContainer<TEnum, TType> where TType : Object
	{
		internal TType this[TEnum prefabType] { get; }
	}
}