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
		internal Gem[,] Board { get; }

		internal List<GemType> GemTypes { get; }

		internal UniTask InitializeBoard(Transform boardParent);
	}
}
