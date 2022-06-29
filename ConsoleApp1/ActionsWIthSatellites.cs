using Geolocation.Domain.Core.Model.Satellites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geolocation.Domain.Core.Services.Satellites.Validations;

namespace Demo
{
    static class ActionsWithSatellites
    {
        public static void ShowSatellites(IReadOnlyList<Satellite> satellites)
        {
            for (int i = 0; i < satellites.Count; i++)
            {
                Console.WriteLine($"ID: {satellites[i].Id}");
                Console.WriteLine($"InternationalCode: {satellites[i].InternationalCode}");
                Console.WriteLine($"Norad: {satellites[i].Norad}");
                Console.WriteLine($"Name: {satellites[i].Name}");
                Console.WriteLine($"SubSatellitePoint: {satellites[i].SubSatellitePoint}");
                foreach (var beam in satellites[i].Beams)
                {
                    Console.WriteLine("BeamName: " + beam.Name);
                }
                Console.WriteLine();
            }
        }

        public static void CompareSatellites(IReadOnlyList<Satellite> satellites1, IReadOnlyList<Satellite> satellites2)
        {
            var biggerArr = satellites1;
            var lowerArr = satellites2;
            if (satellites1.Count < satellites2.Count)
            {
                biggerArr = satellites2;
                lowerArr = satellites1;
            }
            foreach (var t in biggerArr)
            {
                bool isCorrect = false;
                foreach (var t1 in lowerArr)
                {
                    if (t.Id != t1.Id) continue;
                    if (t.InternationalCode == t1.InternationalCode
                        && t.Norad == t1.Norad
                        && t.Name == t1.Name
                        && t.SubSatellitePoint == t1.SubSatellitePoint)
                    {
                        isCorrect = true;
                    }
                }
                if (!isCorrect)
                {
                    Console.WriteLine($"\nThere is no {t.Name} satellite!\n");
                }
            }
        }

        // TODO Сравнивать лучи и транспондеры
        public static void CompareBeams(Satellite satellite1, Satellite satellite2)
        {
            foreach (var sat1 in satellite1.Beams)
            {
                foreach (var sat2 in satellite2.Beams)
                {
                    if (sat1.Name == sat2.Name &&
                        sat1.Polarization == sat2.Polarization &&
                        sat1.FullName == sat2.FullName &&
                        sat1.Id == sat2.Id &&
                        sat1.Area.Id == sat2.Area.Id &&
                        sat1.Area.Region == sat2.Area.Region &&
                        sat1.Area.Name == sat2.Area.Name)
                    {
                        Console.WriteLine("Beams are equal!");
                    }
                }
            }
        }

        public static void CompareTransponders(Satellite satellite1, Satellite satellite2)
        {
            foreach (var sat1 in satellite1.Transponders)
            {
                foreach (var sat2 in satellite2.Transponders)
                {
                    if (sat1.Name == sat2.Name &&
                        sat1.Id == sat2.Id &&
                        sat1.Delay == sat2.Delay &&
                        sat1.UplinkBeam.Id == sat2.UplinkBeam.Id &&
                        sat1.DownlinkBeam.Id == sat2.DownlinkBeam.Id &&
                        sat1.UplinkFrequency.Start == sat2.UplinkFrequency.Start &&
                        sat1.UplinkFrequency.End == sat2.UplinkFrequency.End &&
                        sat1.DownlinkFrequency.Start == sat2.DownlinkFrequency.Start &&
                        sat1.DownlinkFrequency.End == sat2.DownlinkFrequency.End)
                    {
                        Console.WriteLine("Transponders are equal!");
                    }
                }
            }
        }
    }
}
