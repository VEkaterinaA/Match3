using System;
using LitMotion;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Runtime.Extensions.UnityEngine.Rendering.Universal
{
	internal static class Light2DExtensions
	{
		internal static MotionHandle SetPointLightRadii(this Light2D light2D, Vector2 startRadii, Vector2 endRadii, Single duration)
		{
			return LMotion.Create(startRadii, endRadii, duration).Bind(UpdateRadii);

			void UpdateRadii(Vector2 radii)
			{
				light2D.pointLightInnerRadius = radii.x;
				light2D.pointLightOuterRadius = radii.y;
			}
		}

		internal static MotionHandle SetPointLightRadii(this Light2D light2D, Vector2 endRadii, Single duration)
		{
			var startRadii = new Vector2(light2D.pointLightInnerRadius, light2D.pointLightOuterRadius);

			return light2D.SetPointLightRadii(startRadii, endRadii, duration);
		}

		internal static MotionHandle SetPointLightAngles(this Light2D light2D, Vector2 startAngles, Vector2 endAngles, Single duration)
		{
			return LMotion.Create(startAngles, endAngles, duration).Bind(UpdateAngles);

			void UpdateAngles(Vector2 angles)
			{
				light2D.pointLightInnerAngle = angles.x;
				light2D.pointLightOuterAngle = angles.y;
			}
		}

		internal static MotionHandle SetPointLightAngles(this Light2D light2D, Vector2 endAngles, Single duration)
		{
			var startRadii = new Vector2(light2D.pointLightInnerAngle, light2D.pointLightOuterAngle);

			return light2D.SetPointLightAngles(startRadii, endAngles, duration);
		}

		internal static MotionHandle SetPointLightInnerRadius(this Light2D light2D, Single startRadius, Single endRadius, Single duration)
		{
			return LMotion.Create(startRadius, endRadius, duration).Bind(radius => light2D.pointLightInnerRadius = radius);
		}

		internal static MotionHandle SetPointLightInnerRadius(this Light2D light2D, Single endRadius, Single duration)
		{
			return light2D.SetPointLightInnerRadius(light2D.pointLightInnerRadius, endRadius, duration);
		}

		internal static MotionHandle SetPointLightOuterRadius(this Light2D light2D, Single startRadius, Single endRadius, Single duration)
		{
			return LMotion.Create(startRadius, endRadius, duration).Bind(radius => light2D.pointLightOuterRadius = radius);
		}

		internal static MotionHandle SetPointLightOuterRadius(this Light2D light2D, Single endRadius, Single duration)
		{
			return light2D.SetPointLightOuterRadius(light2D.pointLightOuterRadius, endRadius, duration);
		}

		internal static MotionHandle SetPointLightInnerAngle(this Light2D light2D, Single startAngle, Single endAngle, Single duration)
		{
			return LMotion.Create(startAngle, endAngle, duration).Bind(angle => light2D.pointLightInnerAngle = angle);
		}

		internal static MotionHandle SetPointLightInnerAngle(this Light2D light2D, Single endAngle, Single duration)
		{
			return light2D.SetPointLightInnerAngle(light2D.pointLightInnerAngle, endAngle, duration);
		}

		internal static MotionHandle SetPointLightOuterAngle(this Light2D light2D, Single startAngle, Single endAngle, Single duration)
		{
			return LMotion.Create(startAngle, endAngle, duration).Bind(angle => light2D.pointLightOuterAngle = angle);
		}

		internal static MotionHandle SetPointLightOuterAngle(this Light2D light2D, Single endAngle, Single duration)
		{
			return light2D.SetPointLightOuterAngle(light2D.pointLightOuterAngle, endAngle, duration);
		}

		internal static MotionHandle AnimateIntensity(this Light2D light2D, Single startIntensity, Single endIntensity, Single duration, Action onComplete = null, Action onCancel = null)
		{
			return LMotion.Create(startIntensity, endIntensity, duration).WithOnComplete(onComplete).WithOnCancel(onCancel).Bind(intensity => light2D.intensity = intensity);
		}

		internal static MotionHandle AnimateIntensity(this Light2D light2D, Single endIntensity, Single duration, Action onComplete = null, Action onCancel = null)
		{
			return light2D.AnimateIntensity(light2D.intensity, endIntensity, duration, onComplete, onCancel);
		}
	}
}