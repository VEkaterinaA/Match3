using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.MonoBehaviours;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Runtime.MonoBehaviours.UIControllers
{
    public class CoreSceneController : InjectedBehaviour
    {
        private IGameStateMachine _gameStateMachine;
        private IScreensService _screensService;

        [SerializeField]
        private UIDocument _gameDocument;

        private Button _menuButton;

        [Inject]
        internal void Construct(IGameStateMachine gameStateMachine, IScreensService screensService)
        {
            _gameStateMachine = gameStateMachine;
            _screensService = screensService;

            Subscribe();
        }

        private void Subscribe()
        {
            var root = _gameDocument.rootVisualElement;

            _menuButton = root.Q<Button>("MenuButton");

			_menuButton.clicked += OnMainMenuButtonClicked;
        }

        private void UnSubscribe()
        {
			_menuButton.clicked -= OnMainMenuButtonClicked;
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }


        private void OnMainMenuButtonClicked()
        {
            Time.timeScale = 1f;
            _screensService.Show<PauseScreen>();
        }
    }
} 