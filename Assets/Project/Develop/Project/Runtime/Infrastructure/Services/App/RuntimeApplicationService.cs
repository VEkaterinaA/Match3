using Runtime.Infrastructure.Services.App.Core;
using UnityEngine.Device;

namespace Runtime.Infrastructure.Services.App
{
	internal sealed class RuntimeApplicationService : ApplicationService
	{
		protected override void RequestQuit()
		{
			Application.Quit();
		}
	}
}