using System;

namespace Runtime.Infrastructure.GameStateMachine.States.Core
{
	internal interface IGameState
	{
		internal Boolean IsBlocksTransitions { get; }

		internal void Enter();

		internal void Exit();
	}
}
