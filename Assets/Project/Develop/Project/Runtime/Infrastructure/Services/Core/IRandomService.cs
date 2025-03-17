using System;

namespace Runtime.Infrastructure.Services.Core
{
	internal interface IRandomService
	{
		internal Single Next(Single minInclusive, Single maxExclusive);

		internal Int32 Next(Int32 minInclusive, Int32 maxExclusive);

		internal String CreateGUID(params String[] takenGUIDs);
	}
}
