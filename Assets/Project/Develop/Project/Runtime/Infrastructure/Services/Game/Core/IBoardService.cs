using Cysharp.Threading.Tasks;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.MonoBehaviours.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runtime.Infrastructure.Services.Game.Core
{
	internal interface IBoardService
	{
		internal Stone[,] Board { get; }

		internal List<StoneType> GemTypes { get; }

		internal UniTask InitializeBoard(Transform boardParent);

		internal void SwapGemsInBoard(Stone gem1, Stone gem2);

		internal Stone GetStone(Int32 x, Int32 y);

		internal void HandleMatchesAfterSwap();

		internal Vector2 GetBoardOffset();
	}
}
