using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Services.AssetsProvider.Assets;
using Runtime.Infrastructure.Services.AssetsProvider.Containers.Core;
using UnityEngine;

namespace Runtime.Infrastructure.Services.AssetsProvider.Containers
{
	[Serializable]
	internal sealed class UIDocumentInfoAssetsContainer<TEnum> : IUIDocumentInfoAssetsContainer<TEnum> where TEnum : Enum
	{
		[SerializeField]
		private SerializedDictionary<TEnum, UIDocumentInfo> _uiDocumentsInfosDictionary;

		Dictionary<TEnum, UIDocumentInfo>.KeyCollection IUIDocumentInfoAssetsContainer<TEnum>.AssetsTypes => _uiDocumentsInfosDictionary.Keys;

		UIDocumentInfo IUIDocumentInfoAssetsContainer<TEnum>.this[TEnum uiDocumentType] => _uiDocumentsInfosDictionary[uiDocumentType];

		async UniTask IAssetsContainer<TEnum>.LoadAssetsAsync()
		{
			var assetsCount = _uiDocumentsInfosDictionary.Count;

			var uniTasks = new List<UniTask>(assetsCount);

			foreach (var screenInfo in _uiDocumentsInfosDictionary.Values)
			{
				uniTasks.Add(screenInfo.Load());
			}

			await UniTask.WhenAll(uniTasks);
		}
	}
}