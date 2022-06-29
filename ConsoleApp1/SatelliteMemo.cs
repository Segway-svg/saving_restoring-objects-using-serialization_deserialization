using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geolocation.Domain.Core.Model.Satellites;
using Geolocation.Domain.Core.Repositories;
using Geolocation.Domain.Services.Repositories.Volatile;

namespace Demo
{
    [Serializable]
    internal class SatelliteMemo
    {
        public Guid id;
        string internationalCode;
        long? norad;
        string name;
        double subSatellitePoint;

        private BeamMemo[] beams;

        private TransponderMemo[] transponders;

        public SatelliteMemo(Satellite satellite)
        {
            this.id = satellite.Id;
            this.internationalCode = satellite.InternationalCode;
            this.norad = satellite.Norad;
            this.name = satellite.Name;
            this.subSatellitePoint = satellite.SubSatellitePoint;

            //beams = satellite.Beams.Select(b => new BeamMemo(b)).ToArray();

            beams = new BeamMemo[satellite.Beams.Count];
            var beamsArray = satellite.Beams.ToArray();
            for (var i = 0; i < beams.Length; i++)
            {
                beams[i] = new BeamMemo(beamsArray[i]);
            }

            transponders = new TransponderMemo[satellite.Transponders.Count];
            var transpondersArray = satellite.Transponders.ToArray();
            for (var i = 0; i < transponders.Length; i++)
            {
                transponders[i] = new TransponderMemo(transpondersArray[i]);
            }
        }

        public void UnpackIntoRepository(IRepository repository)
        {
            var satellite = new Satellite(id, internationalCode, norad, name, subSatellitePoint);
            foreach (var beam in beams)
            {
                beam.UnpackInto(satellite, repository.CoverageAreas);
            }

            foreach (var transponder in transponders)
            {
                transponder.UnpackInto(satellite);
            }

            repository.Satellites.Save(satellite);
        }
    }
}
