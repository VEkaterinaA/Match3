using System;
using UnityEngine;

namespace Runtime.Extensions.UnityEngine
{
	internal static class ComponentExtensions
	{
		internal static String GetSceneName(this Component component)
		{
			return component.gameObject.scene.name;
		}
	}
}