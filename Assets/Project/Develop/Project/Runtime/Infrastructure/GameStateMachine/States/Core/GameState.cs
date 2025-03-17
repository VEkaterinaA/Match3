using Runtime.Infrastructure.GameStateMachine.Core;
using System;
using VContainer;

namespace Runtime.Infrastructure.GameStateMachine.States.Core
{
	internal abstract class GameState : IGameState
	{
		protected Boolean IsBlocksTransitions;

		protected IGameStateMachine GameStateMachine { get; private set; }

		[Inject]
		internal void Construct(IGameStateMachine gameStateMachine)
		{
			GameStateMachine = gameStateMachine;
		}

		Boolean IGameState.IsBlocksTransitions => IsBlocksTransitions;

		void IGameState.Enter()
		{
			Enter();
		}

		void IGameState.Exit()
		{
			Exit();
		}

		protected virtual void Enter()
		{
		}

		protected virtual void Exit()
		{
		}
	}

}
