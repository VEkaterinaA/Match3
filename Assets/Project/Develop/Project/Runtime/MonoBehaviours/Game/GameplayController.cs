using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.Game.Core;
using Runtime.Infrastructure.Services.Game.Helper;
using Runtime.Infrastructure.Services.Input.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.MonoBehaviours.Game.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Runtime.MonoBehaviours.Game
{
	internal class GameplayController : InjectedBehaviour
	{
		private ILevelInfoService _levelInfoService;
		private BoardItemAnimation _stoneAnimation;
		private IScreensService _screensService;
		private IInputService _inputService;
		private IBoardService _boardService;
		private IPauseable _pauseable;

		private EventSystem _eventSystem;
		private PointerEventData _pointerData;

		[SerializeField] private float _minSwipeDistance = 30f;

		[SerializeField] private GraphicRaycaster raycaster;

		[SerializeField] private Transform _boardParent;

		private IBoardItem _selectedCell;
		private Vector2 _dragStartPosition;

		private Boolean _isBoardInitialized;
		private Boolean _isPause;

		private Single _elapsedTime;


		[Inject]
		private void Construct(IInputService inputService, IBoardService boardService, BoardItemAnimation stoneAnimation, ILevelInfoService levelInfoService,
								IScreensService screensService, IPauseable pauseable)
		{
			_levelInfoService = levelInfoService;
			_stoneAnimation = stoneAnimation;
			_screensService = screensService;
			_inputService = inputService;
			_boardService = boardService;
			_pauseable = pauseable;

			_eventSystem = EventSystem.current;

			SubscribeToEvents();
		}


		private void Start()
		{
			_screensService.Show<GameScreen>();
			_boardService.InitializeBoard(_boardParent);

			_boardService.InvokeAfterInitialization(() => _isBoardInitialized = true);
		}

		private void Update()
		{
			if (!_isBoardInitialized || _isPause)
			{
				return;
			}
			_elapsedTime += Time.deltaTime;

			if (_elapsedTime >= 1)
			{
				_levelInfoService.SubsctractFromTimeLimit();
				_elapsedTime = 0;
				if(_levelInfoService.LevelInfo.TimeLimit == 0)
				{
					Time.timeScale = 0f;
					_screensService.Show<GameOverScreen>();
				}
			}
		}

		private void OnDisable()
		{
			UnsubscribeFromEvents();
		}

		private void OnBoosterCheckAndActivate()
		{
			var clickPosition = _inputService.PointerPosition != Vector2.zero ? _inputService.PointerPosition : (Vector2) Input.mousePosition;

			var clickedBooster = GetItemAtPosition<Booster>(clickPosition);

			if(clickedBooster != null)
			{
				_boardService.RunBooster(clickedBooster);
			}
		}


		private void OnStoneSelect()
		{
			var clickPosition = _inputService.PointerPosition != Vector2.zero ? _inputService.PointerPosition : (Vector2) Input.mousePosition;

			var clickedStone = GetItemAtPosition<Stone>(clickPosition);

			if (clickedStone != null)
			{
				_selectedCell = clickedStone;
				_dragStartPosition = clickPosition;
			}
		}

		private void OnGemDrag()
		{
			if (_selectedCell == null) return;

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

			var newX = _selectedCell.X + moveDirection.x;
			var newY = _selectedCell.Y + moveDirection.y;

			if (IsValidPosition(newX, newY))
			{
				var targetCell = _boardService.GetCell(newX, newY);
				if (targetCell != null)
				{
					var selectedCell = _selectedCell;
					_boardService.SwapGemsInBoard(selectedCell, targetCell);
					_stoneAnimation.SwapWith(selectedCell, targetCell, () =>
					{
						_boardService.TrySwapOrRevert(selectedCell, targetCell);
					});

					_selectedCell = null;
				}
			}
		}

		private T GetItemAtPosition<T>(Vector2 position) where T : class
		{
			_pointerData = new PointerEventData(_eventSystem)
			{
				position = position
			};

			var results = new List<RaycastResult>();
			raycaster.Raycast(_pointerData, results);

			if (results.Count > 0)
			{
				return results[0].gameObject.GetComponent<T>();
			}

			return null;
		}

		private Vector2Int GetMoveDirection(Vector2 direction)
		{
			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				return new Vector2Int(direction.x > 0 ? 1 : -1, 0);
			}
			else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
			{
				return new Vector2Int(0, direction.y > 0 ? -1 : 1);
			}

			return Vector2Int.zero;
		}

		private bool IsValidPosition(int x, int y)
		{
			return x >= 0 && x < _levelInfoService.LevelInfo.WidthOfBoard &&
				   y >= 0 && y < _levelInfoService.LevelInfo.HeightOfBoard;
		}

		private void SubscribeToEvents()
		{
			_inputService.DoubleClick += OnBoosterCheckAndActivate;
			_inputService.Select += OnStoneSelect;
			_inputService.Drag += OnGemDrag;

			_pauseable.OnPause += () => _isPause = true;
			_pauseable.OnUnpause += () => _isPause = false;
		}

		private void UnsubscribeFromEvents()
		{
			_inputService.DoubleClick -= OnBoosterCheckAndActivate;
			_inputService.Select -= OnStoneSelect;
			_inputService.Drag -= OnGemDrag;

			_pauseable.OnPause -= () => _isPause = true;
			_pauseable.OnUnpause -= () => _isPause = false;
		}
	}
}