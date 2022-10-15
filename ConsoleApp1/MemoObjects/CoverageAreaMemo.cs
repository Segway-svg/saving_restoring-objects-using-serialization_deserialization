using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Demo;
using Geolocation.Common.GeoRegions;
using Geolocation.Domain.Core.Model.Satellites;

namespace ConsoleApp1
{
    [Serializable]
    internal class CoverageAreaMemo
    {
        public Guid id;
        public string name;
        public GeoRegionMemo geoRegion;

        public CoverageAreaMemo(CoverageArea coverageArea)
        {
            id = coverageArea.Id;
            name = coverageArea.Name;
            geoRegion = new GeoRegionMemo(coverageArea.Region);
        }

        public CoverageArea GetCoverageArea()
        {
            var coverageArea = new CoverageArea(id, name, geoRegion.GetRegion());
            return coverageArea;
        }
    }
}
