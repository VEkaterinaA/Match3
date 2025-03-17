using System;
using LitMotion;
using LitMotion.Adapters;
using UnityEngine.UIElements;

namespace Runtime.Extensions.UnityEngine.UIElements
{
	internal static class ProgressBarExtensions
	{
		internal static MotionHandle BindToValue(this (ProgressBar ProgressBar, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.ProgressBar, ((value, style) => style.value = value));
		}
	}
}