using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.MonoBehaviours.Game.Core;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Infrastructure.Services.Game.Helper
{
	internal class BoosterCreator
	{
		public List<(int x, int y, CellType type)> GetBoostersToCreate(List<List<IBoardItem>> allMatches)
		{
			var horizontalMatches = new Dictionary<IBoardItem, List<IBoardItem>>();
			var verticalMatches = new Dictionary<IBoardItem, List<IBoardItem>>();

			foreach (var match in allMatches)
			{
				var isHorizontal = match.All(c => c.Y == match[0].Y);
				var isVertical = match.All(c => c.X == match[0].X);

				if (isHorizontal)
				{
					foreach (var cell in match)
					{
						if (!horizontalMatches.ContainsKey(cell))
							horizontalMatches[cell] = new List<IBoardItem>();

						horizontalMatches[cell].AddRange(match);
					}
				}
				else if (isVertical)
				{
					foreach (var cell in match)
					{
						if (!verticalMatches.ContainsKey(cell))
							verticalMatches[cell] = new List<IBoardItem>();

						verticalMatches[cell].AddRange(match);
					}
				}
			}

			var boosterCenters = new HashSet<IBoardItem>();
			var boostersToCreate = new List<(int x, int y, CellType type)>();

			foreach (var cell in horizontalMatches.Keys)
			{
				if (boosterCenters.Contains(cell)) continue;

				var horizontalMatch = horizontalMatches[cell].Distinct().ToList();
				var isThreeOrMoreHorizontal = horizontalMatch.Count >= 3;
				var isFiveOrMoreHorizontal = horizontalMatch.Count >= 5;
				
				var verticalMatch = horizontalMatch
					.Where(c => verticalMatches.ContainsKey(c))
					.SelectMany(c => verticalMatches[c])
					.Distinct()
					.ToList();
				var hasVertical = verticalMatch.Count >= 3;

				if ((hasVertical && verticalMatch.Count >= 3 && isThreeOrMoreHorizontal) || isFiveOrMoreHorizontal)
				{
					var intersect = horizontalMatch.Intersect(verticalMatch).FirstOrDefault();
					if (intersect != null && !boosterCenters.Contains(intersect))
					{
						boostersToCreate.Add((intersect.X, intersect.Y, CellType.RadiusBomb));
						boosterCenters.Add(intersect);
					}
				}
				else if (horizontalMatch.Count == 4)
				{
					var center = GetCenterItem(horizontalMatch);
					if (!boosterCenters.Contains(center))
					{
						boostersToCreate.Add((center.X, center.Y, CellType.HorizontalBomb));
						boosterCenters.Add(center);
					}
				}
			}

			foreach (var cell in verticalMatches.Keys)
			{
				if (boosterCenters.Contains(cell)) continue;

				var verticalMatch = verticalMatches[cell].Distinct().ToList();
				var isFourVertical = verticalMatch.Count >= 4;
				var isFiveVertical = verticalMatch.Count >= 5;

				if(isFiveVertical)
				{
					var center = GetCenterItem(verticalMatch);
					if (!boosterCenters.Contains(center))
					{
						boostersToCreate.Add((center.X, center.Y, CellType.RadiusBomb));
						boosterCenters.Add(center);
					}

				}
				else if (isFourVertical)
				{
					var center = GetCenterItem(verticalMatch);
					if (!boosterCenters.Contains(center))
					{
						boostersToCreate.Add((center.X, center.Y, CellType.VerticalBomb));
						boosterCenters.Add(center);
					}
				}
			}

			return boostersToCreate;
		}

		private IBoardItem GetCenterItem(List<IBoardItem> match)
		{
			var sorted = match.OrderBy(i => i.Y).ThenBy(i => i.X).ToList();
			return sorted[sorted.Count / 2];
		}

	}
}
