using System;
using IO.Astrodynamics.Models.Mission;
using IO.Astrodynamics.Models.SeedWork;

namespace IO.Astrodynamics.Models.Body.Spacecraft
{
    public class SpacecraftFuelTank : Entity
    {
        public SpacecraftScenario Spacecraft { get; private set; }
        public FuelTank FuelTank { get; private set; }
        public double InitialQuantity { get; private set; }
        public double Quantity { get; private set; }

        private SpacecraftFuelTank() { }
        public SpacecraftFuelTank(SpacecraftScenario spacecraft, FuelTank fuelTank, double quantity)
        {
            if (spacecraft == null)
            {
                throw new ArgumentException("SpacecraftFuelTank requires a spacecraft");
            }

            if (fuelTank == null)
            {
                throw new ArgumentException("SpacecraftFuelTank requires a fuel tank");
            }

            if (quantity < 0.0)
            {
                throw new ArgumentException("Fuel quantity must be positive");
            }

            if (quantity > fuelTank.Capacity)
            {
                throw new ArgumentException("Fuel quantity must be lower or equal to fuel tank capacity");
            }

            Spacecraft = spacecraft;
            FuelTank = fuelTank;
            Quantity = InitialQuantity = quantity;
        }

        public void BurnFuel(double quantity)
        {
            if (quantity > Quantity)
            {
                throw new InvalidOperationException($"Not enought fuel in tank {FuelTank.Name}");
            }

            Quantity -= quantity;
        }
    }
}