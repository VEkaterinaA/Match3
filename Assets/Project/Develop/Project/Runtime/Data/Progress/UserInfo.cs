using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	public sealed class UserInfo : IUserInfo
	{
		[SerializeField]
		private List<String> _progressSlotsIDs = new List<String>();
		[SerializeField]
		private SavedSettings _savedSettings;
		[SerializeField]
		private String _activeProgressID;
		[SerializeField]
		private String _id;

		List<String> IUserInfo.ProgressSlotsIDs => _progressSlotsIDs;

		SavedSettings IUserInfo.SavedSettings => _savedSettings;

		String IUserInfo.ID => _id;

		String IUserInfo.ActiveProgressID
		{
			get => _activeProgressID;
			set
			{
				_activeProgressID = value;

				if (!_progressSlotsIDs.Contains(_activeProgressID))
				{
					_progressSlotsIDs.Add(_activeProgressID);
				}
			}
		}

		internal void GenerateID()
		{
			_id = Guid.NewGuid().ToString();
		}
	}
}