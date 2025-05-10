using Runtime.Data.Constants.Enums;
using Runtime.Data.Progress;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Services.Game.Core;
using System;
using UnityEngine;

namespace Runtime.Infrastructure.Services.Game
{
	internal class LevelInfoService : ILevelInfoService, IInitializationInformer
    {
        private LevelInfo _levelInfo;

        private Action _initialized;

		private Action _timeChanged;

		private Action _moveCompleted;

		private Action _goalQuantityChanged;

        private Boolean _isInitialized;


		Boolean IInitializationInformer.IsInitialized => _isInitialized;

        LevelInfo ILevelInfoService.LevelInfo => _levelInfo;

		event Action ILevelInfoService.MoveCompleted
		{
			add => _moveCompleted += value;
			remove => _moveCompleted -= value;
		}

		event Action ILevelInfoService.GoalQuantityChanged
		{
			add => _goalQuantityChanged += value;
			remove => _goalQuantityChanged -= value;
		}

		event Action ILevelInfoService.TimeChanged
		{
			add => _timeChanged += value;
			remove => _timeChanged -= value;
		}


		event Action IInitializationInformer.Initialized
        {
            add => _initialized += value;
            remove => _initialized -= value;
        }

        void ILevelInfoService.SetLevelInfo(Int32 widthOfBoard, Int32 heightOfBoard, TargetType targetType, Int32? moveLimit, Int32? goalQuantity, Int32? timeLimit)
        {
            _levelInfo = new(widthOfBoard, heightOfBoard, targetType, moveLimit, goalQuantity, timeLimit);

            _isInitialized = true;
            _initialized?.Invoke();
        }

		void ILevelInfoService.SubsctractFromGoalQuantity()
		{
			if(!_levelInfo.GoalQuantity.HasValue)
			{
				return;
			}

			_levelInfo.GoalQuantity--;
			_goalQuantityChanged?.Invoke();			
		}

		void ILevelInfoService.SubsctractFromMoveLimit()
		{
			if(!_levelInfo.MoveLimit.HasValue)
            {
                return;
            }

            _levelInfo.MoveLimit--;
            _moveCompleted?.Invoke();
		}

		void ILevelInfoService.SubsctractFromTimeLimit()
		{
			if(!_levelInfo.TimeLimit.HasValue)
			{
				return;
			}

			_levelInfo.TimeLimit--;
			_timeChanged?.Invoke();
		}
	}
}
