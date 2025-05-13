using LitMotion;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.MonoBehaviours.Game.Core;
using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Runtime.MonoBehaviours.Game

{
    internal class Stone : MonoBehaviour, IBoardItem
	{
        [SerializeField] private CellType _stoneType;

		private RectTransform _rectTransform;


        private IBoardItem ThisInterface => this;

        private Action<IBoardItem> _cellDestroyComplete;

		Int32 IBoardItem.X { get; set; }
		Int32 IBoardItem.Y { get; set; }

		RectTransform IBoardItem.RectTransform => _rectTransform;

		CellType IBoardItem.CellType => _stoneType;

		GameObject IBoardItem.GameObject => gameObject;

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