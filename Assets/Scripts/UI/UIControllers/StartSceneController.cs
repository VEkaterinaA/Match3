using UI.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.UIControllers
{

	public class StartSceneController : MonoBehaviour
	{
		[Inject]
		private async void Construct(ISceneLoadService sceneLoadService)
		{
			sceneLoadService.LoadMenuScene();
		}
	}
}
