using System;

namespace Runtime.Data.Progress
{
	public interface IUserInfo
	{
		internal Settings Settings { get; }
		internal PlayerStats PlayerStats { get; }

		internal void ScoreCheck(Int32 value);
	}
}