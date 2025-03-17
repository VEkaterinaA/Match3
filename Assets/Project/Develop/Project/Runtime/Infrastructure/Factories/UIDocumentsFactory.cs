using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Factories.Core;
using Runtime.Infrastructure.Services.AssetsProvider.Containers.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Object = System.Object;

namespace Runtime.Infrastructure.Factories
{
	internal class UIDocumentsFactory<TEnum> : IUIDocumentsFactory<TEnum>, IInitializationInformer where TEnum : Enum
	{
		private IUIDocumentInfoAssetsContainer<TEnum> _uiDocumentInfoAssetsContainer;
		private IObjectResolver _objectResolver;

		private Action _initialized;

		private Boolean _isInitialized;

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		private IUIDocumentsFactory<TEnum> Factory => this;

		[Inject]
		internal async void Construct(IUIDocumentInfoAssetsContainer<TEnum> uiDocumentInfoAssetsContainer, IObjectResolver objectResolver)
		{
			_uiDocumentInfoAssetsContainer = uiDocumentInfoAssetsContainer;
			_objectResolver = objectResolver;

			await uiDocumentInfoAssetsContainer.LoadAssetsAsync();

			_isInitialized = true;
			_initialized?.Invoke();
		}

		UIDocumentWrapper IUIDocumentsFactory<TEnum>.Create(TEnum uiDocumentType, Transform parentTransform)
		{
			var gameObject = new GameObject(uiDocumentType.ToString());
			gameObject.transform.parent = parentTransform;

			var uiDocument = gameObject.AddComponent<UIDocument>();
			_uiDocumentInfoAssetsContainer[uiDocumentType].ApplyTo(uiDocument);

			var uiDocumentWrapper = Create(uiDocumentType, uiDocument);

			_objectResolver.Inject(uiDocumentWrapper);

			return uiDocumentWrapper;
		}

		List<UIDocumentWrapper> IUIDocumentsFactory<TEnum>.CreateAll(Transform parentTransform)
		{
			var assetsTypes = _uiDocumentInfoAssetsContainer.AssetsTypes;

			var uiDocumentWrappers = new List<UIDocumentWrapper>(assetsTypes.Count);

			foreach (var assetsType in assetsTypes)
			{
				uiDocumentWrappers.Add(Factory.Create(assetsType, parentTransform));
			}

			return uiDocumentWrappers;
		}

		void IUIDocumentsFactory<TEnum>.Inject(Object systemObject)
		{
			_objectResolver.Inject(systemObject);
		}

		private UIDocumentWrapper Create(TEnum uiDocumentType, UIDocument uiDocument)
		{
			return uiDocumentType switch
			{
				_ => throw new NotImplementedException($"[{GetType().Name}] There is no implementation to create {typeof(TEnum)}."),
			};
		}
	}

}
