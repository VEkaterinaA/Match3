using System;
using UnityEngine;

namespace Runtime.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class GetComponentAttribute : PropertyAttribute
	{
		public Boolean AddComponentIfNotFound { get; }

		public Type OverrideTypeToAdd { get; }

		internal GetComponentAttribute(Boolean addComponentIfNotFound = true, Type overrideTypeToAdd = null)
		{
			AddComponentIfNotFound = addComponentIfNotFound;
			OverrideTypeToAdd = overrideTypeToAdd;
		}
	}
}