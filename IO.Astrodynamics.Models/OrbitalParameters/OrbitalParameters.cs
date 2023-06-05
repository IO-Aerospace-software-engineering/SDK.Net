using System;
using System.Collections.Generic;
using IO.Astrodynamics.Models.Body;
using IO.Astrodynamics.Models.Coordinates;
using IO.Astrodynamics.Models.Math;
using IO.Astrodynamics.Models.Mission;
using IO.Astrodynamics.Models.SeedWork;
using IO.Astrodynamics.Models.Time;

namespace IO.Astrodynamics.Models.OrbitalParameters;

public abstract class OrbitalParameters : Entity, IEquatable<OrbitalParameters>
{
    public CelestialBodyScenario CenterOfMotion { get; protected set; }

    public DateTime Epoch { get; private set; }

    public Frame.Frame Frame { get; private set; }

    protected OrbitalParameters() { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="centerOfMotion"></param>
    /// <param name="epoch"></param>
    /// <param name="frame"></param>
    protected OrbitalParameters(CelestialBodyScenario centerOfMotion, DateTime epoch, Frame.Frame frame)
    {
        CenterOfMotion = centerOfMotion;
        Epoch = epoch;
        Frame = frame;
    }

    /// <summary>
    /// Get eccentric vector
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 EccentricityVector()
    {
        return ToStateVector().EccentricityVector();
    }

    /// <summary>
    /// Get eccentricity
    /// </summary>
    /// <returns></returns>
    public abstract double Eccentricity();

    /// <summary>
    /// Get the specific angular momentum
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 SpecificAngularMomentum()
    {
        return ToStateVector().SpecificAngularMomentum();
    }

    /// <summary>
    /// Get the specific orbital energy in MJ
    /// </summary>
    /// <returns></returns>
    public virtual double SpecificOrbitalEnergy()
    {
        return ToStateVector().SpecificOrbitalEnergy();
    }

    /// <summary>
    /// Get inclination
    /// </summary>
    /// <returns></returns>
    public abstract double Inclination();

    /// <summary>
    /// Get the semi major axis
    /// </summary>
    /// <returns></returns>
    public abstract double SemiMajorAxis();

    /// <summary>
    /// Get vector to ascending node unitless
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 AscendingNodeVector()
    {
        if (Inclination() == 0.0)
        {
            return Vector3.VectorX;
        }
        return ToStateVector().AscendingNodeVector();
    }

    /// <summary>
    /// Get vector to descending node unitless
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 DescendingNodeVector()
    {
        return AscendingNodeVector().Inverse();
    }

    /// <summary>
    /// Get ascending node angle
    /// </summary>
    /// <returns></returns>
    public abstract double AscendingNode();

    /// <summary>
    /// Get the argument of periapis
    /// </summary>
    /// <returns></returns>
    public abstract double ArgumentOfPeriapsis();

    /// <summary>
    /// Get the true anomaly
    /// </summary>
    /// <returns></returns>
    public abstract double TrueAnomaly();

    /// <summary>
    /// Get the eccentric anomaly
    /// </summary>
    /// <returns></returns>
    public abstract double EccentricAnomaly();

    /// <summary>
    /// Get the mean anomaly
    /// </summary>
    /// <returns></returns>
    public abstract double MeanAnomaly();

    /// <summary>
    /// Compute mean anomaly from true anomaly
    /// </summary>
    /// <param name="trueAnomaly"></param>
    /// <returns></returns>
    public double MeanAnomaly(double trueAnomaly)
    {
        if (trueAnomaly < 0.0)
        {
            trueAnomaly += Constants._2PI;
        }
        //X = cos E
        double x = (Eccentricity() + System.Math.Cos(trueAnomaly)) / (1 + Eccentricity() * System.Math.Cos(trueAnomaly));
        double eccAno = System.Math.Acos(x);
        double M = eccAno - Eccentricity() * System.Math.Sin(eccAno);

        if (trueAnomaly > Constants.PI)
        {
            M = Constants._2PI - M;
        }

        return M;
    }

    /// <summary>
    /// Get orbital period
    /// </summary>
    /// <returns></returns>
    public TimeSpan Period()
    {
        double a = SemiMajorAxis();
        double T = Constants._2PI * System.Math.Sqrt((a * a * a) / CenterOfMotion.PhysicalBody.GM);
        return TimeSpan.FromSeconds(T);
    }

    /// <summary>
    /// Get orbital mean motion
    /// </summary>
    /// <returns></returns>
    public double MeanMotion()
    {
        return Constants._2PI / Period().TotalSeconds;
    }


    /// <summary>
    /// Get the state vector
    /// </summary>
    /// <returns></returns>
    public virtual StateVector ToStateVector()
    {
        var e = Eccentricity();
        var p = SemiMajorAxis() * (1 - e * e);
        var v = TrueAnomaly();
        var r0 = p / (1 + e * System.Math.Cos(v));
        var x = r0 * System.Math.Cos(v);
        var y = r0 * System.Math.Sin(v);
        var dotX = -System.Math.Sqrt(CenterOfMotion.PhysicalBody.GM / p) * System.Math.Sin(v);
        var dotY = System.Math.Sqrt(CenterOfMotion.PhysicalBody.GM / p) * (e + System.Math.Cos(v));
        Matrix R3 = Matrix.CreateRotationMatrixZ(-AscendingNode());
        Matrix R1 = Matrix.CreateRotationMatrixX(-Inclination());
        Matrix R3w = Matrix.CreateRotationMatrixZ(-ArgumentOfPeriapsis());
        Matrix R = R3 * R1 * R3w;
        double[] pos = { x, y, 0.0 };
        double[] vel = { dotX, dotY, 0.0 };
        double[] finalPos = pos * R;
        double[] finalV = vel * R;

        return new StateVector(new Vector3(finalPos[0], finalPos[1], finalPos[2]), new Vector3(finalV[0], finalV[1], finalV[2]), CenterOfMotion, Epoch, Frame);
    }

    /// <summary>
    /// Convert to equinoctial
    /// </summary>
    /// <returns></returns>
    public virtual EquinoctialElements ToEquinoctial()
    {
        double e = Eccentricity();
        double o = AscendingNode();
        double w = ArgumentOfPeriapsis();
        double i = Inclination();
        double v = TrueAnomaly();

        double p = SemiMajorAxis() * (1 - e * e);
        double f = e * System.Math.Cos(o + w);
        double g = e * System.Math.Sin(o + w);
        double h = System.Math.Tan(i * 0.5) * System.Math.Cos(o);
        double k = System.Math.Tan(i * 0.5) * System.Math.Sin(o);
        double l0 = o + w + v;

        return new EquinoctialElements(p, f, g, h, k, l0, CenterOfMotion, Epoch, Frame);
    }

    /// <summary>
    /// Get perigee vector
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 PerigeeVector()
    {
        if (Eccentricity() == 0.0)
        {
            return Vector3.VectorX * SemiMajorAxis();
        }
        return EccentricityVector().Normalize() * SemiMajorAxis() * (1.0 - Eccentricity());
    }

    /// <summary>
    /// Get apogee vector
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 ApogeeVector()
    {
        if (Eccentricity() == 0.0)
        {
            return Vector3.VectorX.Inverse() * SemiMajorAxis();
        }
        return EccentricityVector().Normalize().Inverse() * SemiMajorAxis() * (1.0 + Eccentricity());
    }

    /// <summary>
    /// Get orbital parameters at epoch
    /// </summary>
    /// <param name="epoch"></param>
    /// <returns></returns>
    public OrbitalParameters AtEpoch(DateTime epoch)
    {
        return ToKeplerianElements(epoch);
    }

    /// <summary>
    /// Get the true longitude
    /// </summary>
    /// <returns></returns>
    public double TrueLongitude()
    {
        return (AscendingNode() + ArgumentOfPeriapsis() + TrueAnomaly()) % Constants._2PI;
    }

    /// <summary>
    /// Get the mean longitude
    /// </summary>
    /// <returns></returns>
    public double MeanLongitude()
    {
        return (AscendingNode() + ArgumentOfPeriapsis() + MeanAnomaly()) % Constants._2PI;
    }

    public bool IsCircular()
    {
        return Eccentricity() == 0.0;
    }

    public bool IsParabolic()
    {
        return Eccentricity() == 1.0;
    }

    public bool IsHyperbolic()
    {
        return Eccentricity() > 1.0;
    }

    private KeplerianElements ToKeplerianElements(DateTime epoch)
    {
        double ellapsedTime = epoch.SecondsFromJ2000() - Epoch.SecondsFromJ2000();
        double M = MeanAnomaly() + MeanMotion() * ellapsedTime;
        while (M < 0.0)
        {
            M += Constants._2PI;
        }
        return new KeplerianElements(SemiMajorAxis(), Eccentricity(), Inclination(), AscendingNode(), ArgumentOfPeriapsis(), M % Constants._2PI, CenterOfMotion, epoch, Frame);
    }

    public virtual KeplerianElements ToKeplerianElements()
    {
        return ToKeplerianElements(Epoch);
    }

    public virtual OrbitalParameters ToFrame(Frame.Frame frame)
    {
        if (frame == Frame)
        {
            return this;
        }

        StateVector icrfSv = this.ToStateVector();
        var orientation = Frame.ToFrame(frame, Epoch);
        var newPos = icrfSv.Position.Rotate(orientation.Orientation);
        var newVel = icrfSv.Velocity.Rotate(orientation.Orientation) - orientation.AngularVelocity.Cross(newPos);
        return new StateVector(newPos, newVel, CenterOfMotion, Epoch, frame);
    }

    public Equatorial ToEquatorial()
    {
        return new Equatorial(ToStateVector());
    }

    public double PerigeeVelocity()
    {
        return System.Math.Sqrt(CenterOfMotion.PhysicalBody.GM * (2 / PerigeeVector().Magnitude() - 1.0 / SemiMajorAxis()));
    }

    public double ApogeeVelocity()
    {
        return System.Math.Sqrt(CenterOfMotion.PhysicalBody.GM * (2 / ApogeeVector().Magnitude() - 1.0 / SemiMajorAxis()));
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as OrbitalParameters);
    }

    public bool Equals(OrbitalParameters other)
    {
        return other is not null &&
               base.Equals(other) || (
               EqualityComparer<CelestialBodyScenario>.Default.Equals(CenterOfMotion, other.CenterOfMotion) &&
               Epoch == other.Epoch &&
               EqualityComparer<Frame.Frame>.Default.Equals(Frame, other.Frame));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Id, CenterOfMotion, Epoch, Frame);
    }

    public static bool operator ==(OrbitalParameters left, OrbitalParameters right)
    {
        return EqualityComparer<OrbitalParameters>.Default.Equals(left, right);
    }

    public static bool operator !=(OrbitalParameters left, OrbitalParameters right)
    {
        return !(left == right);
    }
}