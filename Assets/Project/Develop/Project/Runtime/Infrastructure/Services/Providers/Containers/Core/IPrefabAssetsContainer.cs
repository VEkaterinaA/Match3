using UnityEngine;

namespace Runtime.Infrastructure.Services.Providers.Containers.Core
{
	internal interface IPrefabAssetsContainer<TEnum, TType> : IAsyncAssetsContainer<TEnum, TType> where TType : Object
	{
		internal TType this[TEnum prefabType] { get; }
	}
}