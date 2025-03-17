using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	internal static class ObjectExtensions
	{
		internal static TObject OrNull<TObject>(this TObject unityObject) where TObject : Object
		{
			return (unityObject ? unityObject : null);
		}
	}
}