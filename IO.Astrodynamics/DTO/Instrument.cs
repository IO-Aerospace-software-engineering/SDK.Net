// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System.Runtime.InteropServices;

namespace IO.Astrodynamics.DTO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Instrument
{
    public readonly int Id;
    public readonly string Name;
    public readonly string Shape;
    public Vector3D Orientation;
    public Vector3D Boresight;
    public Vector3D FovRefVector;
    public readonly double FieldOfView;
    public readonly double CrossAngle;

    public Instrument(int id, string name, string shape, Vector3D orientation, Vector3D boresight,
        Vector3D fovRefVector, double fieldOfView, double crossAngle)
    {
        Id = id;
        Name = name;
        Shape = shape.ToLower();
        Orientation = orientation;
        Boresight = boresight;
        FovRefVector = fovRefVector;
        FieldOfView = fieldOfView;
        CrossAngle = crossAngle;
    }
}