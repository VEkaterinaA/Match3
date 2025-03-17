using System;
using Runtime.Extensions.System;
using LitMotion;
using LitMotion.Adapters;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	internal static class Vector3Extensions
	{
		internal static Vector3 Lerp(this Single lerpValue, Vector3 firstVector, Vector3 secondVector)
		{
			var x = lerpValue.Lerp(firstVector.x, secondVector.x);
			var y = lerpValue.Lerp(firstVector.y, secondVector.y);
			var z = lerpValue.Lerp(firstVector.z, secondVector.z);

			return new Vector3(x, y, z);
		}

		internal static Quaternion LookRotation(this Vector3 vectorForward, Vector3 vectorUpward)
		{
			return Quaternion.LookRotation(vectorForward, vectorUpward);
		}

		internal static Quaternion ConvertToQuaternion(this Vector3 eulerAngles)
		{
			return Quaternion.Euler(eulerAngles);
		}

		internal static Boolean IsApproximately(this Vector3 firstVector, Vector3 secondVector, Single tolerance = 0.0001f)
		{
			return (firstVector.x.IsApproximately(secondVector.x, tolerance) && firstVector.y.IsApproximately(secondVector.y, tolerance) && firstVector.z.IsApproximately(firstVector.z, tolerance));
		}

		internal static Single DistanceTo(this Vector3 currentVector, Vector3 targetVector)
		{
			var distanceX = (currentVector.x - targetVector.x);
			var distanceY = (currentVector.y - targetVector.y);
			var distanceZ = (currentVector.z - targetVector.z);

			return ((distanceX * distanceX) + (distanceY * distanceY) + (distanceZ * distanceZ)).GetSquareRoot();
		}

		internal static Vector3 SetY(this Vector3 vector, Single y)
		{
			return new Vector3(vector.x, y, vector.z);
		}

		internal static MotionHandle BindToLocalPosition(this (Transform Transform, MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, ((localPosition, transform) => transform.localPosition = localPosition));
		}

		internal static MotionHandle BindToPosition(this (Transform Transform, MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, ((position, transform) => transform.position = position));
		}

		internal static MotionHandle BindToRotation(this (Transform Transform, MotionBuilder<Quaternion, NoOptions, QuaternionMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, ((rotation, transform) => transform.rotation = rotation));
		}

		internal static MotionHandle BindToLocalRotation(this (Transform Transform, MotionBuilder<Quaternion, NoOptions, QuaternionMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, ((rotation, transform) => transform.localRotation = rotation));
		}
	}
}