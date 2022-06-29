using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Maths;
using ConsoleApp1;
using Demo;
using Geolocation.Common.Extensions;
using Geolocation.Domain.Services.Repositories.Volatile;
using Geolocation.Domain.Core.Model.Satellites;
using Geolocation.Common.Serialization;
using Geolocation.Common.Pinger;
using Geolocation.Common.GeoRegions;
using Geolocation.Common.Maths;
using Geolocation.Domain.Core.Repositories;

namespace Demo
{
    class Program
    {
        public static VolatileRepository FillOriginalRepository(VolatileRepository originalRepository)
        {
            originalRepository.Satellites.Save(new Satellite(Guid.NewGuid(), "1001011", 1111, "Among 10", 20));
            originalRepository.Satellites.Save(new Satellite(Guid.NewGuid(), "1001101", 2110, "Eutelsat 11", 20));
            originalRepository.Satellites.Save(new Satellite(Guid.NewGuid(), "0000000", 1, "Amos", 10));
            return originalRepository;
        }

        public static VolatileRepository FillRepositoryWithBeams(VolatileRepository originalRepository)
        {
            var satellite1 = new Satellite(Guid.NewGuid(), "1001011", 1111, "Among 10", 20);
            var satellite2 = new Satellite(Guid.NewGuid(), "1001101", 2110, "Eutelsat 11", 20);
            var satellite3 = new Satellite(Guid.NewGuid(), "0000000", 1, "Amos", 10);

            Guid a = Guid.NewGuid();
            Guid b = Guid.NewGuid();
            CoverageArea coverageArea1 = new CoverageArea(Guid.NewGuid(), "Area1", new OverallGeoRegion());
            CoverageArea coverageArea2 = new CoverageArea(Guid.NewGuid(), "Area2", new OverallGeoRegion());
            satellite1.AddBeam(a, "Beam 1", Geolocation.Common.Signals.Polarization.CircularLeft, coverageArea1);
            satellite1.AddBeam(b, "Beam 2", Geolocation.Common.Signals.Polarization.Horizontal, coverageArea2);
            satellite1.AddTransponder(Guid.NewGuid(), "Trans1", a, b, new IntRange(1, 100), new IntRange(101, 200), 5.5);

            Guid c = Guid.NewGuid();
            Guid d = Guid.NewGuid();
            CoverageArea coverageArea3 = new CoverageArea(Guid.NewGuid(), "Area3", new OverallGeoRegion());
            CoverageArea coverageArea4 = new CoverageArea(Guid.NewGuid(), "Area4", new OverallGeoRegion());
            satellite2.AddBeam(c, "Beam 1", Geolocation.Common.Signals.Polarization.CircularLeft, coverageArea3);
            satellite2.AddBeam(d, "Beam 2", Geolocation.Common.Signals.Polarization.Horizontal, coverageArea4);
            satellite2.AddTransponder(Guid.NewGuid(), "Trans1", c, d, new IntRange(1, 100), new IntRange(101, 200), 5.5);

            Guid e = Guid.NewGuid();
            Guid f = Guid.NewGuid();
            Guid coverageAreaId = Guid.NewGuid();
            CoverageArea uniqueCoverageArea = new CoverageArea(coverageAreaId, "UniqueArea", new OverallGeoRegion());
            satellite3.AddBeam(e, "Beam 1", Geolocation.Common.Signals.Polarization.CircularLeft, uniqueCoverageArea);
            satellite3.AddBeam(f, "Beam 2", Geolocation.Common.Signals.Polarization.Horizontal, uniqueCoverageArea);
            satellite3.AddTransponder(Guid.NewGuid(), "Trans1", e, f, new IntRange(1, 100), new IntRange(101, 200), 5.5);
            
            ActionsWithSatellites.CompareBeams(satellite1, satellite2);
            originalRepository.Satellites.Save(satellite1);
            originalRepository.Satellites.Save(satellite2);
            originalRepository.Satellites.Save(satellite3);

            originalRepository.CoverageAreas.Save(coverageArea1);
            originalRepository.CoverageAreas.Save(coverageArea2);
            originalRepository.CoverageAreas.Save(coverageArea3);
            originalRepository.CoverageAreas.Save(coverageArea4);
            originalRepository.CoverageAreas.Save(uniqueCoverageArea);
            return originalRepository;
        }

        public static void ShowRepo(VolatileRepository originalRepository)
        {
            foreach (var satellite in originalRepository.Satellites.GetAll().ToArray())
            {
                Console.WriteLine("Name: " + satellite.Name);
                Console.WriteLine("Internationl code: " + satellite.InternationalCode);
                Console.WriteLine("SubSatellitePoint: " + satellite.SubSatellitePoint);
                Console.WriteLine("Norad: " + satellite.Norad);
                Console.WriteLine("ID: " + satellite.Id);
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            ISerializer serializer = new BinarySerializer();
            // Создаём репозиторий со спутниками и заполняем его
            var originalRepository = FillRepositoryWithBeams(new VolatileRepository());

            // Создаём (ReadOnly) массив спутников, передавая данные из репозитория
            var satellitesMemo1 = new SatellitesMemo(originalRepository.Satellites.GetAll());

            // Создаём data1, где храним сериализуемый массив спутников и satellites1 с массивом, благодаря которому мы сможем выводить
            // данные в консоль
            var data1 = serializer.Serialize(satellitesMemo1);

            // Вывод в консоль первого репозитория
            Console.WriteLine("originalRepository:\n");
            ShowRepo(originalRepository);

            // Записываем в файл сериализованные данные
            System.IO.File.WriteAllBytes("satellite.dat", data1);
            // Записываем эти данные в data2 (для дальнейшего восстановаления)
            var data2 = System.IO.File.ReadAllBytes("satellite.dat");

            // Сохраняем в массив десириализованные данные
            // Десериализуем спутники из файла satellite.dat и сохраняем их в destinationRepository
            var satellitesMemo2 = serializer.Deserialize<SatellitesMemo>(data2);

            // Создаём новый репозиторий для хранения окончательных данных
            var destinationRepository = new VolatileRepository();

            satellitesMemo2.UnpackInto(destinationRepository);
            destinationRepository.Satellites.GetAll();
            Console.WriteLine("destinationRepository:\n");
            ShowRepo(destinationRepository);

            //
            foreach (var satellite in originalRepository.Satellites.GetAll().ToArray())
            {
                foreach (var beam in satellite.Beams)
                {
                    Console.WriteLine(beam.Area.Name);
                }
            }

            Console.WriteLine();

            foreach (var satellite in destinationRepository.Satellites.GetAll().ToArray())
            {
                foreach (var beam in satellite.Beams)
                {
                    Console.WriteLine(beam.Area.Name);
                }
            }

            Console.WriteLine();

            foreach (var ca in originalRepository.CoverageAreas.GetAll().ToArray())
            {
                Console.WriteLine(ca.Name);
            }

            Console.WriteLine();

            foreach (var ca in destinationRepository.CoverageAreas.GetAll().ToArray())
            {
                Console.WriteLine(ca.Name);
            }
            //


            /*
            ISerializer serializer = new BinarySerializer();

            //CoverageAreaMemo coverageArea = new CoverageAreaMemo(Guid.NewGuid(), "a", new OverallGeoRegion());
			foreach (var region in new IGeoRegion[]
			{   new OverallGeoRegion(), 
				new SinglePointGeoRegion(new GeographicalCoordinates(60, 30, 0)),
				new BoundingAreaGeoRegion(new BoundingArea(
					new GeographicalCoordinates(60, 30, 0),
					new GeographicalCoordinates(61, 30, 0),
					new GeographicalCoordinates(61, 31, 0),
					new GeographicalCoordinates(60, 31, 0))
				)
			})
			{
                var memoOriginal = new GeoRegionMemo(region);
				var data = serializer.Serialize(memoOriginal);
				var memoDest = serializer.Deserialize<GeoRegionMemo>(data);
				var regionDest = memoDest.GetRegion();

				Console.WriteLine(region.GetType() + " " + regionDest.GetType());

				if (region is BoundingAreaGeoRegion boundingAreaGeoRegion)
				{
					if (!(regionDest is BoundingAreaGeoRegion destBoundingAreaGeoRegion))
					{
						Console.WriteLine("Invalid region on deserialization!");
					}
					else
					{
						var isSequenceEqual = boundingAreaGeoRegion.Area.Points.SequenceEqual(destBoundingAreaGeoRegion.Area.Points);
						Console.WriteLine(isSequenceEqual);
					}
				}
			}
            */
        }
    }
}