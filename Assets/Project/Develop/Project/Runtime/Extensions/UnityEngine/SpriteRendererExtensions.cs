using System;
using LitMotion;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	public static class SpriteRendererExtensions
	{
		public static MotionHandle Fade(this SpriteRenderer spriteRenderer, Single startAlpha, Single endAlpha, Single duration, params SpriteRenderer[] linkedSpriteRenderers)
		{
			return LMotion.Create(startAlpha, endAlpha, duration).Bind(alpha =>
			{
				var targetColor = spriteRenderer.color;
				targetColor.a = alpha;

				spriteRenderer.color = targetColor;

				foreach (var renderer in linkedSpriteRenderers)
				{
					renderer.color = targetColor;
				}
			});
		}

		public static MotionHandle Fade(this SpriteRenderer spriteRenderer, Single endAlpha, Single duration, params SpriteRenderer[] linkedSpriteRenderers)
		{
			return spriteRenderer.Fade(spriteRenderer.color.a, endAlpha, duration, linkedSpriteRenderers);
		}

		public static void SetAlpha(this SpriteRenderer spriteRenderer, Single alpha)
		{
			var targetColor = spriteRenderer.color;
			targetColor.a = alpha;

			spriteRenderer.color = targetColor;
		}
	}
}