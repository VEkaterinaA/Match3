using System;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	public sealed class UserInfo : IUserInfo
	{
		[SerializeField]
		private Settings _settings;

		[SerializeField]
		private PlayerStats _playerStats;

		Settings IUserInfo.Settings
		{
			get
			{
				return _settings;
			}
		}

		PlayerStats IUserInfo.PlayerStats
		{
			get
			{
				return _playerStats;
			}
		}
		public UserInfo()
		{
			_settings = new Settings();
			_playerStats = new PlayerStats();
		}
	}
}