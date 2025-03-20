using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using System;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class GameScreen : Screen
	{
		private IPersistentProgressService _persistentProgressService;
		private ILoopsService _loopsService;

		private String _activeSubtitlesID;

		internal GameScreen(UIDocument uiDocument) : base(uiDocument)
		{
		}

		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, ILoopsService loopsService)
		{
			_persistentProgressService = persistentProgressService;
			_loopsService = loopsService;
		}

	}
}