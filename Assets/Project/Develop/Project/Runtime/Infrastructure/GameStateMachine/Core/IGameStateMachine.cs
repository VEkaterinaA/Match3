using Runtime.Infrastructure.GameStateMachine.States.Core;

namespace Runtime.Infrastructure.GameStateMachine.Core
{
	internal interface IGameStateMachine
	{
		internal void Enter<TState>() where TState : IGameState;

		internal TState Get<TState>() where TState : class, IGameState;
	}
}
