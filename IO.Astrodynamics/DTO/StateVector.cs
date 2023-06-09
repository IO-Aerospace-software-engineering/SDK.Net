// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System.Runtime.InteropServices;

namespace IO.Astrodynamics.DTO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public readonly struct StateVector
{
    public double Epoch { get; }
    public Vector3D Position { get; }
    public Vector3D Velocity { get; }
    public int CenterOfMotionId { get; }
    public string Frame { get; }
    public string Error { get; } = string.Empty;

    public StateVector(int centerOfMotionId, double epoch, string frame, in Vector3D position, in Vector3D velocity)
    {
        CenterOfMotionId = centerOfMotionId;
        Epoch = epoch;
        Frame = frame;
        Position = position;
        Velocity = velocity;
    }
    
    public bool HasError()
    {
        return !string.IsNullOrEmpty(Error);
    }
}