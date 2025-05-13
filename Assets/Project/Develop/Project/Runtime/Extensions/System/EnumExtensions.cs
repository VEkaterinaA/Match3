using Runtime.Data.Constants.Enums;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using System;
using System.Collections.Generic;

namespace Runtime.Extensions.System
{
	internal static class EnumExtensions
	{
		private static readonly Random _random = new();


		internal static List<T> GetValuesList<T>() where T : Enum
		{
			return new List<T>((T[]) Enum.GetValues(typeof(T)));
		}

		internal static List<string> GetNamesList<T>() where T : Enum
		{
			return new List<string>(Enum.GetNames(typeof(T)));
		}

		internal static PrefabType ConvertToPrefabType(CellType oldStatus)
		{
			return oldStatus switch
			{
				CellType.RedStone => PrefabType.RedStone,
				CellType.BlueStone => PrefabType.BlueStone,
				CellType.GreenStone => PrefabType.GreenStone,
				CellType.RadiusBomb => PrefabType.RadiusBomb,
				CellType.YellowStone => PrefabType.YellowStone,
				CellType.VerticalBomb => PrefabType.VerticalBomb,
				CellType.HorizontalBomb => PrefabType.HorizontalBomb,

				_ => PrefabType.Unknown,
			};
		}


		internal static TargetType ConvertToTargetType(CellType oldStatus)
		{
			return oldStatus switch
			{
				CellType.RedStone => TargetType.RedStone,
				CellType.BlueStone => TargetType.BlueStone,
				CellType.GreenStone => TargetType.GreenStone,
				CellType.RadiusBomb => TargetType.YellowStone,
				CellType.YellowStone => TargetType.YellowStone,
				CellType.VerticalBomb => TargetType.YellowStone,
				CellType.HorizontalBomb => TargetType.YellowStone,
				_ => TargetType.Unknown,
			};
		}

		internal static TargetType ConvertToTargetType(PrefabType oldStatus)
		{
			return oldStatus switch
			{
				PrefabType.RedStone => TargetType.RedStone,
				PrefabType.BlueStone => TargetType.BlueStone,
				PrefabType.GreenStone => TargetType.GreenStone,
				PrefabType.YellowStone => TargetType.YellowStone,
				PrefabType.RadiusBomb => TargetType.ActivateBooster,
				PrefabType.VerticalBomb => TargetType.ActivateBooster,
				PrefabType.HorizontalBomb => TargetType.ActivateBooster,
				_ => TargetType.Unknown,
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