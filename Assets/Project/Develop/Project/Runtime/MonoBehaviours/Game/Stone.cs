using LitMotion;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine;
using System;
using UnityEngine;

namespace Runtime.MonoBehaviours.Game

{
	internal class Stone : MonoBehaviour
	{
		public int X { get; set; }
		public int Y { get; set; }
		public StoneType StoneType => _stoneType;

		[SerializeField] private StoneType _stoneType;
		[SerializeField] private float _moveDuration = 0.3f;
		[SerializeField] private float _spawnAnimationDuration = 0.2f;
		[SerializeField] private float _destroyAnimationDuration = 0.8f;

		internal Action<Stone> GemDestroyComplete;

		private RectTransform _rectTransform;
		private CompositeMotionHandle _сompositeMotionHandle;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
			_сompositeMotionHandle = new CompositeMotionHandle();
		}

		internal void Initialize(Int32 x, Int32 y)
		{
			X = x;
			Y = y;
		}


		private void DestroyGem()
		{
			Destroy(this);
		}

		private void OnDestroy()
		{
			_сompositeMotionHandle.Cancel();
		}
	}
}