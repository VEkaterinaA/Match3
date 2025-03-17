using System;
using LitMotion;
using LitMotion.Adapters;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	internal static class AnimationCurveExtensions
	{
		internal static MotionHandle BindToValue(this (AnimationCurve AnimationCurve, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder, Action<Single> onEvaluate)
		{
			return builder.MotionBuilder.BindWithState(builder.AnimationCurve, ((value, animationCurve) => onEvaluate?.Invoke(animationCurve.Evaluate(value))));
		}
	}
}