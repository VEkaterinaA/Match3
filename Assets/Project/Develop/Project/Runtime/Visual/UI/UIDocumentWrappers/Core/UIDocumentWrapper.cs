using UnityEngine.UIElements;

namespace Runtime.Visual.UI.UIDocumentWrappers.Core
{
	internal abstract class UIDocumentWrapper
	{
		protected UIDocument UIDocument { get; }

		protected UIDocumentWrapper(UIDocument uiDocument)
		{
			UIDocument = uiDocument;
		}
	}
}