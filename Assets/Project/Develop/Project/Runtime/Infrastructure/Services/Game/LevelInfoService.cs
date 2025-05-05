using Runtime.Data.Constants.Enums;
using Runtime.Data.Progress;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Services.Game.Core;
using System;

namespace Runtime.Infrastructure.Services.Game
{
    internal class LevelInfoService : ILevelInfoService, IInitializationInformer
    {
        private LevelInfo _levelInfo;

        private Action _initialized;

        private Boolean _isInitialized;

        Boolean IInitializationInformer.IsInitialized => _isInitialized;

        LevelInfo ILevelInfoService.LevelInfo => _levelInfo;


        event Action IInitializationInformer.Initialized
        {
            add => _initialized += value;
            remove => _initialized -= value;
        }

        void ILevelInfoService.SetLevelInfo(Int32 widthOfBoard, Int32 heightOfBoard, TargetType targetType, Int32 moveLimit, Int32 quantity, Int32 timeLimit)
        {
            _levelInfo = new();

            _levelInfo.WidthOfBoard = widthOfBoard;
            _levelInfo.HeightOfBoard = heightOfBoard;
            _levelInfo.MoveLimit = moveLimit;
            _levelInfo.TargetType = targetType;
            _levelInfo.Quantity = quantity;
            _levelInfo.TimeLimit = timeLimit;

            _isInitialized = true;
            _initialized?.Invoke();
        }
    }
}
