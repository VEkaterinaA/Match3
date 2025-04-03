using Runtime.Infrastructure.Services.Input.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Runtime.MonoBehaviours.Game
{
	internal class GameplayController : InjectedBehaviour
	{

		private IInputService _inputService;

		private EventSystem _eventSystem;
		private PointerEventData _pointerData;

		[SerializeField] private float _minSwipeDistance = 30f;

		[SerializeField] private GraphicRaycaster raycaster;

		[SerializeField] private BoardInitializer _boardInitializer;

		private Gem _selectedGem;
		private Vector2 _dragStartPosition;

		[Inject]
		private void Construct(IInputService inputService)
		{
			_inputService = inputService;

			_eventSystem = EventSystem.current;

			SubscribeToEvents();
		}

		private void OnDisable()
		{
			UnsubscribeFromEvents();
		}


		private void OnGemSelect()
		{
			var clickPosition = _inputService.PointerPosition != Vector2.zero ? _inputService.PointerPosition : (Vector2) Input.mousePosition;

			var clickedGem = GetGemAtPosition(clickPosition);

			if (clickedGem != null)
			{
				_selectedGem = clickedGem;
				_dragStartPosition = clickPosition;
			}
		}

		private void OnGemDrag()
		{
			if (_selectedGem == null) return;

			var currentPosition = _inputService.PointerPosition;
			var dragDelta = currentPosition - _dragStartPosition;

			if (dragDelta.magnitude >= _minSwipeDistance)
			{
				TrySwapGems(dragDelta);
			}
		}

		private void TrySwapGems(Vector2 dragDelta)
		{
			var direction = dragDelta.normalized;
			var moveDirection = GetMoveDirection(direction);

			var newX = _selectedGem.X + moveDirection.x;
			var newY = _selectedGem.Y + moveDirection.y;

			if (IsValidPosition(newX, newY))
			{
				var targetGem = _boardInitializer.GetGem(newX, newY);
				if (targetGem != null)
				{
					_selectedGem.SwapWith(targetGem, _boardInitializer);
					_selectedGem = null;

				}
			}
		}

		private Gem GetGemAtPosition(Vector2 position)
		{
			_pointerData = new PointerEventData(_eventSystem)
			{
				position = position
			};

			var results = new List<RaycastResult>();
			raycaster.Raycast(_pointerData, results);

			if(results.Count > 0)
			{
				return results[0].gameObject.GetComponent<Gem>();
			}

			return null;
		}

		private Vector2Int GetMoveDirection(Vector2 direction)
		{
			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				return new Vector2Int(direction.x > 0 ? 1 : -1, 0);
			}
			else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
			{
				return new Vector2Int(0, direction.y > 0 ? -1 : 1);
			}

			return Vector2Int.zero;
		}

		private bool IsValidPosition(int x, int y)
		{
			return x >= 0 && x < _boardInitializer.Width &&
				   y >= 0 && y < _boardInitializer.Height;
		}

		private void SubscribeToEvents()
		{
			_inputService.Select += OnGemSelect;
			_inputService.Drag += OnGemDrag;
		}

		private void UnsubscribeFromEvents()
		{
			_inputService.Select -= OnGemSelect;
			_inputService.Drag -= OnGemDrag;
		}
	}
}