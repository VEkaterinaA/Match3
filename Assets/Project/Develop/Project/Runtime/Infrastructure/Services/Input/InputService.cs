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

		private const Single _doubleClickThreshold = 0.3f;

		private Single _lastClickTime;
		private Int32 _clickCounter;
		private Boolean _isEnabled;

		private ActionWrapper _selectActionWrapper;
		private ActionWrapper _swipeActionWrapper;
		private ActionWrapper _dragActionWrapper;
		private Action _doubleClick;

		private IInputService Service => this;

		Vector2 IInputService.PointerPosition => (_isEnabled ? _inputSystem.Player.Position.ReadValue<Vector2>() : Vector2.zero);

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

				_selectActionWrapper.IsEnabled = _isEnabled;
				_swipeActionWrapper.IsEnabled = _isEnabled;
				_dragActionWrapper.IsEnabled = _isEnabled;

				if (_isEnabled)
				{
					_inputSystem.Enable();
				}
				else
				{
					_inputSystem.Disable();
				}
			}
		}

		event Action IInputService.DoubleClick
		{
			add => _doubleClick += value;
			remove => _doubleClick -= value;
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
			_selectActionWrapper = new ActionWrapper(_inputSystem.Player.Select);
			_swipeActionWrapper = new ActionWrapper(_inputSystem.Player.EndDrag);
			_dragActionWrapper = new ActionWrapper(_inputSystem.Player.DragDelta);

			Subscribe();

			Service.IsEnabled = true;
		}

		void IDisposable.Dispose()
		{
			Unsubscribe();

			_inputSystem.Dispose();
		}

		private void ClickCounter()
		{
			var time = Time.time;

			if (time - _lastClickTime <= _doubleClickThreshold)
			{
				_clickCounter++;
			}
			else
			{
				_clickCounter = 1;
			}

			_lastClickTime = time;

			if(_clickCounter == 2)
			{
				_doubleClick?.Invoke();

				_clickCounter = 0;
				_lastClickTime = 0;
			}
		}

		private void Subscribe()
		{
			_selectActionWrapper.Started += ClickCounter;
		}

		private void Unsubscribe()
		{
			_selectActionWrapper.Started -= ClickCounter;
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