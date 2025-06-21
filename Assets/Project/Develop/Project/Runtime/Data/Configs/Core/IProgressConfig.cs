using Runtime.Data.Progress;

namespace Runtime.Data.Configs.Core
{
	internal interface IProgressConfig
	{
		internal IUserInfo UserInfo { get; }

		internal void Display(IUserInfo userInfo);

		internal IUserInfo CreateUserInfo();
	}
}
