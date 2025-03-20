using LitMotion;
using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens.Core
{
	internal abstract class Screen : UIDocumentWrapper, IScreen
	{
		private readonly List<TextElement> _textElements;

		private MotionHandle _fadeMotionHandle;
		private const Single _fadeDuration = 0.35f;

		Boolean IScreen.IsShowed => IsShowed;

		protected Vector2Int ReferenceResolution => UIDocument.panelSettings.referenceResolution;

		protected VisualElement RootVisualElement => UIDocument.rootVisualElement;

		protected IStyle RootStyle => RootVisualElement.style;

		protected IScreensService ScreensService { get; private set; }

		internal Boolean IsShowed { get; private protected set; }


		protected Screen(UIDocument uiDocument) : base(uiDocument)
		{
			_textElements = RootVisualElement.Query<TextElement>().ToList();
		}

		[Inject]
		internal void Construct(IScreensService screensService)
		{
			ScreensService = screensService;
		}

		void IScreen.Initialize()
		{
			Initialize();
		}

		void IScreen.ShowInstantly()
		{
			RootStyle.visibility = Visibility.Visible;
			RootStyle.opacity = 1.0F;

			Show();
		}

		void IScreen.Show(Action completionAction)
		{
			RootStyle.visibility = Visibility.Visible;

			var builder = RootStyle.CreateMotion(RootStyle.opacity.value, 1.0F, _fadeDuration);
			builder.InvokeAfterCompletion(completionAction);

			_fadeMotionHandle.CancelIfActive();
			_fadeMotionHandle = builder.BindToOpacity();

			Show();
		}

		void IScreen.HideInstantly()
		{
			RootStyle.visibility = Visibility.Hidden;

			Hide();
		}

		void IScreen.Hide(Action completionAction)
		{
			var builder = RootStyle.CreateMotion(RootStyle.opacity.value, 0.0F, _fadeDuration);
			builder.InvokeAfterCompletion(() => RootStyle.visibility = Visibility.Hidden);
			builder.InvokeAfterCompletion(completionAction);

			_fadeMotionHandle.CancelIfActive();
			_fadeMotionHandle = builder.BindToOpacity();

			Hide();
		}

		protected virtual void Initialize()
		{
			RootStyle.visibility = Visibility.Hidden;
		}


		protected virtual void Show()
		{
			IsShowed = true;
		}

		protected virtual void Hide()
		{
			IsShowed = false;
		}

		protected virtual void Subscribe()
		{
		}

		protected virtual void Unsubscribe()
		{
		}
	}
}