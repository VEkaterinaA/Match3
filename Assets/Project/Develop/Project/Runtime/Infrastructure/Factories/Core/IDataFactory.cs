using Runtime.Data.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime.Infrastructure.Factories.Core
{
	internal interface IDataFactory
	{
		internal IUserInfo CreateUserInfo();
	}
}
