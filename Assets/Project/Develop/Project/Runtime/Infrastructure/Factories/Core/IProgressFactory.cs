using Runtime.Data.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime.Infrastructure.Factories.Core
{
	internal interface IProgressFactory
	{
		internal IPersistentProgress CreatePlayerProgress();

		internal IUserInfo CreateUserInfo();
	}
}
