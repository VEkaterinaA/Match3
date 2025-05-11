using Runtime.Data.Constants.Enums;
using Runtime.Data.Progress;
using System;

namespace Runtime.Infrastructure.Services.Game.Core
{
	internal interface ILevelInfoService
	{
		internal event Action TimeChanged;

		internal event Action MoveCompleted;

		internal event Action GoalQuantityChanged;

		internal LevelInfo LevelInfo { get; }

		internal void SetLevelInfo(Int32 widthOfBoard, Int32 heightOfBoard, TargetType targetType, Int32? moveLimit = null, Int32? goalQuantity = null, Int32? timeLimit = null);

		internal void SubsctractFromMoveLimit();

		internal void SubsctractFromTimeLimit();

		internal void SubsctractFromGoalQuantity();

	}
}
