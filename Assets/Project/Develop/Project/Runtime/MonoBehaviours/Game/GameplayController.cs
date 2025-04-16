using Cysharp.Threading.Tasks;
using Runtime.Data.Configs;
using Runtime.Data.Configs.Core;
using Runtime.Infrastructure.Services.Game;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Game.Helper;
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
		private StoneAnimation _stoneAnimation;
		private IInputService _inputService;
		private IBoardService _boardService;
		private IGameConfig _gameConfig;

		private EventSystem _eventSystem;
		private PointerEventData _pointerData;

		[SerializeField] private float _minSwipeDistance = 30f;

		[SerializeField] private GraphicRaycaster raycaster;

		[SerializeField] private Transform _boardParent;

		private Stone _selectedGem;
		private Vector2 _dragStartPosition;

		[Inject]
		private void Construct(IInputService inputService, IBoardService boardService, IGameConfig gameConfig, StoneAnimation stoneAnimation)
		{
			_stoneAnimation = stoneAnimation;
			_inputService = inputService;
			_boardService = boardService;
			_gameConfig = gameConfig;

			_eventSystem = EventSystem.current;

			SubscribeToEvents();
		}

		private void Start()
		{
			_boardService.InitializeBoard(_boardParent);
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
				var targetGem = _boardService.GetStone(newX, newY);
				if (targetGem != null)
				{
					var selectedStone = _selectedGem;
					_boardService.SwapGemsInBoard(selectedStone, targetGem);
					_stoneAnimation.SwapWith(selectedStone, targetGem, () =>
					{
						_boardService.TrySwapOrRevert(selectedStone, targetGem);
					});

					_selectedGem = null;
				}
			}
		}

		private Stone GetGemAtPosition(Vector2 position)
		{
			_pointerData = new PointerEventData(_eventSystem)
			{
				position = position
			};

			var results = new List<RaycastResult>();
			raycaster.Raycast(_pointerData, results);

			if(results.Count > 0)
			{
				return results[0].gameObject.GetComponent<Stone>();
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
			return x >= 0 && x < _gameConfig.Width &&
				   y >= 0 && y < _gameConfig.Height;
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