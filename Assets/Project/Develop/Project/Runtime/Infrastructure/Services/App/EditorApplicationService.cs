using Runtime.Infrastructure.Services.App.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Runtime.Infrastructure.Services.App
{
	internal sealed class EditorApplicationService : ApplicationService
	{
		protected override void RequestQuit()
		{
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#endif
		}
	}
}