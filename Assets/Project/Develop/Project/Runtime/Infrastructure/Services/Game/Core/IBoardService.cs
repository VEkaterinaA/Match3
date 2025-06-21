using Cysharp.Threading.Tasks;
using Runtime.MonoBehaviours.Game;
using Runtime.MonoBehaviours.Game.Core;
using System;
using UnityEngine;

namespace Runtime.Infrastructure.Services.Game.Core
{
	internal interface IBoardService
	{

		internal IBoardItem[,] Board { get; }

		internal UniTask InitializeBoard(Transform boardParent);

		internal void RunBooster(IBoardItem booster);

		internal void TrySwapOrRevert(IBoardItem cellOne, IBoardItem cellTwo);

		internal void SwapGemsInBoard(IBoardItem cellOne, IBoardItem cellTwo);

		internal IBoardItem GetCell(Int32 x, Int32 y);

		internal void HandleMatchesAfterSwap();
	}
}
