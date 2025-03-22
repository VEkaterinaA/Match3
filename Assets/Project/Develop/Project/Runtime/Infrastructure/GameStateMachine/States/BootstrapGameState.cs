using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.GameStateMachine.States.Core;

namespace Runtime.Infrastructure.GameStateMachine.States
{
	internal sealed class BootstrapGameState : GameState
	{
		protected override void Enter()
		{
			base.Enter();

			GameStateMachine.Get<LoadingGameState>().SceneName = SceneName.MainMenuSceneAsset;
			GameStateMachine.Enter<LoadingGameState>();
		}
	}
}