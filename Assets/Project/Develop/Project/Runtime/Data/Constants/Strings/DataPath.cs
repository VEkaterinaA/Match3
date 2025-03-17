using System;
using System.IO;
using UnityEngine;

namespace Runtime.Data.Constants.Strings
{
	public static class DataPath
	{
		public static readonly String UserInfo = Path.Combine(Application.persistentDataPath, "User.info");

		public static String GetForPlayerProgressWithID(String progressSlotID)
		{
			return Path.Combine(Application.persistentDataPath, $"{progressSlotID.Replace(" ", "")}Progress.game");
		}
	}
}