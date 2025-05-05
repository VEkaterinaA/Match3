using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class LevelInfo
	{
		private Int32 _widthOfBoard;
		private Int32 _heightOfBoard;

		private Int32 _moveLimit;

		private TargetType _targetType;
		private Int32 _quantity;

		private Int32 _timeLimit;

        internal Int32 WidthOfBoard { get => _widthOfBoard; set => _widthOfBoard = value; }
        internal Int32 HeightOfBoard { get => _heightOfBoard; set => _heightOfBoard = value; }
        internal Int32 MoveLimit { get => _moveLimit; set => _moveLimit = value; }
        internal Int32 Quantity { get => _quantity; set => _quantity = value; }
        internal Int32 TimeLimit { get => _timeLimit; set => _timeLimit = value; }
        internal TargetType TargetType { get => _targetType; set => _targetType = value; }
    }
}