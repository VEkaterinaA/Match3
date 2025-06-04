using Runtime.Data.Configs.Core;
using Runtime.Data.Progress;
using Runtime.Infrastructure.Factories.Core;
using VContainer;

namespace Runtime.Infrastructure.Factories
{
	internal sealed class DataFactory : IDataFactory
	{
		private IObjectResolver _objectResolver;
		private IProgressConfig _progressConfig;

		[Inject]
		internal void Construct(IObjectResolver objectResolver, IProgressConfig progressConfig)
		{
			_objectResolver = objectResolver;
			_progressConfig = progressConfig;
		}


		IUserInfo IDataFactory.CreateUserInfo()
		{
			var userInfo = _progressConfig.CreateUserInfo();
			_objectResolver.Inject(userInfo);

			return userInfo;
		}
	}
}
