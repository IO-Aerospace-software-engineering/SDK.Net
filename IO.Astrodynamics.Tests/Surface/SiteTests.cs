﻿using System;
using IO.Astrodynamics.Body;
using IO.Astrodynamics.Coordinates;
using IO.Astrodynamics.Math;
using IO.Astrodynamics.Mission;
using IO.Astrodynamics.OrbitalParameters;
using IO.Astrodynamics.Surface;
using IO.Astrodynamics.Time;
using Xunit;

namespace IO.Astrodynamics.Tests.Surface
{
    public class SiteTests
    {
        private readonly API _api;

        public SiteTests()
        {
            _api = API.Instance;
            _api.LoadKernels(Constants.SolarSystemKernelPath);
        }

        [Fact]
        public void StateVector()
        {
            var epoch = new DateTime(2000, 1, 1, 12, 0, 0);
            CelestialBody earth = new CelestialBody(399, "earth", 3.986004418E+5, 6356.7519, 6378.1366);
            Site site = new Site(13, "DSS-13", earth);

            var sv = site.GetEphemeris(epoch, earth, Frames.Frame.ICRF, Aberration.None);

            Assert.Equal(
                new StateVector(new Vector3(4113.332255456191, -4876.6144543658074, 1124.8677317992631),
                    new Vector3(0.355608338559514, 0.29994891922262568, -1.2671335428143015E-08), earth, epoch,
                    Frames.Frame.ICRF), sv);
        }


        [Fact]
        public void GetHorizontalCoordinates()
        {
            var epoch = new DateTime(2000, 1, 1, 12, 0, 0);

            Site site = new Site(13, "DSS-13", TestHelpers.GetEarthAtJ2000());
            var hor = site.GetHorizontalCoordinates(epoch, TestHelpers.GetMoonAtJ2000(), Aberration.None);
            Assert.Equal(117.89631806108865, hor.Azimuth *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(16.79061677201462, hor.Elevation *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(400552679.30743355, hor.Range);
        }

        [Fact]
        public void GetHorizontalCoordinates2()
        {
            var epoch = new DateTime(2000, 1, 5, 12, 0, 0);

            Site site = new Site(13, "DSS-13", TestHelpers.GetEarthAtJ2000());
            var hor = site.GetHorizontalCoordinates(epoch, TestHelpers.GetMoonAtJ2000(), Aberration.None);
            Assert.Equal(100.01881371927551, hor.Azimuth *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(-23.23601238553318, hor.Elevation *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(408535095.85180473, hor.Range);
        }

        [Fact]
        public void GetHorizontalCoordinates3()
        {
            var epoch = new DateTime(2000, 1, 10, 12, 0, 0);

            Site site = new Site(13, "DSS-13", TestHelpers.GetEarthAtJ2000());
            var hor = site.GetHorizontalCoordinates(epoch, TestHelpers.GetMoonAtJ2000(), Aberration.None);
            Assert.Equal(41.60830471508871, hor.Azimuth *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(-63.02074114148227, hor.Elevation *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(401248015.68680006, hor.Range);
        }

        [Fact]
        public void GetHorizontalCoordinates4()
        {
            var epoch = new DateTime(2000, 1, 15, 12, 0, 0);

            Site site = new Site(13, "DSS-13", TestHelpers.GetEarthAtJ2000());
            var hor = site.GetHorizontalCoordinates(epoch, TestHelpers.GetMoonAtJ2000(), Aberration.None);
            Assert.Equal(312.5426255803723, hor.Azimuth *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(-33.618934779034475, hor.Elevation *IO.Astrodynamics.Constants.Rad2Deg);
            Assert.Equal(376638211.1106281, hor.Range);
        }
    }
}