using Runtime.Data.Constants.Enums;
using System;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class LevelInfo
	{
		private Int32 _widthOfBoard;
		private Int32 _heightOfBoard;

		private Int32? _moveLimit;

		private TargetType _targetType;
		private Int32? _goalQuantity;

		private Int32? _timeLimit;

		internal int WidthOfBoard { get => _widthOfBoard; set => _widthOfBoard = value; }
		internal int HeightOfBoard { get => _heightOfBoard; set => _heightOfBoard = value; }
		internal int? MoveLimit { get => _moveLimit; set => _moveLimit = value; }
		internal TargetType TargetType { get => _targetType; set => _targetType = value; }
		internal int? GoalQuantity { get => _goalQuantity; set => _goalQuantity = value; }
		internal Int32? TimeLimit { get => _timeLimit; set => _timeLimit = value; }


		internal LevelInfo(Int32 widthOfBoard, Int32 heightOfBoard, TargetType targetType, Int32? moveLimit = null, Int32? goalQuantity = null, Int32? timeLimit = null)
		{
			_widthOfBoard = widthOfBoard;
			_heightOfBoard = heightOfBoard;
			_targetType = targetType;
			_moveLimit = moveLimit;
			_goalQuantity = goalQuantity;
			_timeLimit = timeLimit;
		}
	}
}