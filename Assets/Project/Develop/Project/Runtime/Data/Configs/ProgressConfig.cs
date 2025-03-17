using Runtime.Data.Configs.Core;
using Runtime.Data.Progress;
using UnityEngine;

namespace Runtime.Data.Configs
{
	[CreateAssetMenu(menuName = "Data/Configs/Progress", fileName = nameof(ProgressConfig))]
	internal sealed class ProgressConfig : Config, IProgressConfig
	{
		[Header("Runtime")]
		[SerializeField]
		private PersistentProgress _runtimeProgress;
		[SerializeField]
		private UserInfo _runtimeUserInfo;
		[Header("New")]
		[SerializeField]
		private PersistentProgress _playerProgress;
		[SerializeField]
		private UserInfo _userInfo;

		internal PersistentProgress RuntimeProgress
		{
			set => _runtimeProgress = value;
		}

		internal UserInfo RuntimeUserInfo
		{
			set => _runtimeUserInfo = value;
		}

		IReadOnlyProgress IProgressConfig.ProgressPrototype => _playerProgress;

		void IProgressConfig.Display(IPersistentProgress persistentProgress, IUserInfo userInfo)
		{
			_runtimeProgress = (PersistentProgress) persistentProgress;
			_userInfo = (UserInfo) userInfo;
		}

		IUserInfo IProgressConfig.CreateUserInfo()
		{
			var userInfo = Instantiate(this)._userInfo;
			userInfo.GenerateID();

			return userInfo;
		}
	}
}
