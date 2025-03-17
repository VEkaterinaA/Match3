using Runtime.Infrastructure.Core;
using LitMotion;
using LitMotion.Adapters;
using System;
using UnityEngine;
using Object = System.Object;

namespace Runtime.Extensions.System
{
	internal static class ObjectExtensions
	{
		internal static void InvokeAfterInitialization(this Object initializationInformer, Action onInitialized)
		{
#if UNITY_EDITOR || DEBUG
			if (initializationInformer is not IInitializationInformer)
			{
				Debug.LogWarning($"[{nameof(InvokeAfterInitialization)}] {onInitialized.Method.Name} Invoked: {initializationInformer.GetType().Name} do not implement {nameof(IInitializationInformer)} interface.");
			}
#endif

			if ((initializationInformer is IInitializationInformer informer) && (!informer.IsInitialized))
			{
				informer.Initialized += onInitialized;
			}
			else
			{
				onInitialized?.Invoke();
			}
		}

		internal static void InvokeAfterAllInitializations(this Object systemObject, Action onInitialized, params Object[] initializationInformers)
		{
			var count = 0;

			foreach (var initializationInformer in initializationInformers)
			{
				if ((initializationInformer is IInitializationInformer informer) && (!informer.IsInitialized))
				{
					count++;

					informer.Initialized += () =>
					{
						count--;
						TryInvoke();
					};
				}
			}

			TryInvoke();

			return;

			void TryInvoke()
			{
				if (count == 0)
				{
					onInitialized?.Invoke();
				}
			}
		}

		internal static Boolean IsInitialized(this Object systemObject)
		{
#if UNITY_EDITOR || DEBUG
			if (systemObject is not IInitializationInformer)
			{
				Debug.LogWarning($"[{nameof(InvokeAfterInitialization)}] True returned: {systemObject.GetType().Name} do not implement {nameof(IInitializationInformer)} interface.");
			}
#endif

			return ((systemObject is not IInitializationInformer initializationInformer) || (initializationInformer.IsInitialized));
		}

		internal static String Serialize(this Object systemObject, Boolean usePrettyPrint = false)
		{
			return JsonUtility.ToJson(systemObject, usePrettyPrint);
		}

		internal static (TObject SystemObject, MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> MotionBuilder) CreateMotion<TObject>(this TObject systemObject, Vector3 startValue, Vector3 endValue, Single duration)
		{
			return (systemObject, LMotion.Create(startValue, endValue, duration));
		}

		internal static (TObject SystemObject, MotionBuilder<Vector2, NoOptions, Vector2MotionAdapter> MotionBuilder) CreateMotion<TObject>(this TObject systemObject, Vector2 startValue, Vector2 endValue, Single duration)
		{
			return (systemObject, LMotion.Create(startValue, endValue, duration));
		}

		internal static (TObject SystemObject, MotionBuilder<Single, NoOptions, FloatMotionAdapter> MotionBuilder) CreateMotion<TObject>(this TObject systemObject, Single startValue, Single endValue, Single duration)
		{
			return (systemObject, LMotion.Create(startValue, endValue, duration));
		}

		internal static (TObject SystemObject, MotionBuilder<Int32, IntegerOptions, IntMotionAdapter> MotionBuilder) CreateMotionInt<TObject>(this TObject systemObject, Int32 startValue, Int32 endValue, Single duration)
		{
			return (systemObject, LMotion.Create(startValue, endValue, duration));
		}

		internal static (TObject SystemObject, MotionBuilder<Quaternion, NoOptions, QuaternionMotionAdapter> MotionBuilder) CreateMotion<TObject>(this TObject systemObject, Quaternion startValue, Quaternion endValue, Single duration)
		{
			return (systemObject, LMotion.Create(startValue, endValue, duration));
		}

		internal static (TObject SystemObject, MotionBuilder<Color, NoOptions, ColorMotionAdapter> MotionBuilder) CreateMotion<TObject>(this TObject systemObject, Color startValue, Color endValue, Single duration)
		{
			return (systemObject, LMotion.Create(startValue, endValue, duration));
		}
	}

}
