using System;

namespace Runtime.Extensions.System
{
	internal static class EnumExtensions
	{
		private static readonly Random _random = new();


		internal static Type ConvertToType(this Enum targetEnum)
		{
			return targetEnum switch
			{
				_ => throw new NotImplementedException($"[{nameof(ConvertToType)}] There is not implementation to convert [{targetEnum.GetType().Name}].{targetEnum} to type.")
			};
		}

		internal static T GetRandomValue<T>() where T : Enum
		{
			var values = Enum.GetValues(typeof(T));
			return (T) values.GetValue(_random.Next(values.Length));
		}

		internal static Int32 Lenght<T>() where T : Enum
		{
			return Enum.GetValues(typeof(T)).Length;
		}
	}
}