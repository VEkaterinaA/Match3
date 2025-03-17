using System;
using UnityEngine;

namespace Runtime.Extensions.System
{
	internal static class SingleExtensions
	{
		internal static Boolean IsApproximately(this Single firstValue, Single secondValue, Single tolerance = 0.0001F)
		{
			return ((firstValue - secondValue).GetAbsolute() < tolerance);
		}

		internal static Single GetSquareRoot(this Single single)
		{
			return MathF.Sqrt(single);
		}

		internal static Single Lerp(this Single lerpValue, Single firstValue, Single secondValue)
		{
			var delta = ((secondValue - firstValue) * lerpValue.Clamp(0.0F, 1.0F));

			return (firstValue + delta);
		}

		internal static Int32 GetSign(this Single single)
		{
			return single switch
			{
				< 0.0F => -1,
				> 0.0F => 1,
				_ => 0,
			};
		}

		internal static Single ClampMaxValue(this Single single, Single maxValue)
		{
			return (single > maxValue) ? maxValue : single;
		}

		internal static Single ClampMinValue(this Single single, Single minValue)
		{
			return (single < minValue) ? minValue : single;
		}

		internal static Single Clamp(this Single value, Single minValue, Single maxValue)
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

		public static Single ClampAngle(this Single angle)
		{
			var targetAngle = (angle % 360.0F);

			if (targetAngle < 0.0F)
			{
				targetAngle += 360.0F;
			}

			return targetAngle.GetAbsolute();
		}

		internal static Single Power(this Single value, Single power)
		{
			return MathF.Pow(value, power);
		}

		internal static Single MoveTowards(this Single single, Single target, Single maxDelta)
		{
			var delta = (target - single).GetAbsolute();

			if (delta <= maxDelta)
			{
				return target;
			}

			return (single + (Mathf.Sign(target - single) * maxDelta));
		}

		internal static Single ToDegreesAngle(this Single radiansAngles)
		{
			return (radiansAngles * 57.29577951308F);
		}

		internal static Single GetRadiansAngle(this Single degreesAngle)
		{
			return (degreesAngle * 0.01745329252F);
		}

		internal static Single GetAbsolute(this Single single)
		{
			return ((single < 0) ? -single : single);
		}

		internal static Single GetArccosine(this Single radians)
		{
			return MathF.Acos(radians);
		}

		internal static Single GetCosine(this Single radians)
		{
			return MathF.Cos(radians);
		}

		internal static Single GetArcsine(this Single radians)
		{
			return MathF.Asin(radians);
		}

		internal static Single GetSine(this Single radians)
		{
			return MathF.Sin(radians);
		}

		[Obsolete]
		internal static Single GetDegreesAngleSine(this Single degreesAngle)
		{
			return MathF.Sin(degreesAngle.GetRadiansAngle());
		}
	}
}