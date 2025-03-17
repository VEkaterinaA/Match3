using System;

namespace Runtime.Infrastructure.Services.Core
{
	internal interface ILoopsService
	{
		internal event Action FixedUpdated;

		internal event Action LateUpdated;

		internal event Action Updated;
	}
}
