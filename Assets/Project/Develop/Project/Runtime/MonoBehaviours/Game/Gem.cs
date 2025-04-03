using LitMotion;
using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine;
using System;
using UnityEngine;

namespace Runtime.MonoBehaviours.Game

{
	internal class Gem : MonoBehaviour
	{
		public int Type { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		[SerializeField] private float _moveDuration = 0.3f; // длительность анимации

		private RectTransform _rectTransform;
		private CompositeMotionHandle _сompositeMotionHandle;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
			_сompositeMotionHandle = new CompositeMotionHandle();
		}

		internal void Initialize(Int32 type, Int32 x, Int32 y)
		{
			Type = type;
			X = x;
			Y = y;
		}

		public void MoveTo(Vector2 newPosition)
		{
			var handle = _rectTransform.CreateMotion(
				_rectTransform.anchoredPosition,
				newPosition,
				_moveDuration);

			_сompositeMotionHandle.Add(handle.BindToAnchoredPosition());
		}

		public void MoveToCell(int newX, int newY, float cellSize, Vector2 offset, Single height)
		{

			var position = new Vector2(
							newX * cellSize + offset.x,
							(height - 1 - newY) * cellSize + offset.y
);

			MoveTo(position);
		}

		public void SwapWith(Gem otherGem, BoardInitializer boardInitializer)
		{
			boardInitializer.SwapGemsInBoard(this, otherGem);

			MoveToCell(X, Y, boardInitializer.CellSize, boardInitializer.GetBoardOffset(), boardInitializer.Height);
			otherGem.MoveToCell(otherGem.X, otherGem.Y, boardInitializer.CellSize, boardInitializer.GetBoardOffset(), boardInitializer.Height);
		}

		public void PlayMatchAnimation()
		{
			var handle = transform.CreateMotion(transform.localScale, Vector3.zero, 0.2f);
			handle.AddEase(Ease.InBack);
			handle.WithOnComplete(() => gameObject.SetActive(false));

			_сompositeMotionHandle.Add(handle.BindToLocalScale());

		}

		public void PlaySpawnAnimation()
		{
			transform.localScale = Vector3.zero;
			var handle = transform.CreateMotion(
				Vector3.zero,
				Vector3.one,
				0.2f);
			handle.AddEase(Ease.OutBack);

			_сompositeMotionHandle.Add(handle.BindToLocalScale());
		}

		private void OnDestroy()
		{
			_сompositeMotionHandle.Cancel();
		}
	}
}