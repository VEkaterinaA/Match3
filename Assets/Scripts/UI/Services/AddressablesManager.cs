using UnityEngine.AddressableAssets;

namespace UI.Services
{
	public class AddressablesManager
	{
		public AddressablesManager()
		{
			Addressables.InitializeAsync();
		}
	}
}
