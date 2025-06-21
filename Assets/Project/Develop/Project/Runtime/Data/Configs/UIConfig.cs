using AYellowpaper.SerializedCollections;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums;
using UnityEngine;

namespace Runtime.Data.Configs
{
	internal interface IUIConfig
	{
		internal SerializedDictionary<TargetType, Texture2D> TargetTextures { get; }
	}

	[CreateAssetMenu(menuName = "Data/Configs/UI", fileName = nameof(UIConfig))]
	internal class UIConfig : Config, IUIConfig
	{
		[SerializeField]
		SerializedDictionary<TargetType, Texture2D> _targetTextures = new();

		SerializedDictionary<TargetType, Texture2D> IUIConfig.TargetTextures => _targetTextures;
	}
}
