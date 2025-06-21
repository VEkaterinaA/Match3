using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using System;
using UnityEngine;

namespace Runtime.MonoBehaviours.Game.Core
{
	internal interface IBoardItem
	{

		internal event Action<IBoardItem> CellDestroyComplete;

		internal CellType CellType { get; }

		internal GameObject GameObject { get; }

		internal RectTransform RectTransform { get; }

		internal Boolean IsBooster { get; }

		internal Int32 Value { get; }

		internal Int32 X { get; set; }

		internal Int32 Y { get; set; }

		internal void Initialize(Int32 x, Int32 y);

		internal void OnCellDestroyCompleted();

		internal void OnCellDestroyGameObject();

	}
}
