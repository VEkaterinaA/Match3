using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.MonoBehaviours;
using System;
using UnityEngine;
using VContainer;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens.Core
{
	internal abstract class ScreenBehaviour : MonoBehaviour, IInjectable, IScreen
	{
		Boolean IScreen.IsShowed => gameObject.activeSelf;

		protected IScreensService ScreensService { get; private set; }

		[Inject]
		internal void Construct(IScreensService screensService)
		{
			ScreensService = screensService;

			screensService.Add(this);
		}

		void IScreen.Initialize()
		{
			Initialize();
		}

		void IScreen.Show(Action completionAction)
		{
			Show();

			completionAction?.Invoke();
		}

		void IScreen.ShowInstantly()
		{
			Show();
		}

		void IScreen.Hide(Action completionAction)
		{
			Hide();

			completionAction?.Invoke();
		}

		void IScreen.HideInstantly()
		{
			Hide();
		}

		protected virtual void Initialize()
		{
		}

		protected virtual void Show()
		{
			gameObject.SetActive(true);

			Subscribe();
		}

		protected virtual void Hide()
		{
			gameObject.SetActive(false);

			Unsubscribe();
		}

		protected virtual void Subscribe()
		{
		}

		protected virtual void Unsubscribe()
		{
		}
	}
}