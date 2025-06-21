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
		private UserInfo _runtimeUserInfo;
		[Header("New")]
		[SerializeField]
		private UserInfo _userInfo;

		internal UserInfo RuntimeUserInfo
		{
			set => _runtimeUserInfo = value;
		}

		void IProgressConfig.Display(IUserInfo userInfo)
		{
			_userInfo = (UserInfo) userInfo;
		}

		IUserInfo IProgressConfig.CreateUserInfo()
		{
			var userInfo = Instantiate(this)._userInfo;

			return userInfo;
		}
	}
}
