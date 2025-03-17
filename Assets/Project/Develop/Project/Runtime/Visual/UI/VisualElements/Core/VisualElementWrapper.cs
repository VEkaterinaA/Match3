using UnityEngine.UIElements;

namespace Runtime.Visual.UI.VisualElements.Core
{
	internal abstract class VisualElementWrapper
	{
		internal IStyle Style => RootVisualElement.style;

		protected VisualElement RootVisualElement { get; }

		internal VisualElementWrapper(VisualElement rootVisualElement)
		{
			RootVisualElement = rootVisualElement;
		}
	}
}