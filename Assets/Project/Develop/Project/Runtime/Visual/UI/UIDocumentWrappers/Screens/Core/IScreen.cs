using System;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens.Core
{
	internal interface IScreen
	{
		internal Boolean IsShowed { get; }

		internal void Initialize();

		internal void Show(Action completionAction);

		internal void Hide(Action completionAction);

		internal void ShowInstantly();

		internal void HideInstantly();
	}
}