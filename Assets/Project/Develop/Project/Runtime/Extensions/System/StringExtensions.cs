using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = System.Object;

namespace Runtime.Extensions.System
{
	public static class StringExtensions
	{
		private static readonly StringBuilder _stringBuilder = new StringBuilder();

		public static String SplitUpperCase(this String inputString)
		{
			var regex = new Regex(" (?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
			return regex.Replace(inputString, " ");
		}

		public static Boolean IsNullOrEmpty(this String targetString)
		{
			return ((targetString == null) || (targetString.Length == 0));
		}

		public static String Encrypt(this String data, String encryptionKey)
		{
			_stringBuilder.Clear();

			for (var charIndex = 0; charIndex < data.Length; charIndex++)
			{
				_stringBuilder.Append((Char)(data[charIndex] ^ encryptionKey[charIndex % encryptionKey.Length]));
			}

			return _stringBuilder.ToString();
		}

		public static TObject Deserialize<TObject>(this String json)
		{
			return JsonUtility.FromJson<TObject>(json);
		}

		public static void Deserialize(this String json, Object systemObject)
		{
			JsonUtility.FromJsonOverwrite(json, systemObject);
		}
	}
}