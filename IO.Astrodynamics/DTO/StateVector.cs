// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System.Runtime.InteropServices;

namespace IO.Astrodynamics.DTO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct StateVector
{
    public double Epoch;
    public Vector3D Position;
    public Vector3D Velocity;
    public int CenterOfMotionId;
    public string Frame;

    public StateVector(int centerOfMotionId, double epoch, string frame, Vector3D position, Vector3D velocity)
    {
        CenterOfMotionId = centerOfMotionId;
        Epoch = epoch;
        Frame = frame;
        Position = position;
        Velocity = velocity;
    }
}