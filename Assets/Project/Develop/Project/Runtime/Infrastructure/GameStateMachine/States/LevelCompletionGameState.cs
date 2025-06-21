using Runtime.Data.Progress;
using Runtime.Infrastructure.GameStateMachine.States.Core;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.SaveProgressServices;
using System;
using VContainer;

namespace Runtime.Infrastructure.GameStateMachine.States
{
	internal class LevelCompletionGameState : GameState
	{
		private ILevelInfoService _levelInfoService;
		private IBoardService _boardService;
		private DataService _dataService;

		private Action _action;

		[Inject]
		internal void Construct(IBoardService boardService, DataService dataService, ILevelInfoService levelInfoService)
		{
			_levelInfoService = levelInfoService;
			_boardService = boardService;
			_dataService = dataService;
		}

		protected override void Enter()
		{
			base.Enter();

			_boardService.ResetAll();
			_dataService.UserInfo.ScoreCheck(_levelInfoService.LevelInfo.Score);

			_action?.Invoke();
		}


		protected override void Exit()
		{
			base.Exit();

			_action = null;
		}

		internal void RunAllLevelCompletionProcesses(Action actionOnCompleted)
		{
			_action = actionOnCompleted;
		}
	}
}
