using Runtime.Data.Configs.Core;
using Runtime.Data.Progress;
using Runtime.Infrastructure.Factories.Core;
using VContainer;

namespace Runtime.Infrastructure.Factories
{
	internal sealed class ProgressFactory : IProgressFactory
	{
		private IObjectResolver _objectResolver;
		private IProgressConfig _progressConfig;

		[Inject]
		internal void Construct(IObjectResolver objectResolver, IProgressConfig progressConfig)
		{
			_objectResolver = objectResolver;
			_progressConfig = progressConfig;
		}

		IPersistentProgress IProgressFactory.CreatePlayerProgress()
		{
			var playerProgress = _progressConfig.ProgressPrototype.Clone();
			_objectResolver.Inject(playerProgress);

			return playerProgress;
		}

		IUserInfo IProgressFactory.CreateUserInfo()
		{
			var userInfo = _progressConfig.CreateUserInfo();
			_objectResolver.Inject(userInfo);

			return userInfo;
		}
	}
}
