// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System.Runtime.InteropServices;

namespace IO.Astrodynamics.DTO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct PhasingManeuver
{
    public readonly int ManeuverOrder;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Spacecraft.ENGINESIZE)]
    public readonly string[] Engines;

    public readonly double AttitudeHoldDuration;
    public readonly double MinimumEpoch;

    public readonly int NumberRevolutions;
    public StateVector TargetOrbit;

    public Window ManeuverWindow;
    public Window ThrustWindow;
    public Window AttitudeWindow;
    public Vector3D DeltaV;
    public readonly double FuelBurned;

    public PhasingManeuver() : this(-1, 0.0, double.NaN, 0, default)
    {
    }

    public PhasingManeuver(int maneuverOrder, double attitudeHoldDuration, double minimumEpoch, int numberRevolutions,
        StateVector targetOrbit)
    {
        Engines = new string[Spacecraft.ENGINESIZE];
        ManeuverOrder = maneuverOrder;
        AttitudeHoldDuration = attitudeHoldDuration;
        MinimumEpoch = minimumEpoch;
        NumberRevolutions = numberRevolutions;
        TargetOrbit = targetOrbit;
        ManeuverWindow = default;
        ThrustWindow = default;
        AttitudeWindow = default;
        DeltaV = default;
        FuelBurned = default;
    }
}