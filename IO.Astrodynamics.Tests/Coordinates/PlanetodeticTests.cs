// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using IO.Astrodynamics.Coordinates;
using Xunit;

namespace IO.Astrodynamics.Tests.Coordinates;

public class PlanetodeticTests
{
    public PlanetodeticTests()
    {
        API.Instance.LoadKernels(Constants.SolarSystemKernelPath);
    }

    [Fact]
    public void Planetocentric()
    {
        Planetodetic planetodetic = new Planetodetic(-116.79445837 * Astrodynamics.Constants.Deg2Rad, 35.24716450 * Astrodynamics.Constants.Deg2Rad, 1070.85);
        Planetocentric planetocentric = planetodetic.ToPlanetocentric(TestHelpers.EarthAtJ2000.Flatenning, TestHelpers.EarthAtJ2000.EquatorialRadius);
        Assert.Equal(35.06601815, planetocentric.Latitude * Astrodynamics.Constants.Rad2Deg,2);
        Assert.Equal(-116.79445837, planetocentric.Longitude * Astrodynamics.Constants.Rad2Deg,3);
        Assert.Equal(6372125.09695, planetocentric.Radius);
    }
}