using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINE.Geometry
{
    public class Transform
    {
        private static double[,] transformMatrix;

        private static TransformationType transType;

        public double[,] TransformMatrix
        {
            get { return transformMatrix; }
            set { transformMatrix = value; }
        }

        public TransformationType TransformationType
        {
            get { return transType; }
        }

        /// <summary>
        /// 4x4 Transformation matrix
        /// </summary>
        /// <param name="_m11"></param>
        /// <param name="_m12"></param>
        /// <param name="_m13"></param>
        /// <param name="_m14"></param>
        /// <param name="_m21"></param>
        /// <param name="_m22"></param>
        /// <param name="_m23"></param>
        /// <param name="_m24"></param>
        /// <param name="_m31"></param>
        /// <param name="_m32"></param>
        /// <param name="_m33"></param>
        /// <param name="_m34"></param>
        /// <param name="_m41"></param>
        /// <param name="_m42"></param>
        /// <param name="_m43"></param>
        /// <param name="_m44"></param>
        public Transform(double _m11, double _m12, double _m13, double _m14, double _m21, double _m22, double _m23, double _m24, double _m31, double _m32, double _m33, double _m34, double _m41, double _m42, double _m43, double _m44)
        {
            transformMatrix = new double[4, 4] 
            {
                {_m11, _m12, _m13, _m14},
                {_m21, _m22, _m23, _m24},
                {_m31, _m32, _m33, _m34},
                {_m41, _m42, _m43, _m44}
            };
        }

        public Transform()
        {
            transformMatrix = new double[4, 4]
            {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            };
            transType = TransformationType.Null;
        }

        private Transform(double[,] matrix)
        {
            transformMatrix = matrix;
        }

        public static Transform Translation(Vector3d translationVector)
        {
            transformMatrix = new double[4, 4]
            {
                { 1, 0, 0, translationVector.X },
                { 0, 1, 0, translationVector.Y },
                { 0, 0, 1, translationVector.Z },
                { 0, 0, 0, 1 }
            };
            transType = TransformationType.Translation;
            return new Transform(transformMatrix);
        }

        public static Transform Rotation(Vector3d axis, double angleRadians)
        {
            axis.Unitize();

            // Check if the axis is parallel to one of the 3 axes
            if (axis.IsParallelTo(Vector3d.ZAxis))
            {
                // Rotate on the Z axis
                transformMatrix = new double[4, 4]
                {
                    { Math.Cos(angleRadians), -Math.Sin(angleRadians), 0, 0 },
                    { Math.Sin(angleRadians), Math.Cos(angleRadians), 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
            }

            else if (axis.IsParallelTo(Vector3d.YAxis))
            {
                // rotate on the Y axis
                transformMatrix = new double[4, 4]
                {
                    { Math.Cos(angleRadians), 0, Math.Sin(angleRadians), 0 },
                    { 0, 1, 0, 0 },
                    { -Math.Sin(angleRadians), 0, Math.Cos(angleRadians), 0 },
                    { 0, 0, 0, 1 }
                };
            }

            else if (axis.IsParallelTo(Vector3d.XAxis))
            {
                // Rotation on the X Axis
                transformMatrix = new double[4, 4]
                {
                    { 1, 0, 0, 0 },
                    { 0, Math.Cos(angleRadians), -Math.Sin(angleRadians), 0 },
                    { 0, Math.Sin(angleRadians), Math.Cos(angleRadians), 0 },
                    { 0, 0, 0, 1 }
                };
            }

            else
            {
                // Rotation on an arbitrary axis
                transformMatrix = new double[4, 4]
                {
                    { Math.Pow(axis.X, 2) + (1 - Math.Pow(axis.X,2)) * Math.Cos(angleRadians), axis.X * axis.Y * (1 - Math.Cos(angleRadians)) - axis.Z * Math.Sin(angleRadians), axis.X * axis.Z * (1 - Math.Cos(angleRadians)) + axis.Y * Math.Sin(angleRadians), 0 },
                    { axis.X * axis.Y * (1 - Math.Cos(angleRadians)) + axis.Z * Math.Sin(angleRadians), Math.Pow(axis.Y, 2) + (1 - Math.Pow(axis.Y,2)) * Math.Cos(angleRadians), axis.Y * axis.Z * (1 - Math.Cos(angleRadians)) - axis.X * Math.Sin(angleRadians), 0 },
                    { axis.X * axis.Z * (1 - Math.Cos(angleRadians)) - axis.Y * Math.Sin(angleRadians), axis.Y * axis.Z * (1 - Math.Cos(angleRadians)) + axis.X * Math.Sin(angleRadians), Math.Pow(axis.Z, 2) + (1 - Math.Pow(axis.Z,2)) * Math.Cos(angleRadians), 0 },
                    { 0, 0, 0, 1 }
                };
            }
            transType = TransformationType.Rotation;
            return new Transform(transformMatrix);
        }

    }

    public enum TransformationType
    {
        // null
        Null = -1,

        // Translation
        Translation = 0,

        // Rotation
        Rotation = 1
    }
}
