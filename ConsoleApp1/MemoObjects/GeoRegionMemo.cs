using System;
using System.Linq;
using Base.Utils.Maths;
using Geolocation.Common.GeoRegions;
using Geolocation.Common.Maths;
using Geolocation.Domain.Core.Services.Satellites.Filters;
using JetBrains.Annotations;

namespace Demo
{
	[Serializable]
	public class GeoRegionMemo
    {
		public GeographicalCoordinates[] coordinates;

        public GeoRegionMemo(IGeoRegion region)
		{
            coordinates = MemoCoordinatesGetter.GetMemoCoordinates(region);
        }

		public IGeoRegion GetRegion()
		{
			if (coordinates.Length == 0)
			{
				return new OverallGeoRegion();
			}

            if (coordinates.Length == 1)
            {
                return new SinglePointGeoRegion(coordinates[0]);
            }

			return new BoundingAreaGeoRegion(new BoundingArea(coordinates));
		}

		#region Nested

        public class MemoCoordinatesGetter : IGeoRegionVisitor
		{
            private GeographicalCoordinates[] coordinates;

            public static GeographicalCoordinates[] GetMemoCoordinates(IGeoRegion region)
            {
                var result = new MemoCoordinatesGetter();
                region.Visit(result);
                return result.coordinates;
            }

			public void Visit(SinglePointGeoRegion region)
			{
                coordinates = new GeographicalCoordinates[1];
                coordinates[0] = region.Point;
            }

			public void Visit(BoundingAreaGeoRegion region)
			{
                coordinates = region.Area.Points.ToArray();
            }

			public void Visit(OverallGeoRegion region)
			{
                coordinates = new GeographicalCoordinates[]{};
            }
		}
        #endregion
	}
}