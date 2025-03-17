using Runtime.Infrastructure.Services.Core;
using System;
using System.Linq;

namespace Runtime.Infrastructure.Services
{
	internal sealed class SystemRandomService : IRandomService
	{
		private readonly Random _random = new Random((Int32) DateTime.Now.TimeOfDay.TotalSeconds);

		Single IRandomService.Next(Single minInclusive, Single maxExclusive)
		{
			return (minInclusive + ((Single) _random.NextDouble() / maxExclusive));
		}

		Int32 IRandomService.Next(Int32 minInclusive, Int32 maxExclusive)
		{
			return _random.Next(minInclusive, maxExclusive);
		}

		String IRandomService.CreateGUID(params String[] takenGUIDs)
		{
			var guid = Guid.NewGuid().ToString();

			for (var attemptIndex = 0; (attemptIndex < 1000); attemptIndex++)
			{
				if (takenGUIDs.Contains(guid))
				{
					guid = Guid.NewGuid().ToString();
				}
				else
				{
					return guid;
				}
			}

			throw new Exception($"[{GetType().Name}] There have been too many attempts to create GUID");
		}
	}
}
