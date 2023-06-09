using System;
using System.Collections.Generic;
using System.Linq;
using IO.Astrodynamics.Frames;
using IO.Astrodynamics.Math;
using IO.Astrodynamics.OrbitalParameters;


namespace IO.Astrodynamics.Body.Spacecraft
{
    public class Spacecraft : Body
    {
        public static readonly Vector3 Front = Vector3.VectorY;
        public static readonly Vector3 Back = Front.Inverse();
        public static readonly Vector3 Right = Vector3.VectorX;
        public static readonly Vector3 Left = Right.Inverse();
        public static readonly Vector3 Up = Vector3.VectorZ;
        public static readonly Vector3 Down = Up.Inverse();

        public Maneuver.Maneuver StandbyManeuver { get; private set; }
        public Spacecraft Parent { get; private set; }
        public Spacecraft Child { get; private set; }
        public Clock Clock { get; private set; }

        private readonly HashSet<Instrument> _instruments = new();
        public IReadOnlyCollection<Instrument> Intruments => _instruments;

        private readonly HashSet<FuelTank> _fuelTanks = new();
        public IReadOnlyCollection<FuelTank> FuelTanks => _fuelTanks;

        private readonly HashSet<Engine> _engines = new();
        public IReadOnlyCollection<Engine> Engines => _engines;

        private readonly HashSet<Payload> _payloads = new();
        public IReadOnlyCollection<Payload> Payloads => _payloads;
        public double DryOperatingMass => Mass;

        public double MaximumOperatingMass { get; }
        public Frame Frame { get; }

        public Spacecraft(int naifId, string name, double mass, double maximumOperatingMass, Clock clock, OrbitalParameters.OrbitalParameters initialOrbitalParameters) : base(
            naifId, name, mass, initialOrbitalParameters)
        {
            if (maximumOperatingMass < mass) throw new ArgumentOutOfRangeException(nameof(maximumOperatingMass));
            if (naifId >= 0) throw new ArgumentOutOfRangeException(nameof(naifId));
            MaximumOperatingMass = maximumOperatingMass;
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            Frame = new Frame($"{name}_SPACECRAFT");
        }

        /// <summary>
        /// Add instrument to spacecraft
        /// </summary>
        /// <param name="instrument"></param>
        public void AddInstrument(Instrument instrument)
        {
            if (instrument == null) throw new ArgumentNullException(nameof(instrument));
            if (!_instruments.Add(instrument))
            {
                throw new ArgumentException("Instrument already added to spacecraft");
            }
        }

        /// <summary>
        /// Add engine to spacecraft
        /// </summary>
        /// <param name="engine"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddEngine(Engine engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            if (!FuelTanks.Contains(engine.FuelTank))
            {
                throw new InvalidOperationException(
                    "Unknown fuel tank, you must add fuel tank to spacecraft before add engine");
            }

            if (!_engines.Add(engine))
            {
                throw new ArgumentException("Engine already added to spacecraft");
            }
        }

        /// <summary>
        /// Add fuel tank to spacecraft
        /// </summary>
        /// <param name="fuelTank"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddFuelTank(FuelTank fuelTank)
        {
            if (fuelTank == null) throw new ArgumentNullException(nameof(fuelTank));
            if (!_fuelTanks.Add(fuelTank))
            {
                throw new ArgumentException("Fuel tank already added to spacecraft");
            }
        }

        /// <summary>
        /// Add payload to spacecraft
        /// </summary>
        /// <param name="payload"></param>
        public void AddPayload(Payload payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (!_payloads.Add(payload))
            {
                throw new ArgumentException("Payload already added to spacecraft");
            }
        }

        /// <summary>
        /// Set child spacecraft
        /// </summary>
        /// <param name="spacecraft"></param>
        public void SetChild(Spacecraft spacecraft)
        {
            Child = spacecraft;

            if (spacecraft != null)
            {
                spacecraft.Parent = this;
            }
        }

        /// <summary>
        /// Set parent spacecraft
        /// </summary>
        /// <param name="spacecraft"></param>
        public void SetParent(Spacecraft spacecraft)
        {
            Parent = spacecraft;
            if (spacecraft != null)
            {
                spacecraft.Child = this;
            }
        }

        /// <summary>
        /// Get Dry operating mass + fuel+ payloads + children
        /// </summary>
        /// <returns></returns>
        public override double GetTotalMass()
        {
            return DryOperatingMass + FuelTanks.Sum(x => x.InitialQuantity) + Payloads.Sum(x => x.Mass) +
                   (Child != null ? Child.GetTotalMass() : 0.0);
        }

        /// <summary>
        /// Get total ISP of this spacecraft
        /// </summary>
        /// <returns></returns>
        public double GetTotalISP()
        {
            return (Engines.Where(x => x.FuelTank.InitialQuantity > 0.0).Sum(x => x.Thrust) / Constants.g0) /
                   GetTotalFuelFlow();
        }

        /// <summary>
        /// Get total fuel flow of this spacecraft
        /// </summary>
        /// <returns></returns>
        public double GetTotalFuelFlow()
        {
            return Engines.Where(x => x.FuelTank.InitialQuantity > 0.0).Sum(x => x.FuelFlow);
        }

        /// <summary>
        /// Get total fuel of this spacecraft
        /// </summary>
        /// <returns></returns>
        public double GetTotalFuel()
        {
            return FuelTanks.Sum(x => x.InitialQuantity);
        }

        /// <summary>
        /// Put a maneuver in standby
        /// </summary>
        /// <param name="maneuver"></param>
        public void SetStandbyManeuver(Maneuver.Maneuver maneuver)
        {
            StandbyManeuver = maneuver;
        }

        public Dictionary<int, Maneuver.Maneuver> GetManeuvers()
        {
            Dictionary<int, Maneuver.Maneuver> maneuvers = new Dictionary<int, Maneuver.Maneuver>();

            var maneuver = StandbyManeuver;
            int order = 0;
            while (maneuver != null)
            {
                maneuvers[order] = maneuver;
                maneuver = maneuver.NextManeuver;
                order++;
            }

            return maneuvers;
        }

        public void SetInitialOrbitalParameters(OrbitalParameters.OrbitalParameters orbitalParameters)
        {
            if (orbitalParameters == null) throw new ArgumentNullException(nameof(orbitalParameters));
            (InitialOrbitalParameters?.Observer as CelestialBody)?.RemoveSatellite(this);

            InitialOrbitalParameters = orbitalParameters;
            (InitialOrbitalParameters?.Observer as CelestialBody)?.AddSatellite(this);
        }


        /// <summary>
        /// Get orientation relative to reference frame
        /// </summary>
        /// <param name="referenceFrame"></param>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public StateOrientation GetOrientation(Frame referenceFrame, in DateTime epoch)
        {
            return referenceFrame.ToFrame(Frame, epoch);
        }
    }
}