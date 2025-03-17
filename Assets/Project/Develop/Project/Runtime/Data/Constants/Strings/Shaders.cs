using UnityEngine;
using Object = System.Object;

namespace Runtime.Data.Constants.Strings
{
	public static class Shaders
	{
		public static readonly Shader UIGaussianBlur = Shader.Find(ShaderName.UIGaussianBlur);
		public static readonly Shader Vignette = Shader.Find(ShaderName.Vignette);
		public static readonly Shader Diffuse = Shader.Find(ShaderName.Diffuse);
	}
}