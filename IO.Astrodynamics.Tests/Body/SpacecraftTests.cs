using System;
using IO.Astrodynamics.Models.Body.Spacecraft;
using Xunit;

namespace IO.Astrodynamics.Models.Tests.Body
{
    public class SpacecraftTests
    {
        [Fact]
        public void Create()
        {
            Clock clk = new Clock("My clock", 1.0 / 256.0);
            Spacecraft spc = new Spacecraft(-1001, "MySpacecraft", 1000.0, 10000.0);
            Assert.Equal(-1001, spc.NaifId);
            Assert.Equal("MySpacecraft", spc.Name);
            Assert.Equal(1000.0, spc.DryOperatingMass);
            Assert.Equal(10000.0, spc.MaximumOperatingMass);
        }

        [Fact]
        public void CreateExceptions()
        {
            Clock clk = new Clock("My clock", 1.0 / 256.0);
            Assert.Throws<ArgumentException>(() => new Spacecraft(-1001, "", 1000.0, 10000.0));
            Assert.Throws<ArgumentException>(() => new Spacecraft(-1001, "MySpacecraft", 0.0, 10000.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Spacecraft(-1001, "MySpacecraft", 1000.0, 999.0));
        }
    }
}