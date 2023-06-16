﻿using System;
using System.Collections.Generic;
using IO.Astrodynamics.Body;
using IO.Astrodynamics.Coordinates;
using IO.Astrodynamics.Frames;
using IO.Astrodynamics.Time;

namespace IO.Astrodynamics.Body
{
    public interface ILocalizable : INaifObject
    {
        IEnumerable<OrbitalParameters.OrbitalParameters> GetEphemeris(Window searchWindow, CelestialBody observer, Frame frame, Aberration aberration, TimeSpan stepSize);
        OrbitalParameters.OrbitalParameters GetEphemeris(DateTime epoch, CelestialBody observer, Frame frame, Aberration aberration);
        Math.Vector3 GetPosition(DateTime epoch, ILocalizable observer, Frame frame, Aberration aberration);
        Math.Vector3 GetVelocity(DateTime epoch, ILocalizable observer, Frame frame, Aberration aberration);
        double AngularSeparation(DateTime epoch, ILocalizable target1, ILocalizable target2, Aberration aberration);
        Frame Frame { get; }

        /// <summary>
        ///     Find time windows based on distance constraint
        /// </summary>
        /// <param name="searchWindow"></param>
        /// <param name="observer"></param>
        /// <param name="target"></param>
        /// <param name="relationalOperator"></param>
        /// <param name="value"></param>
        /// <param name="aberration"></param>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        IEnumerable<Window> FindWindowsOnDistanceConstraint(Window searchWindow, INaifObject observer, RelationnalOperator relationalOperator, double value,
            Aberration aberration, TimeSpan stepSize);

        /// <summary>
        ///     Find time windows based on occultation constraint
        /// </summary>
        /// <param name="searchWindow"></param>
        /// <param name="target"></param>
        /// <param name="targetShape"></param>
        /// <param name="frontBody"></param>
        /// <param name="frontShape"></param>
        /// <param name="occultationType"></param>
        /// <param name="aberration"></param>
        /// <param name="stepSize"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        IEnumerable<Window> FindWindowsOnOccultationConstraint(Window searchWindow, INaifObject observer, ShapeType targetShape, INaifObject frontBody,
            ShapeType frontShape, OccultationType occultationType, Aberration aberration, TimeSpan stepSize);

        /// <summary>
        ///     Find time windows based on coordinate constraint
        /// </summary>
        /// <param name="searchWindow"></param>
        /// <param name="frame"></param>
        /// <param name="coordinateSystem"></param>
        /// <param name="coordinate"></param>
        /// <param name="relationalOperator"></param>
        /// <param name="value"></param>
        /// <param name="adjustValue"></param>
        /// <param name="aberration"></param>
        /// <param name="stepSize"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IEnumerable<Window> FindWindowsOnCoordinateConstraint(Window searchWindow, INaifObject observer, Frame frame, CoordinateSystem coordinateSystem,
            Coordinate coordinate, RelationnalOperator relationalOperator, double value, double adjustValue, Aberration aberration, TimeSpan stepSize);

        /// <summary>
        ///     Find time windows based on illumination constraint
        /// </summary>
        /// <param name="searchWindow"></param>
        /// <param name="illuminationSource"></param>
        /// <param name="observer"></param>
        /// <param name="targetBody"></param>
        /// <param name="fixedFrame"></param>
        /// <param name="geodetic"></param>
        /// <param name="illuminationType"></param>
        /// <param name="relationalOperator"></param>
        /// <param name="value"></param>
        /// <param name="adjustValue"></param>
        /// <param name="aberration"></param>
        /// <param name="stepSize"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable<Window> FindWindowsOnIlluminationConstraint(Window searchWindow, INaifObject observer,
            Geodetic geodetic, IlluminationAngle illuminationType, RelationnalOperator relationalOperator, double value, double adjustValue, Aberration aberration,
            TimeSpan stepSize, INaifObject illuminationSource, string method = "Ellipsoid");
    }
}