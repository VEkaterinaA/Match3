using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Runtime.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class GetComponentInChildrenAttribute : PropertyAttribute
	{
		[UsedImplicitly]
		public Boolean AddComponentIfNotFound { get; }

		internal GetComponentInChildrenAttribute(Boolean addComponentIfNotFound = false)
		{
			AddComponentIfNotFound = addComponentIfNotFound;
		}
	}
}