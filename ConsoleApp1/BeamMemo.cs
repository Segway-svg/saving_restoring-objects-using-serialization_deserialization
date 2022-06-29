using System;
using System.Linq;
using ConsoleApp1;
using Geolocation.Common.GeoRegions;
using Geolocation.Common.Signals;
using Geolocation.Domain.Core.Model.Satellites;
using Geolocation.Domain.Core.Repositories;

namespace Demo
{
	[Serializable]
	internal class BeamMemo
	{
		public Guid id;
		public string name;
		public Polarization polarization;
        public Guid coverageId;

        public BeamMemo(Beam beam)
		{
			id = beam.Id;
			name = beam.Name;
			polarization = beam.Polarization;
            coverageId = beam.Area.Id;
        }

		public void UnpackInto(Satellite satellite, ICoverageAreasRepository coverageAreasRepository)
		{
			if (coverageAreasRepository.TryFind(coverageId, out var coverageArea))
            {
                satellite.AddBeam(id, name, polarization, coverageArea);
			}
            else
            {
                throw new Exception($"There is no beam with coverageId: {coverageId}");
            }
        }
    }
}