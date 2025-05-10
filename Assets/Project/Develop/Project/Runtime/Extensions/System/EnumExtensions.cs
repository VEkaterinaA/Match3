using Runtime.Data.Constants.Enums;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using System;
using System.Collections.Generic;

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

		internal static List<T> GetValuesList<T>() where T : Enum
		{
			return new List<T>((T[]) Enum.GetValues(typeof(T)));
		}

		internal static List<string> GetNamesList<T>() where T : Enum
		{
			return new List<string>(Enum.GetNames(typeof(T)));
		}

		internal static PrefabType ConvertToPrefabType(StoneType oldStatus)
		{
			switch (oldStatus)
			{
				case StoneType.RedStone:
					return PrefabType.RedStone;
				case StoneType.BlueStone:
					return PrefabType.BlueStone;
				case StoneType.GreenStone:
					return PrefabType.GreenStone;
				case StoneType.YellowStone:
					return PrefabType.YellowStone;
				default:
					return PrefabType.Unknown;
			}
		}

		internal static TargetType ConvertToTargetType(StoneType oldStatus)
		{
			switch (oldStatus)
			{
				case StoneType.RedStone:
					return TargetType.RedStone;
				case StoneType.BlueStone:
					return TargetType.BlueStone;
				case StoneType.GreenStone:
					return TargetType.GreenStone;
				case StoneType.YellowStone:
					return TargetType.YellowStone;
				default:
					return TargetType.Unknown;

			}
		}

		internal static TargetType ConvertToTargetType(PrefabType oldStatus)
		{
			switch (oldStatus)
			{
				case PrefabType.RedStone:
					return TargetType.RedStone;
				case PrefabType.BlueStone:
					return TargetType.BlueStone;
				case PrefabType.GreenStone:
					return TargetType.GreenStone;
				case PrefabType.YellowStone:
					return TargetType.YellowStone;
				case PrefabType.RadiusBomb:
					return TargetType.ActivateBooster;
				case PrefabType.VerticalBomb:
					return TargetType.ActivateBooster;
				case PrefabType.HorizontalBomb:
					return TargetType.ActivateBooster;
				default:
					return TargetType.Unknown;

			}
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