﻿using System;
using IO.Astrodynamics.Body;
using IO.Astrodynamics.Coordinates;
using IO.Astrodynamics.Math;
using IO.Astrodynamics.Mission;
using IO.Astrodynamics.OrbitalParameters;
using IO.Astrodynamics.SolarSystemObjects;
using IO.Astrodynamics.Time;
using Xunit;

namespace IO.Astrodynamics.Tests.Coordinates
{
    public class EquatorialTests
    {
        public EquatorialTests()
        {
            API.Instance.LoadKernels(Constants.SolarSystemKernelPath);
        }

        [Fact]
        public void Create()
        {
            Equatorial eq = new Equatorial(1.0, 2.0, 3.0);
            Assert.Equal(1.0, eq.Declination);
            Assert.Equal(2.0, eq.RightAscencion);
            Assert.Equal(3.0, eq.Distance);
        }

        [Fact]
        public void CreateFromStateVector()
        {
            Astrodynamics.Mission.Mission mission = new Astrodynamics.Mission.Mission("mission1");
            new Scenario("scn1", mission, new Window(new DateTime(2021, 1, 1), new DateTime(2021, 1, 2)));
            var epoch = DateTime.MinValue;
            CelestialBody earth = new CelestialBody(PlanetsAndMoons.EARTH);
            Equatorial eq = new Equatorial(new StateVector(
                new Vector3(-291608.38463344, -266716.83339423, -76102.48709990), new Vector3(), earth, epoch,
                Frames.Frame.ICRF));
            Assert.Equal(new Equatorial(-0.19024413568211371, 3.8824377884371972, 402448.63988732797), eq);
        }

        [Fact]
        public void CreateFromStateVector2()
        {
            var epoch = new DateTime(2021, 1, 1);

            var moon = TestHelpers.Moon;
            var earth = moon.InitialOrbitalParameters.Observer;
            var eq = new Equatorial(new StateVector(
                new Vector3(-202831.34150844064, 284319.70678317308, 150458.88140126597),
                new Vector3(-0.48702480142667454, -0.26438331399030518, -0.17175837261637006), earth, epoch,
                Frames.Frame.ICRF));
            Assert.Equal(new Equatorial(0.406773808779999, 2.1904536325374035, 380284.26703704614), eq);
        }
    }
}