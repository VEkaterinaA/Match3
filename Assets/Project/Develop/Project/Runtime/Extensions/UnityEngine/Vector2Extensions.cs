using System;
using Runtime.Extensions.System;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	internal static class Vector2Extensions
	{
		internal static Single GetDegreesAngleRelativeTo(this Vector2 vector, Vector2 relativeVector)
		{
			return (vector.GetScalarProduct(relativeVector) / (vector.sqrMagnitude * relativeVector.sqrMagnitude).GetSquareRoot()).GetArccosine().ToDegreesAngle();
		}

		internal static Single GetRadiansAngleRelativeTo(this Vector2 vector, Vector2 relativeVector)
		{
			return (vector.GetScalarProduct(relativeVector) / (vector.sqrMagnitude * relativeVector.sqrMagnitude).GetSquareRoot()).GetArccosine();
		}

		internal static Single GetScalarProduct(this Vector2 firstVector, Vector2 secondVector)
		{
			return ((firstVector.x * secondVector.x) + (firstVector.y * secondVector.y));
		}

		internal static Vector2 GetPerpendicular(this Vector2 vector)
		{
			return new Vector2(-vector.y, vector.x);
		}

		internal static Single DistanceTo(this Vector2 currentVector, Vector2 targetVector)
		{
			var distanceX = (currentVector.x - targetVector.x);
			var distanceY = (currentVector.y - targetVector.y);

			return ((distanceX * distanceX) + (distanceY * distanceY)).GetSquareRoot();
		}
	}
}