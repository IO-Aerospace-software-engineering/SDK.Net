// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System;
using IO.Astrodynamics.Body;
using IO.Astrodynamics.Body.Spacecraft;

namespace IO.Astrodynamics.Maneuver;

public class InstrumentPointingToAttitude : Maneuver
{
    public Instrument Instrument { get; }
    public INaifObject TargetId { get; }
    
    public InstrumentPointingToAttitude(Spacecraft spacecraft, DateTime minimumEpoch, TimeSpan maneuverHoldDuration, Engine[] engines,
        Instrument instrument, INaifObject targetId) : base(spacecraft, minimumEpoch, maneuverHoldDuration, engines)
    {
        Instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));
        TargetId = targetId ?? throw new ArgumentNullException(nameof(targetId));
    }

    
}