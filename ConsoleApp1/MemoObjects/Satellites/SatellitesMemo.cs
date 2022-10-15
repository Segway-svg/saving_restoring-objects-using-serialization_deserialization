using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;
using Geolocation.Common.Serialization;
using Geolocation.Domain.Core.Model.Satellites;
using Geolocation.Domain.Core.Repositories;
using Geolocation.Domain.Services.Repositories.Volatile;

namespace Demo
{
    [Serializable]
    // public?!1
    public class SatellitesMemo
    {
        SatelliteMemo[] satellites;
        CoverageAreaMemo[] coverageAreas;

        public SatellitesMemo(IReadOnlyList<Satellite> satellites)
        {
            this.satellites = new SatelliteMemo[satellites.Count];
            for (int i = 0; i < satellites.Count; i++)
            {
                this.satellites[i] = new SatelliteMemo(satellites[i]);
            }

            var areas = CreateHashAreas(satellites);
            coverageAreas = areas.Select(a => new CoverageAreaMemo(a)).ToArray();
        }

        public HashSet<CoverageArea> CreateHashAreas(IReadOnlyList<Satellite> satellites)
        {
            HashSet<CoverageArea> areas = new HashSet<CoverageArea>();
            foreach (var satellite in satellites)
            {
                foreach (var beam in satellite.Beams)
                {
                    areas.Add(beam.Area);
                }
            }
            return areas;
        }

        public void UnpackInto(IRepository repository)
        {
            for (int i = 0; i < this.coverageAreas.Length; i++)
            {
                if (!repository.CoverageAreas.TryFind(coverageAreas[i].id, out _))
                {
                    repository.CoverageAreas.Save(coverageAreas[i].GetCoverageArea());
                }
            }

            for (int i = 0; i < this.satellites.Length; i++)
            { 
                satellites[i].UnpackIntoRepository(repository);
            }
        }


        //public IReadOnlyList<Satellite> GetSatellites()
        //{
        //    var arr = new Satellite[this.satellites.Length];

        //    for (int i = 0; i < satellites.Length; i++)
        //    {
        //        arr[i] = satellites[i].GetSatellite();
        //    }
        //    return arr;
        //}
    }
}
