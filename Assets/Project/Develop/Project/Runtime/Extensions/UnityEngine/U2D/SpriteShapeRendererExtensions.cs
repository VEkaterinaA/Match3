using System;
using LitMotion;
using UnityEngine.U2D;

namespace Runtime.Extensions.UnityEngine.U2D
{
	public static class SpriteShapeRendererExtensions
	{
		public static MotionHandle Fade(this SpriteShapeRenderer spriteShapeRenderer, Single startAlpha, Single endAlpha, Single duration, params SpriteShapeRenderer[] linkedSpriteShapeRenderers)
		{
			return LMotion.Create(startAlpha, endAlpha, duration).Bind(alpha =>
			{
				var targetColor = spriteShapeRenderer.color;
				targetColor.a = alpha;

				spriteShapeRenderer.color = targetColor;

				foreach (var renderer in linkedSpriteShapeRenderers)
				{
					renderer.color = targetColor;
				}
			});
		}

		public static MotionHandle Fade(this SpriteShapeRenderer spriteShapeRenderer, Single endAlpha, Single duration, params SpriteShapeRenderer[] linkedSpriteShapeRenderers)
		{
			return spriteShapeRenderer.Fade(spriteShapeRenderer.color.a, endAlpha, duration, linkedSpriteShapeRenderers);
		}
	}
}