using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VContainer.Unity;
using VContainer;

namespace Runtime.Infrastructure.Services
{
	internal sealed class GameService : IGameStateMachine, IInitializable, IGameService
	{
		private IObjectResolver _objectResolver;
		private IGameState _activeGameState;

		private readonly Dictionary<Type, IGameState> _availableGameStates = new Dictionary<Type, IGameState>()
		{
			[typeof(LoadingGameState)] = new LoadingGameState(),
			[typeof(PausedGameState)] = new PausedGameState(),
			[typeof(LoopsGameState)] = new LoopsGameState(),
			[typeof(ExitGameState)] = new ExitGameState(),
		};

		[Inject]
		internal void Inject(IObjectResolver objectResolver)
		{
			_objectResolver = objectResolver;
		}

		void IInitializable.Initialize()
		{
			foreach (var gameState in _availableGameStates.Values)
			{
				_objectResolver.Inject(gameState);
			}

			_activeGameState = new BootstrapGameState();
			_objectResolver.Inject(_activeGameState);

			_activeGameState.Enter();
		}

		async void IGameStateMachine.Enter<TState>()
		{
			await UniTask.WaitWhile(() => _activeGameState.IsBlocksTransitions);

			_activeGameState.Exit();
			_activeGameState = _availableGameStates[typeof(TState)];
			_activeGameState.Enter();
		}

		TState IGameStateMachine.Get<TState>()
		{
			return (_availableGameStates[typeof(TState)] as TState);
		}
	}

}
