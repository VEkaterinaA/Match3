using LitMotion;
using LitMotion.Adapters;
using System;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	internal static class MaterialExtensions
	{
		internal static MotionHandle BindToAlpha(this (Material Material, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Material, ((alpha, material) =>
			{
				var targetColor = material.color;
				targetColor.a = alpha;

				material.color = targetColor;
			}));
		}

		internal static MotionHandle BindToAlpha(this (SpriteRenderer Material, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Material, ((alpha, material) =>
			{
				var targetColor = material.color;
				targetColor.a = alpha;

				material.color = targetColor;
			}));
		}

		internal static MotionHandle BindToFloat(this (Material Material, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder, Int32 shaderPropertyID)
		{
			return builder.MotionBuilder.BindWithState(builder.Material, ((value, material) => material.SetFloat(shaderPropertyID, value)));
		}

		/*		internal static MotionHandle BindToVignetteIntensity(this (Vignette Vignette, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
				{
					return builder.MotionBuilder.BindWithState(builder.Vignette, ((value, vignette) => vignette.intensity = new ClampedFloatParameter(value,0F,1F,true)));
				}*/

		internal static MotionHandle BindToColor(this (Material Material, MotionBuilder<Color, NoOptions, ColorMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.Material, ((color, material) => material.color = color));
		}

		internal static MotionHandle BindToColor(this (Light light, MotionBuilder<Color, NoOptions, ColorMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.light, ((value, light) => light.color = value));
		}

		/*		internal static MotionHandle BindToColor(this (ColorParameter ColorParameter, MotionBuilder<Color, NoOptions, ColorMotionAdapter> MotionBuilder) builder)
				{
					return builder.MotionBuilder.BindWithState(builder.ColorParameter, ((color, colorParameter) => colorParameter.value = color));
				}*/

		internal static MotionHandle BindToValue(this (ParticleSystem particleSystem, MotionBuilder<Int32, IntegerOptions, IntMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.particleSystem, ((value, particleSystem) =>
			{
				var main = particleSystem.main;
				main.maxParticles = value;
			}));
		}

		internal static MotionHandle BindToColorA(this (SpriteRenderer spriteRenderer, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) builder)
		{
			return builder.MotionBuilder.BindWithState(builder.spriteRenderer, (alpha, renderer) =>
			{
				var targetColor = renderer.color;
				targetColor.a = alpha;
				renderer.color = targetColor;
			});
		}
	}
}