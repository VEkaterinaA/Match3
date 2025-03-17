using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Runtime.Data.ScriptableObjects.AudioSource
{
	[CreateAssetMenu(menuName = "Data/Audio Sources/Full", fileName = nameof(AudioSourceFullData), order = 1)]
	internal sealed class AudioSourceFullData : AudioSourceData
	{
		[SerializeField]
		private AudioMixerGroup _output;

		[Header("Common Settings")] [SerializeField]
		private Boolean _bypassEffects;
		[SerializeField]
		private Boolean _bypassListenerEffects;
		[SerializeField]
		private Boolean _bypassReverbZones;
		[SerializeField] [Range(0, 256)]
		private Int32 _priority = 128;

		[SerializeField] [Range(-3.0f, 3.0f)]
		private Single _pitch = 1.0f;
		[SerializeField] [Range(-1.0f, 1.0f)]
		private Single _stereoPan;
		[SerializeField] [Range(0.0f, 1.0f)]
		private Single _spatialBlend;
		[SerializeField] [Range(0.0f, 1.1f)]
		private Single _reverbZoneMix = 1.0f;

		[Header("3D Sound Settings")] [SerializeField] [Range(0.0f, 5.0f)]
		private Single _dopplerLevel = 1.0f;
		[SerializeField] [Range(0.0f, 360.0f)]
		private Single _spread;
		[SerializeField]
		private AudioRolloffMode _volumeRolloff;
		[SerializeField] [Min(0.0f)]
		private Single _minDistance = 1.0f;
		[SerializeField] [Min(0.0f)]
		private Single _maxDistance = 500.0f;

		[Header("Additional Settings")] [SerializeField]
		private Boolean _spatializePostEffects;
		[SerializeField]
		private Boolean _ignoreListenerVolume;
		[SerializeField]
		private Boolean _ignoreListenerPause;

		internal override void ApplyTo(UnityEngine.AudioSource audioSource)
		{
			audioSource.bypassListenerEffects = _bypassListenerEffects;
			audioSource.bypassReverbZones = _bypassReverbZones;
			audioSource.outputAudioMixerGroup = _output;
			audioSource.bypassEffects = _bypassEffects;
			audioSource.spatializePostEffects = _spatializePostEffects;
			audioSource.ignoreListenerVolume = _ignoreListenerVolume;
			audioSource.ignoreListenerPause = _ignoreListenerPause;
			audioSource.spatialize = (_spatialBlend > 0.0f);
			audioSource.reverbZoneMix = _reverbZoneMix;
			audioSource.dopplerLevel = _dopplerLevel;
			audioSource.spatialBlend = _spatialBlend;
			audioSource.rolloffMode = _volumeRolloff;
			audioSource.minDistance = _minDistance;
			audioSource.maxDistance = _maxDistance;
			audioSource.panStereo = _stereoPan;
			audioSource.priority = _priority;
			audioSource.spread = _spread;

			audioSource.pitch = _pitch;
		}
	}
}