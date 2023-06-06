// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System;
using IO.Astrodynamics.Models.Body;
using IO.Astrodynamics.Models.Body.Spacecraft;
using IO.Astrodynamics.Models.Mission;

namespace IO.Astrodynamics.Models.Maneuver;

public class InstrumentPointingToAttitude : Maneuver
{
    public SpacecraftInstrument Instrument { get; }
    public INaifObject TargetId { get; }
    
    public InstrumentPointingToAttitude(SpacecraftScenario spacecraft, DateTime minimumEpoch, TimeSpan maneuverHoldDuration, SpacecraftEngine[] engines,
        SpacecraftInstrument instrument, INaifObject targetId) : base(spacecraft, minimumEpoch, maneuverHoldDuration, engines)
    {
        Instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));
        TargetId = targetId ?? throw new ArgumentNullException(nameof(targetId));
    }

    
}