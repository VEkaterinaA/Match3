﻿using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Infrastructure.Services.Localization.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Core;
using LitMotion;
using System;
using System.Collections.Generic;
using System.Linq;
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

		protected ILocalizationService LocalizationService { get; private set; }

		protected IScreensService ScreensService { get; private set; }

		internal Boolean IsShowed { get; private protected set; }


		protected Screen(UIDocument uiDocument) : base(uiDocument)
		{
			_textElements = RootVisualElement.Query<TextElement>().ToList();
		}

		[Inject]
		internal void Construct(IScreensService screensService, ILocalizationService localizationService)
		{
			LocalizationService = localizationService;
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

		protected virtual void UpdateLocalization()
		{
			foreach (var localizedTextElement in _textElements.Where(localizedTextElement => (localizedTextElement.text.Length > 0)))
			{
				LocalizationService.GetLocalizedString($"{GetType().Name}/{localizedTextElement.name}", (localizedText => localizedTextElement.text = localizedText));
			}
		}

		protected virtual void Show()
		{
			UpdateLocalization();
			Subscribe();

			IsShowed = true;
		}

		protected virtual void Hide()
		{
			Unsubscribe();

			IsShowed = false;
		}

		protected virtual void Subscribe()
		{
			LocalizationService.LocaleChanged += UpdateLocalization;
		}

		protected virtual void Unsubscribe()
		{
			LocalizationService.LocaleChanged -= UpdateLocalization;
		}
	}
}