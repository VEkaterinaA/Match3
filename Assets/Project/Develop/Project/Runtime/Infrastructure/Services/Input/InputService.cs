using Runtime.Infrastructure.Services.Input.Core;
using System;
using VContainer.Unity;
using InputSystem = Infrastructure.Services.Input.Systems.InputSystem;

namespace Runtime.Infrastructure.Services.Input
{
	internal class InputService : IInputService, IInitializable, IDisposable
	{
		private readonly InputSystem _inputSystem = new InputSystem();

		private Boolean _isEnabled;

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
					_inputSystem.Dispose();
					Unsubscribe();
				}
			}
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

		}

		private void Unsubscribe()
		{

		}



	}
}