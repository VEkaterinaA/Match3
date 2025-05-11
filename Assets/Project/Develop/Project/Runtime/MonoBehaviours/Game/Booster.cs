using LitMotion;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using System;
using UnityEngine;

namespace Runtime.MonoBehaviours.Game
{
	internal class Booster : MonoBehaviour
	{
		[SerializeField]
		private BoosterType _boosterType;

		internal int X { get; set; }
		internal int Y { get; set; }


		internal BoosterType BoosterType => _boosterType;

		private RectTransform _rectTransform;


		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		internal void Initialize(Int32 x, Int32 y)
		{
			X = x;
			Y = y;

			_rectTransform.localScale = Vector3.zero;
		}

	}
}
