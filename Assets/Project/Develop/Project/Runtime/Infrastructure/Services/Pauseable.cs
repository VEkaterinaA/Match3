using Runtime.Infrastructure.Services.Core;
using System;

namespace Runtime.Infrastructure.Services
{
	internal sealed class Pauseable : IPauseable, IPauseControl
	{
		private Action _pause;
		private Action _unPause;

		event Action IPauseable.OnPause
		{
			add => _pause += value;
			remove => _pause -= value;
		}

		event Action IPauseable.OnUnpause
		{
			add => _unPause += value;
			remove => _unPause -= value;
		}

		void IPauseControl.RunPause()
		{
			_pause?.Invoke();
		}
		void IPauseControl.RunUnpause()
		{
			_unPause?.Invoke();
		}
	}
}
