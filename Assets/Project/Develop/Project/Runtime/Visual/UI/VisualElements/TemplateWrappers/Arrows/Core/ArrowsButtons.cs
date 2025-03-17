using Runtime.Visual.UI.VisualElements.Core;
using System;
using UnityEngine.UIElements;

namespace Runtime.Visual.UI.VisualElements.TemplateWrappers.Arrows.Core
{
	internal abstract class ArrowsButtons : VisualElementWrapper, IDisposable
	{
		protected Button RightButton { get; }

		protected Button LeftButton { get; }

		protected Label Label { get; }

		protected ArrowsButtons(VisualElement rootVisualElement) : base(rootVisualElement)
		{
			RightButton = rootVisualElement.Q<Button>(nameof(RightButton));
			LeftButton = rootVisualElement.Q<Button>(nameof(LeftButton));
			Label = rootVisualElement.Q<Label>(nameof(Label));
		}

		void IDisposable.Dispose()
		{
			Unsubscribe();
		}

		protected abstract void HandleRightButtonClick();

		protected abstract void HandleLeftButtonClick();

		protected virtual void Subscribe()
		{
			RightButton.clicked += HandleRightButtonClick;
			LeftButton.clicked += HandleLeftButtonClick;
		}

		protected virtual void Unsubscribe()
		{
			RightButton.clicked -= HandleRightButtonClick;
			LeftButton.clicked -= HandleLeftButtonClick;
		}
	}
}