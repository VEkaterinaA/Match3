using System;

namespace Runtime.Infrastructure.Services.Core
{
	internal interface IPauseable
	{
		internal event Action OnPause;
		internal event Action OnUnpause;
	}

	internal interface IPauseControl
	{
		internal void RunPause();
		internal void RunUnpause();
	}
}
