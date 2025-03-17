using Runtime.Data.Constants.Enums;
using LitMotion;
using LitMotion.Adapters;
using System;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	public static class TransformExtensions
	{
		public static MotionHandle Scale(this Transform transform, Vector3 startScale, Vector3 endScale, Single duration)
		{
			return LMotion.Create(startScale, endScale, duration).Bind(scale => transform.localScale = scale);
		}

		public static MotionHandle Scale(this Transform transform, Vector3 endScale, Single duration)
		{
			return transform.Scale(transform.localScale, endScale, duration);
		}

		public static MotionHandle MoveX(this Transform transform, Single startPositionX, Single endPositionX, Single duration)
		{
			return LMotion.Create(startPositionX, endPositionX, duration).Bind(positionX =>
			{
				var targetPosition = transform.position;
				targetPosition.x = positionX;

				transform.position = targetPosition;
			});
		}

		internal static MotionHandle BindToLocalScale(this (Transform Transform, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder, Axis axis)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, (localScale, transform) =>
			{
				var targetLocalEulerAngles = transform.localScale;
				targetLocalEulerAngles[(Int32) axis] = localScale;

				transform.localScale = targetLocalEulerAngles;
			});
		}

		internal static MotionHandle BindToLocalEulerAngle(this (Transform Transform, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder, Axis axis)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, (localEulerAngle, transform) =>
			{
				var targetLocalEulerAngles = transform.localEulerAngles;
				targetLocalEulerAngles[(Int32) axis] = localEulerAngle;

				transform.localEulerAngles = targetLocalEulerAngles;
			});
		}

		internal static MotionHandle BindToEulerAngle(this (Transform Transform, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder, Axis axis)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, (eulerAngle, transform) =>
			{
				var targetEulerAngles = transform.eulerAngles;
				targetEulerAngles[(Int32) axis] = eulerAngle;

				transform.eulerAngles = targetEulerAngles;
			});
		}

		internal static MotionHandle BindToPosition(this (Transform Transform, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder, Axis axis)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, (localEulerAngle, transform) =>
			{
				var targetPosition = transform.position;
				targetPosition[(Int32) axis] = localEulerAngle;

				transform.position = targetPosition;
			});
		}

		internal static MotionHandle BindToLocalPosition(this (Transform Transform, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder, Axis axis)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, (localEulerAngle, transform) =>
			{
				var targetPosition = transform.position;
				targetPosition[(Int32) axis] = localEulerAngle;

				transform.localPosition = targetPosition;
			});
		}

		internal static MotionHandle BindToLocalScale(this (Transform Transform, MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Transform, (localScale, transform) => transform.localScale = localScale);
		}
	}
}