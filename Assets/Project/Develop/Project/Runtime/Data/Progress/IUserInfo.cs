using Runtime.Infrastructure.Core;
using System;

namespace Runtime.Data.Progress
{
	public interface IUserInfo : IPrototype<UserInfo>
	{
		public Settings Settings { get; }
		public PlayerStats PlayerStats { get; }

		public void ScoreCheck(Int32 value);
	}
}