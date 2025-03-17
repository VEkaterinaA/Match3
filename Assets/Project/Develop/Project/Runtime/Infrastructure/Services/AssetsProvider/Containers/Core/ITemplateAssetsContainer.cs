using UnityEngine.UIElements;

namespace Runtime.Infrastructure.Services.AssetsProvider.Containers.Core
{
	internal interface ITemplateAssetsContainer<TEnum> : IAssetsContainer<TEnum>
	{
		internal VisualTreeAsset this[TEnum templateID] { get; }
	}
}