﻿using System;
using IO.Astrodynamics.Frames;
using IO.Astrodynamics.Time;

namespace IO.Astrodynamics.Body;

public class Barycenter : Body
{
    public Barycenter(int naifId) : this(naifId, DateTimeExtension.J2000)
    {
    }

    public Barycenter(int naifId, DateTime epoch) : base(naifId, Frame.ECLIPTIC, epoch)
    {
    }

    public override double GetTotalMass()
    {
        return this.Mass;
    }
}