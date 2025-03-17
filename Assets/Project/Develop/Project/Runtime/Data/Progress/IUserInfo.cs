using System;
using System.Collections.Generic;

namespace Runtime.Data.Progress
{
	public interface IUserInfo
	{
		public List<String> ProgressSlotsIDs { get; }

		internal SavedSettings SavedSettings { get; }

		internal String ActiveProgressID { get; set; }

		internal String ID { get; }
	}
}