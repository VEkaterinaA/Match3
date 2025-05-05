using Runtime.Data.Constants.Enums;
using Runtime.Data.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime.Infrastructure.Services.Game.Core
{
    internal interface ILevelInfoService
    {
        internal LevelInfo LevelInfo { get; }

        internal void SetLevelInfo(Int32 widthOfBoard, Int32 heightOfBoard, TargetType targetType, Int32 moveLimit = default, Int32 quantity = default, Int32 timeLimit = default);
    }
}
