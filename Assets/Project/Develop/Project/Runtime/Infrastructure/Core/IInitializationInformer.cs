using System;

namespace Runtime.Infrastructure.Core
{
	internal interface IInitializationInformer
	{
		internal event Action Initialized;

		internal Boolean IsInitialized { get; }
	}
}