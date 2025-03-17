using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	public static class MonoBehaviourExtensions
	{
		public static void DeactivateGameObject(this MonoBehaviour monoBehaviour)
		{
			monoBehaviour.gameObject.SetActive(false);
		}

		public static void ActivateGameObject(this MonoBehaviour monoBehaviour)
		{
			monoBehaviour.gameObject.SetActive(true);
		}
	}
}