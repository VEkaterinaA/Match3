using LitMotion;
using LitMotion.Adapters;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Extensions.UnityEngine.UIElements
{
	internal static class IStyleExtensions
	{
		internal static (IStyle Style, MotionBuilder<Vector2, NoOptions, Vector2MotionAdapter> MotionBuilder) CreateMotion(this IStyle style, StyleScale startValue, Vector2 endValue, Single duration)
		{
			return (style, LMotion.Create((Vector2) startValue.value.value, endValue, duration));
		}

		internal static MotionHandle BindToTranslate(this (IStyle Style, MotionBuilder<Vector2, NoOptions, Vector2MotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Style, ((translate, style) => style.translate = new Translate(translate.x, translate.y)));
		}

		internal static MotionHandle BindToScale(this (IStyle Style, MotionBuilder<Vector2, NoOptions, Vector2MotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Style, ((scale, style) => style.scale = scale));
		}

		internal static MotionHandle BindToOpacity(this (IStyle Style, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Style, (opacity, style) => style.opacity = opacity);
		}

		internal static MotionHandle BindToBottom(this (IStyle Style, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Style, ((bottom, style) => style.bottom = bottom));
		}

		internal static MotionHandle BindToBackgroundColorAlpha(this (IStyle Style, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Style, ((alpha, style) =>
			{
				var color = new Color(style.backgroundColor.value.r,
									  style.backgroundColor.value.g,
									  style.backgroundColor.value.b,
									  alpha);
				style.backgroundColor = color;
			}));
		}

		internal static void SetBordersColor(this IStyle style, Color targetColor)
		{
			style.borderBottomColor = targetColor;
			style.borderRightColor = targetColor;
			style.borderLeftColor = targetColor;
			style.borderTopColor = targetColor;
		}
	}
}