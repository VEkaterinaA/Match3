using UnityEngine.UIElements;

namespace Runtime.Visual.UI.VisualElements.TemplateWrappers.Core
{
	internal abstract class TemplateWrapper
	{
		internal IStyle Style => TemplateContainer.style;

		internal TemplateContainer TemplateContainer { get; }

		protected TemplateWrapper(TemplateContainer templateContainer)
		{
			TemplateContainer = templateContainer;
		}
	}
}