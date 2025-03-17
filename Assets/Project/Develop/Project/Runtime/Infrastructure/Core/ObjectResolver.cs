using System;
using VContainer;
using VContainer.Diagnostics;

namespace Runtime.Infrastructure.Core
{
	internal abstract class ObjectResolver : IObjectResolver
	{
		private IObjectResolver _objectResolver;

		Object IObjectResolver.ApplicationOrigin => _objectResolver.ApplicationOrigin;

		DiagnosticsCollector IObjectResolver.Diagnostics
		{
			get => _objectResolver.Diagnostics;
			set => _objectResolver.Diagnostics = value;
		}

		[Inject]
		internal void Construct(IObjectResolver objectResolver)
		{
			_objectResolver = objectResolver;
		}

		Boolean IObjectResolver.TryGetRegistration(Type type, out Registration registration)
		{
			return _objectResolver.TryGetRegistration(type, out registration);
		}

		IScopedObjectResolver IObjectResolver.CreateScope(Action<IContainerBuilder> installation)
		{
			return _objectResolver.CreateScope(installation);
		}

		Boolean IObjectResolver.TryResolve(Type type, out Object resolved)
		{
			return _objectResolver.TryResolve(type, out resolved);
		}

		Object IObjectResolver.Resolve(Registration registration)
		{
			return _objectResolver.Resolve(registration);
		}

		void IObjectResolver.Inject(Object instance)
		{
			_objectResolver.Inject(instance);
		}

		Object IObjectResolver.Resolve(Type type)
		{
			return _objectResolver.Resolve(type);
		}

		void IDisposable.Dispose()
		{
			_objectResolver.Dispose();
		}
	}
}