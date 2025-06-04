using System;
using System.Collections.Generic;

namespace Runtime.Data.Progress
{
	public interface IUserInfo
	{
		internal Settings Settings { get; }
		internal PlayerStats PlayerStats { get; }
	}
}