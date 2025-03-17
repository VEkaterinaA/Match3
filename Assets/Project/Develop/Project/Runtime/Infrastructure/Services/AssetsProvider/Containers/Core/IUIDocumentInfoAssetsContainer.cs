using System;
using System.Collections.Generic;
using Runtime.Infrastructure.Services.AssetsProvider.Assets;

namespace Runtime.Infrastructure.Services.AssetsProvider.Containers.Core
{
	internal interface IUIDocumentInfoAssetsContainer<TEnum> : IAssetsContainer<TEnum> where TEnum : Enum
	{
		internal Dictionary<TEnum, UIDocumentInfo>.KeyCollection AssetsTypes { get; }

		internal UIDocumentInfo this[TEnum prefabType] { get; }
	}
}