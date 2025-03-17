using Runtime.Attributes;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Runtime.DI
{
	public class CustomLifetimeScope : LifetimeScope
	{
		[SerializeField]
		[ReadOnlyField]
		private List<Component> _injectableComponents;

		protected override void Configure(IContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterBuildCallback(ResolveComponents);
		}

		private void ResolveComponents(IObjectResolver objectResolver)
		{
			foreach (var injectableComponent in _injectableComponents)
			{
				objectResolver.Inject(injectableComponent);
			}
		}
	}
}
