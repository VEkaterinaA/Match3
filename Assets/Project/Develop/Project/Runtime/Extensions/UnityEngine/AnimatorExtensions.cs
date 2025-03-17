using System;
using LitMotion;
using LitMotion.Adapters;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	internal static class AnimatorExtensions
	{
		internal static MotionHandle BindToSpeed(this (Animator UnityAnimator, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.UnityAnimator, ((speed, animator) => animator.speed = speed));
		}
	}
}