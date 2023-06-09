using System;
using IO.Astrodynamics.Frames;
using IO.Astrodynamics.Math;

namespace IO.Astrodynamics.OrbitalParameters
{
    public record StateOrientation(Quaternion Rotation, Vector3 AngularVelocity, DateTime Epoch, Frame ReferenceFrame)
    {
        /// <summary>
        /// Frame from which the rotation is applied
        /// </summary>
        public Frame ReferenceFrame { get; } = ReferenceFrame;

        public StateOrientation RelativeToICRF()
        {
            return RelativeTo(Frame.ICRF);
        }

        public StateOrientation RelativeTo(Frame frame)
        {
            if (ReferenceFrame == frame)
            {
                return this;
            }

            return new StateOrientation(Rotation * ReferenceFrame.ToFrame(frame, Epoch).Rotation,
                AngularVelocity - Frame.ICRF.ToFrame(frame, Epoch).AngularVelocity, Epoch, frame);
        }
    }
}