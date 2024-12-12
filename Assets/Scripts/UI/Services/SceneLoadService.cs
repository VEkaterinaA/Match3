using Cysharp.Threading.Tasks;
using UI.Interfaces;
using UnityEngine.AddressableAssets;

namespace UI.Services
{
	public class SceneLoadService : ISceneLoadService
	{
		private const string LevelSceneAssetKey = "GameScene";
		private const string MenuSceneAssetKey = "StartMenuScene";

		public void LoadLevelScene()
		{
			LoadSceneAsync(LevelSceneAssetKey).Forget();
		}

		public void LoadMenuScene()
		{
			LoadSceneAsync(MenuSceneAssetKey).Forget();
		}

		private async UniTaskVoid LoadSceneAsync(string sceneKey)
		{
			var handler = Addressables.LoadSceneAsync(sceneKey);
			await UniTask.WaitUntil(() => handler.IsDone);
		}
	}
}