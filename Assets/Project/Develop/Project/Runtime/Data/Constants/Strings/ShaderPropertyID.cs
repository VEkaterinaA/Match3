using System;
using UnityEngine;
using Object = System.Object;

namespace Runtime.Data.Constants.Strings
{
	internal static class ShaderPropertyID
	{
		internal static readonly Int32 Intensity = Shader.PropertyToID($"_{nameof(Intensity)}");
	}
}