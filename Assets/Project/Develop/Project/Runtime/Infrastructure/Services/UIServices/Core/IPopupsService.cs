using Runtime.Visual.UI.UIDocumentWrappers.Popups.Core;
using System;

namespace Runtime.Infrastructure.Services.UIServices.Core
{
	internal interface IPopupsService
	{
		internal void Show<TPopup>(Action completionAction) where TPopup : Popup;

		internal void Show<TPopup>() where TPopup : Popup;

		internal void Show(Type type, Action completionAction = null);

		internal void Hide<TPopup>(Action completionAction) where TPopup : Popup;

		internal void Hide<TPopup>() where TPopup : Popup;

		internal void Hide(Type type, Action completionAction = null);

		internal TPopup Get<TPopup>() where TPopup : Popup;

		internal Popup Get(Type popupType);
	}
}
