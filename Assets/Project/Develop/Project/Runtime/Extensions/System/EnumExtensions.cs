using System;

namespace Runtime.Extensions.System
{
	internal static class EnumExtensions
	{
		internal static Type ConvertToType(this Enum targetEnum)
		{
			return targetEnum switch
			{
				_ => throw new NotImplementedException($"[{nameof(ConvertToType)}] There is not implementation to convert [{targetEnum.GetType().Name}].{targetEnum} to type.")
			};
		}
	}
}