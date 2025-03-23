using Runtime.Infrastructure.Services.Input.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;
using InputCallback = UnityEngine.InputSystem.InputAction.CallbackContext;


namespace Runtime.Infrastructure.Services.Input
{
	internal class InputService : IInputService, IInitializable, IDisposable
	{
		private readonly InputSystem _inputSystem = new InputSystem();

		private Boolean _isEnabled;
		private Boolean _isDragging;
		private Vector2 _startPosition;
		private const float MinSwipeDistance = 30f;


		private ActionWrapper _selectActionWrapper;
		private ActionWrapper _swipeActionWrapper;
		private ActionWrapper _dragActionWrapper;

		private IInputService Service => this;

		Boolean IInputService.IsEnabled
		{
			get => _isEnabled;
			set
			{
				if (_isEnabled == value)
				{
					return;
				}

				_isEnabled = value;

				if (_isEnabled)
				{
					_inputSystem.Enable();
					Subscribe();
				}
				else
				{
					_inputSystem.Disable();
					Unsubscribe();
				}
			}
		}

		event Action IInputService.Select
		{
			add => _selectActionWrapper.Started += value;
			remove => _selectActionWrapper.Started -= value;
		}

		event Action IInputService.Swipe
		{
			add => _swipeActionWrapper.Performed += value;
			remove => _swipeActionWrapper.Performed -= value;
		}

		event Action IInputService.Drag
		{
			add => _dragActionWrapper.Performed += value;
			remove => _dragActionWrapper.Performed -= value;
		}


		void IInitializable.Initialize()
		{
			Service.IsEnabled = true;
		}

		void IDisposable.Dispose()
		{
			_inputSystem.Dispose();
		}

		private void Subscribe()
		{
			_selectActionWrapper = new ActionWrapper(_inputSystem.Player.Select);
			_swipeActionWrapper = new ActionWrapper(_inputSystem.Player.EndDrag);
			_dragActionWrapper = new ActionWrapper(_inputSystem.Player.DragDelta);

			_selectActionWrapper.Started += HandleSelectStarted;
			_selectActionWrapper.Performed += HandleSelectPerformed;

			_swipeActionWrapper.Started += HandleSwipeStarted;
			_swipeActionWrapper.Performed += HandleSwipePerformed;

			_dragActionWrapper.Started += HandleDragStarted;
			_dragActionWrapper.Performed += HandleDragPerformed;

			_selectActionWrapper.IsEnabled = true;
			_swipeActionWrapper.IsEnabled = true;
			_dragActionWrapper.IsEnabled = true;
		}

		private void Unsubscribe()
		{
			_selectActionWrapper.Started -= HandleSelectStarted;
			_selectActionWrapper.Performed -= HandleSelectPerformed;

			_swipeActionWrapper.Started -= HandleSwipeStarted;
			_swipeActionWrapper.Performed -= HandleSwipePerformed;

			_dragActionWrapper.Started -= HandleDragStarted;
			_dragActionWrapper.Performed -= HandleDragPerformed;
		}

		private void HandleSelectStarted()
		{
			_isDragging = true;
			_startPosition = _inputSystem.Player.Position.ReadValue<Vector2>();
			OnSelectPerformed?.Invoke(_startPosition);
		}

		private void HandleSelectPerformed()
		{
			// Дополнительная логика при завершении нажатия, если нужна
		}

		private void HandleSwipeStarted()
		{
			// Логика начала свайпа, если нужна
		}

		private void HandleSwipePerformed()
		{
			if (!_isDragging) return;

			_isDragging = false;
			var endPosition = _inputSystem.Player.Position.ReadValue<Vector2>();
			var dragVector = endPosition - _startPosition;

			if (dragVector.magnitude >= MinSwipeDistance)
			{
				var direction = GetMainDragDirection(dragVector);
				OnSwipePerformed?.Invoke(direction);
			}
		}

		private void HandleDragStarted()
		{
			// Логика начала перетаскивания, если нужна
		}

		private void HandleDragPerformed()
		{
			if (!_isDragging) return;

			var currentPosition = _inputSystem.Player.Position.ReadValue<Vector2>();
			OnDragPerformed?.Invoke(currentPosition);
		}

		private Vector2 GetMainDragDirection(Vector2 dragVector)
		{
			dragVector.Normalize();
			return Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y)
				? new Vector2(Mathf.Sign(dragVector.x), 0)
				: new Vector2(0, Mathf.Sign(dragVector.y));
		}

		private sealed class ActionWrapper : IDisposable
		{
			private readonly InputAction _inputAction;

			private Boolean _isEnabled;

			internal event Action Started;

			internal event Action Performed;

			internal event Action Canceled;

			internal Boolean IsEnabled
			{
				get => _isEnabled;
				set
				{
					if (_isEnabled == value)
					{
						return;
					}

					_isEnabled = value;

					if (value)
					{
						Subscribe();
					}
					else
					{
						Unsubscribe();
					}
				}
			}

			internal ActionWrapper(InputAction inputAction)
			{
				_inputAction = inputAction;
			}

			void IDisposable.Dispose()
			{
				_inputAction.Disable();
			}

			internal TStruct ReadValue<TStruct>() where TStruct : struct
			{
				return _inputAction.ReadValue<TStruct>();
			}

			private void HandleStart(InputCallback inputCallback)
			{

				Started?.Invoke();
			}

			private void HandlePerformance(InputCallback inputCallback)
			{
				Performed?.Invoke();
			}

			private void HandleCancellation(InputCallback inputCallback)
			{
				Canceled?.Invoke();
			}

			private void Subscribe()
			{
				_inputAction.started += HandleStart;
				_inputAction.performed += HandlePerformance;
				_inputAction.canceled += HandleCancellation;
			}

			private void Unsubscribe()
			{
				_inputAction.started -= HandleStart;
				_inputAction.performed -= HandlePerformance;
				_inputAction.canceled -= HandleCancellation;
			}
		}

	}

}