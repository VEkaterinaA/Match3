using System;
using UnityEngine;

namespace Runtime.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MinMaxSliderAttribute : PropertyAttribute
	{
		public Single MinLimit { get; }

		public Single MaxLimit { get; }

		internal MinMaxSliderAttribute(Single minLimit, Single maxLimit)
		{
			MinLimit = minLimit;
			MaxLimit = maxLimit;
		}
	}
}