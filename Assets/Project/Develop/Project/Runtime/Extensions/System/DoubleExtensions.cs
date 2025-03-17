using System;

namespace Runtime.Extensions.System
{
	internal static class DoubleExtensions
	{
		internal static Double Lerp(this Double lerpValue, Double firstValue, Double secondValue)
		{
			var delta = ((secondValue - firstValue) * lerpValue.Clamp(0.0f, 1.0f));

			return (firstValue + delta);
		}

		internal static Double Clamp(this Double value, Double minValue, Double maxValue)
		{
			if (value < minValue)
			{
				return minValue;
			}

			if (value > maxValue)
			{
				return maxValue;
			}

			return value;
		}
	}
}