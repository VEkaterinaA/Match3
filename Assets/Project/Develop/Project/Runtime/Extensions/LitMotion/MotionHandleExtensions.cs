using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

namespace Runtime.Extensions.LitMotion
{
	internal static class MotionHandleExtensions
	{
		internal static void CancelIfActive(this MotionHandle motionHandle)
		{
			if (motionHandle.IsActive())
			{
				motionHandle.Cancel();
			}
		}

		internal static void AddAutoRemove(this CompositeMotionHandle composite, MotionHandle motion)
		{
			composite.Add(motion);

			motion.ToUniTask()
				  .ContinueWith(() =>
				  {
					  composite.Remove(motion);
				  }).Forget();
		}
	}
}