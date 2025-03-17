using Runtime.Data.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtime.Data.Configs.Core
{
	internal interface IProgressConfig
	{
		internal IReadOnlyProgress ProgressPrototype { get; }

		internal void Display(IPersistentProgress persistentProgress, IUserInfo userInfo);

		internal IUserInfo CreateUserInfo();
	}
}
