using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.GameStateMachine.States.Core;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

namespace Runtime.Infrastructure.GameStateMachine.States
{
	internal sealed class LoadingGameState : GameState
	{
		private ISceneLoadService _sceneLoadService;
		private IScreensService _screensService;

		private AsyncOperationHandle _asyncOperation;

		internal SceneName SceneName { get; set; }

		[Inject]
		internal void Construct(ISceneLoadService sceneLoadService, IScreensService screensService)
		{
			_sceneLoadService = sceneLoadService;
			_screensService = screensService;
		}

		protected override void Enter()
		{
			base.Enter();

			IsBlocksTransitions = true;

			_asyncOperation = _sceneLoadService.LoadSceneAsync(SceneName);

			_screensService.Get<LoadingScreen>().VisualizeAsyncOperation(_asyncOperation, HandleCompletion);
			_screensService.Show<LoadingScreen>();

			return;

			void HandleCompletion()
			{
				IsBlocksTransitions = false;

				GameStateMachine.Enter<LoopsGameState>();
			}
		}

		protected override void Exit()
		{
			base.Exit();

			_screensService.Hide<LoadingScreen>();
		}
	}
}