using Cysharp.Threading.Tasks;
using Runtime.MonoBehaviours.Game;
using System;
using UnityEngine;

namespace Runtime.Infrastructure.Services.Game.Core
{
	internal interface IBoardService
	{

		internal Stone[,] Board { get; }

		internal UniTask InitializeBoard(Transform boardParent);

		internal void TrySwapOrRevert(Stone stoneOne, Stone stoneTwo);

		internal void SwapGemsInBoard(Stone gem1, Stone gem2);

		internal Stone GetStone(Int32 x, Int32 y);

		internal void HandleMatchesAfterSwap();
	}
}
