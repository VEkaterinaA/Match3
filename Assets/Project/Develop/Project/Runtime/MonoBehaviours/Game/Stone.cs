using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.MonoBehaviours.Game.Core;
using System;
using UnityEngine;

namespace Runtime.MonoBehaviours.Game

{
	internal class Stone : MonoBehaviour, IBoardItem
	{
		[SerializeField] private CellType _stoneType;
		[SerializeField] private Int32 _value;

		private RectTransform _rectTransform;


		private IBoardItem ThisInterface => this;

		private Action<IBoardItem> _cellDestroyComplete;

		Int32 IBoardItem.X { get; set; }
		Int32 IBoardItem.Y { get; set; }
		Int32 IBoardItem.Value => _value;

		RectTransform IBoardItem.RectTransform => _rectTransform;

		GameObject IBoardItem.GameObject => gameObject;

		CellType IBoardItem.CellType => _stoneType;

		Boolean IBoardItem.IsBooster => false;


		event Action<IBoardItem> IBoardItem.CellDestroyComplete
		{
			add => _cellDestroyComplete += value;
			remove => _cellDestroyComplete -= value;
		}


		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		void IBoardItem.Initialize(Int32 x, Int32 y)
		{
			ThisInterface.X = x;
			ThisInterface.Y = y;

			_rectTransform.localScale = Vector3.zero;
		}

		void IBoardItem.OnCellDestroyGameObject()
		{
			Destroy(gameObject);
		}

		void IBoardItem.OnCellDestroyCompleted()
		{
			_cellDestroyComplete?.Invoke(this);
		}
	}
}