using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo;
using Geolocation.Common.Extensions;
using Geolocation.Domain.Core.Model.Satellites;
using Geolocation.Domain.Core.Services.Satellites;

namespace Demo
{
    [Serializable]
    internal class TransponderMemo
    {
        public Guid id;

        public string name;

        public Guid uplinkBeamID;

        public Guid downlinkBeamID;

        public IntRange uplinkFrequency;

        public IntRange downlinkFrequency;

        public double delay;
        public TransponderMemo(Transponder transponder)
        {
            id = transponder.Id;
            name = transponder.Name;
            uplinkBeamID = transponder.UplinkBeam.Id;
            downlinkBeamID = transponder.DownlinkBeam.Id;
            uplinkFrequency = transponder.UplinkFrequency;
            downlinkFrequency = transponder.DownlinkFrequency;
            delay = transponder.Delay;
        }

        public void UnpackInto(Satellite satellite)
        {
            satellite.AddTransponder(id, name, uplinkBeamID, downlinkBeamID, uplinkFrequency, downlinkFrequency, delay);
        }
    }
}
