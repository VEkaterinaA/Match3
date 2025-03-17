using UnityEngine;

namespace Runtime.Data.ScriptableObjects.AudioSource
{
	internal abstract class AudioSourceData : ScriptableObject
	{
		internal abstract void ApplyTo(UnityEngine.AudioSource audioSource);
	}
}