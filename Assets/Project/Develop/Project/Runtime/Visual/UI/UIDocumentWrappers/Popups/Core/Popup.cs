using LitMotion;
using Runtime.Data.Constants.Strings;
using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Visual.UI.UIDocumentWrappers.Popups.Core
{
	internal abstract class Popup : UIDocumentWrapper
	{
		protected readonly CompositeMotionHandle CompositeMotionHandle = new CompositeMotionHandle();

		protected Single MotionDuration = 0.2f;

		private const Ease _ease = Ease.OutCirc;

		internal event Action Hided;

		protected VisualElement RootVisualElement => UIDocument.rootVisualElement;

		internal Boolean CanOverlapOtherPopups { get; }

		internal Boolean IsShowed { get; private set; }

		protected IStyle RootStyle => RootVisualElement.style;

		protected VisualElement PanelVisualElement { get; }

		protected List<Button> CloseButtons { get; }

		protected IPopupsService PopupsService { get; private set; }

		protected Popup(UIDocument uiDocument, Boolean canOverlapOtherPopups) : base(uiDocument)
		{
			PanelVisualElement = RootVisualElement.Q<VisualElement>(null, ClassName.PopupPanel);

			RootVisualElement.StretchToParentSize();
			CanOverlapOtherPopups = canOverlapOtherPopups;

			CloseButtons = RootVisualElement.Query<Button>(null, ClassName.CloseButton).ToList();

			PanelVisualElement.style.scale = Vector2.zero;
			RootStyle.display = DisplayStyle.None;
		}

		internal void Construct(IPopupsService popupsService)
		{
			PopupsService = popupsService;
		}

		internal void ShowInstantly()
		{
			CompositeMotionHandle.Cancel();

			Show();
		}

		internal virtual void Show(Action completionAction)
		{
			CompositeMotionHandle.Cancel();

			var builder = PanelVisualElement.style.CreateMotion((Vector2) PanelVisualElement.style.scale.value.value, Vector2.one, MotionDuration);
			builder.InvokeAfterCompletion(completionAction);
			builder.InvokeAfterCompletion(Hided);
			builder.AddEase(_ease);

			builder.BindToScale().AddTo(CompositeMotionHandle);

			Show();
		}

		internal void HideInstantly()
		{
			Hide();

			Hided?.Invoke();
		}

		internal virtual void Hide(Action completionAction)
		{
			CompositeMotionHandle.Cancel();

			var builder = PanelVisualElement.style.CreateMotion((Vector2) PanelVisualElement.style.scale.value.value, Vector2.zero, MotionDuration);
			builder.InvokeAfterCompletion(() => RootStyle.display = DisplayStyle.None);
			builder.InvokeAfterCompletion(completionAction);
			builder.InvokeAfterCompletion(Hided);
			builder.AddEase(_ease);

			builder.BindToScale().AddTo(CompositeMotionHandle);

			Hide();
		}

		protected Action GetHidedAction()
		{
			return Hided;
		}

		protected virtual void Subscribe()
		{
			foreach (var closeButton in CloseButtons)
			{
				closeButton.clicked += HideByPopupService;
			}
		}

		protected virtual void Unsubscribe()
		{
			foreach (var closeButton in CloseButtons)
			{
				closeButton.clicked -= HideByPopupService;
			}
		}

		protected virtual void Show()
		{
			RootVisualElement.style.display = DisplayStyle.Flex;
			IsShowed = true;

			Subscribe();
		}

		protected virtual void Hide()
		{
			IsShowed = false;

			Unsubscribe();
		}

		/*		protected virtual void BeforeDestruction()
				{
					IsShowed = false;
					Unsubscribe();

					CompositeMotionHandle.Cancel();
				}*/

		private void HideByPopupService()
		{
			PopupsService.Hide(GetType());
		}

		/*		public void Dispose()
				{
					BeforeDestruction();
				}*/
	}
}