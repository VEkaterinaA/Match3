using LitMotion;

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
	}
}