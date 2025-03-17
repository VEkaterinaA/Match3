using Runtime.Visual.UI.UIDocumentWrappers.Screens.Core;
using System;
using UnityEngine;

namespace Runtime.Infrastructure.Services.UIServices.Core
{
	internal interface IScreensService
	{
		internal Vector2 GetScreenCenter();

		internal Vector2 GetScreenSize();

		internal void Add(IScreen screen);

		internal void Show<TScreen>(Action completionAction = null) where TScreen : class, IScreen;

		internal void ShowInstantly(Type screenType);

		internal void Hide<TScreen>(Action completionAction) where TScreen : class, IScreen;

		internal void Hide<TScreen>() where TScreen : class, IScreen;

		internal TScreen Get<TScreen>() where TScreen : class, IScreen;

		internal IScreen Get(Type screenType);
	}
}
