﻿// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System.Runtime.InteropServices;

namespace IO.Astrodynamics.DTO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ConicElements
{
    public int CenterOfMotionId;
    public double Epoch;
    public double PerifocalDistance;
    public double Eccentricity;
    public double Inclination;
    public double AscendingNodeLongitude;
    public double PeriapsisArgument;
    public double MeanAnomaly;
    public double TrueAnomaly;
    public double OrbitalPeriod;
    public double SemiMajorAxis;
    public string Frame;
}