using Runtime.Infrastructure.Core;
using System;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	public sealed class UserInfo : IUserInfo
	{
		[SerializeField]
		public Settings _settings;

		[SerializeField]
		public PlayerStats _playerStats;

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

		void IUserInfo.ScoreCheck(Int32 value)
		{
			if(_playerStats.Highest_score < value)
			{
				_playerStats.Highest_score = value;
			}
		}

		UserInfo IPrototype<UserInfo>.Clone()
		{
			var userInfo = new UserInfo();

			userInfo._settings = ((IPrototype<Settings>) _settings).Clone();
			userInfo._playerStats = ((IPrototype<PlayerStats>) _playerStats).Clone();

			return userInfo;
		}

	}
}