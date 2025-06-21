using Runtime.Data.Progress;
using Runtime.Infrastructure.GameStateMachine.States.Core;
using Runtime.Infrastructure.Services.Game;
using Runtime.Infrastructure.Services.Game.Core;
using System;
using VContainer;

namespace Runtime.Infrastructure.GameStateMachine.States
{
	internal class LevelCompletionGameState : GameState
	{
		private ILevelInfoService _levelInfoService;
		private IBoardService _boardService;
		private IUserInfo _userInfo;

		private Action _action;

		[Inject]
		private void Construct(IBoardService boardService, IUserInfo userInfo, ILevelInfoService levelInfoService)
		{
			_levelInfoService = levelInfoService;
			_boardService = boardService;
			_userInfo = userInfo;
		}

		protected override void Enter()
		{
			base.Enter();

			_boardService.ResetAll();
			_userInfo.ScoreCheck(_levelInfoService.LevelInfo.Score);

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
