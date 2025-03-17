using System;
using LitMotion;
using LitMotion.Adapters;
using UnityEngine;
using MotionHandle = LitMotion.MotionHandle;
using NoOptions = LitMotion.NoOptions;

namespace Runtime.Extensions.UnityEngine
{
	internal static class LightExtensions
	{
		internal static MotionHandle BindToIntensity(this (Light Light, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Light, ((intensity, light) => light.intensity = intensity));
		}
	}
}