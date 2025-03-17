using Runtime.Visual.UI.UIDocumentWrappers.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Runtime.Infrastructure.Factories.Core
{
	internal interface IUIDocumentsFactory<in TEnum> where TEnum : Enum
	{
		internal UIDocumentWrapper Create(TEnum uiDocumentType, Transform parentTransform);

		internal List<UIDocumentWrapper> CreateAll(Transform parentTransform);

		internal void Inject(Object systemObject);
	}
}
