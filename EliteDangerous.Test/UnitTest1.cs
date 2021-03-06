﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EliteDangerous.Test
{
    using System.Linq;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodProof()
        {
            var vectorSystemTest = new System.Numerics.Vector3(1, 2, 3);

            var vectorSystemFrom = new System.Numerics.Vector3(0, 1, 1);
            var vectorSystemTo = new System.Numerics.Vector3(1, 0, 2);
            var vectorDirection = vectorSystemTo - vectorSystemFrom; //1, -1, 1

            var factor = GetFactorVectorDirection(vectorSystemTest, vectorSystemFrom, vectorDirection);

            var vectorFusspunkt = vectorSystemFrom + factor * vectorDirection;

            var distance = (vectorFusspunkt - vectorSystemTest).Length();
        }

        [TestMethod]
        public void TestMethodElite()
        {
            var vectorSystemTest = new System.Numerics.Vector3(-26.90625f, -10.5625f, 2.90625f);

            var vectorSystemFrom = new System.Numerics.Vector3(-21.90625f, 8.125f, 9f);
            var vectorSystemTo = new System.Numerics.Vector3(-28.5625f, -2.09375f, 8.5f);
            var vectorDirection = vectorSystemTo - vectorSystemFrom;

            var factor = GetFactorVectorDirection(vectorSystemTest, vectorSystemFrom, vectorDirection);

            var vectorFusspunkt = vectorSystemFrom + factor * vectorDirection;

            var distance = (vectorFusspunkt - vectorSystemTest).Length();
        }

        [TestMethod]
        public void TestMethodEliteStarSystems()
        {
            var starSystemsAll = CsvHandler.GetAllSystems(ReadCsvTest.CSV_FILE_PATH);

            var vectorSystemFrom = starSystemsAll.First(s => s.Name.Equals("Sol")).Location;
            var vectorSystemTo = starSystemsAll.First(s => s.Name.Equals("Colonia")).Location;

            var starSystemsInRange = new System.Collections.Generic.List<StarSystem>();

            System.Threading.Tasks.Parallel.For(0, starSystemsAll.Count(), f =>
            {
                var starSystem = starSystemsAll.ElementAt(f);

                var vectorDirection = vectorSystemTo - vectorSystemFrom;

                var factor = GetFactorVectorDirection(starSystem.Location, vectorSystemFrom, vectorDirection);

                var vectorFusspunkt = vectorSystemFrom + factor * vectorDirection;

                var distance = (vectorFusspunkt - starSystem.Location).Length();

                if (factor >= 0 && factor <= 1 && distance < 2)
                { starSystemsInRange.Add(starSystem); }
            });
        }

        [TestMethod]
        public void PathBetweenSystems()
        {
            var databaseHandler = new MySqlHandler("localhost", 3307, "root", "k83Wd3/5*");

            float maximumJumpRange = 56.84f;
            var starSystemsAll = databaseHandler.GetSystemsScoopable().ToList();

            var systemFrom = databaseHandler.GetSystem("Sagittarius A*");
            var systemTo = databaseHandler.GetSystem("Hyroks");

            if (!starSystemsAll.Any(s => s.Id == systemFrom.Id)) { starSystemsAll.Add(systemFrom); }
            if (!starSystemsAll.Any(s => s.Id == systemTo.Id)) { starSystemsAll.Add(systemTo); }

            var starSystemsInRange = new System.Collections.Generic.List<StarSystem>();

            System.Threading.Tasks.Parallel.For(0, starSystemsAll.Count(), f =>
            {
                var starSystem = starSystemsAll.ElementAt(f);

                var vectorDirection = systemTo.Location - systemFrom.Location;

                var factor = GetFactorVectorDirection(starSystem.Location, systemFrom.Location, vectorDirection);

                var vectorFusspunkt = systemFrom.Location + factor * vectorDirection;

                var distance = (vectorFusspunkt - starSystem.Location).Length();

                if (factor >= 0 && factor <= 1 && distance < 50)
                { starSystemsInRange.Add(starSystem); }
            });

            using (var stream = new System.IO.FileStream(@"C:\Users\Adrian\Downloads\elite\test.csv", System.IO.FileMode.Create))
            using (var streamWriter = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8))
            {
                var nextSystem = systemFrom;

                //streamWriter.WriteLine(nextSystem);

                while (!nextSystem.Name.Equals(systemTo.Name))
                {
                    var systemClosestToTarget = starSystemsInRange.Where(s => s != null && System.Numerics.Vector3.Distance(nextSystem.Location, s.Location) < maximumJumpRange).OrderBy(s => System.Numerics.Vector3.Distance(systemTo.Location, s.Location)).First();

                    streamWriter.WriteLine("From:{0}, To:{1}, Distance: {2}", nextSystem, systemClosestToTarget, System.Numerics.Vector3.Distance(nextSystem.Location, systemClosestToTarget.Location));
                    
                    nextSystem = systemClosestToTarget;
                }
            }   
        }

        private static float GetFactorVectorDirection(System.Numerics.Vector3 vectorPoint, System.Numerics.Vector3 vectorFrom, System.Numerics.Vector3 vectorDirection)
        {
            return (-vectorPoint.X * vectorDirection.X + vectorFrom.X * vectorDirection.X - vectorPoint.Y * vectorDirection.Y + vectorFrom.Y * vectorDirection.Y - vectorPoint.Z * vectorDirection.Z + vectorFrom.Z * vectorDirection.Z) / (-(vectorDirection.X*vectorDirection.X) - (vectorDirection.Y * vectorDirection.Y) - (vectorDirection.Z * vectorDirection.Z));
        }
    }
}