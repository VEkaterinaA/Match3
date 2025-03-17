using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Constants.Integers
{
	internal sealed class AnimatorStatesHashes<TEnumState> where TEnumState : Enum
	{
		private readonly Dictionary<Int32, TEnumState> _statesHashes;

		internal AnimatorStatesHashes()
		{
			var enumStates = (TEnumState[])Enum.GetValues(typeof(TEnumState));

			_statesHashes = new Dictionary<Int32, TEnumState>(enumStates.Length);

			foreach (var enumState in enumStates)
			{
				_statesHashes.Add(Animator.StringToHash(enumState.ToString()), enumState);
			}
		}

		internal Int32 this[TEnumState enumState] => _statesHashes.FirstOrDefault(pair => pair.Value.Equals(enumState)).Key;

		internal TEnumState this[Int32 shortNameHash] => _statesHashes[shortNameHash];
	}
}