using Runtime.Infrastructure.GameStateMachine.States.Core;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using VContainer;

namespace Runtime.Infrastructure.GameStateMachine.States
{
	internal sealed class PausedGameState : GameState
	{
		private IScreensService _screensService;
		private IPauseControl _pauseControl;

		[Inject]
		internal void Construct(IScreensService screensService, IPauseControl pauseControl)
		{
			_screensService = screensService;
			_pauseControl = pauseControl;
		}

		protected override void Enter()
		{
			base.Enter();

			_screensService.Hide<InputScreen>();

			_pauseControl.RunPause();
		}

		protected override void Exit()
		{
			base.Exit();

			_screensService.Show<HUDScreen>();
			_screensService.Show<GameScreen>();
			_screensService.Show<InputScreen>();

			_pauseControl.RunUnpause();
		}
	}
}