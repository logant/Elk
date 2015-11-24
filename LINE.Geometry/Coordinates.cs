using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINE.Geometry
{
    public class Point3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /// <summary>
        /// Create a 3d Point object with X, Y and Z values.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <param name="z">Z Coordinate</param>
        public Point3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Create a Point3d at coordinates (0,0,0).
        /// </summary>
        public static Point3d Origin
        {
            get { return new Point3d(0, 0, 0); }
        }

        /// <summary>
        /// Get the distance to a second point.
        /// </summary>
        /// <param name="point">Second point to measure distance to.</param>
        /// <returns></returns>
        public double DistanceTo(Point3d point)
        {
            return Math.Sqrt(Math.Pow((X - point.X), 2.0) + Math.Pow((Y - point.Y), 2) + Math.Pow((Z - point.Z), 2));
        }

        /// <summary>
        /// Convert the point to a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + "}";
        }

        /// <summary>
        /// Move a point by adding a vector to it.
        /// </summary>
        /// <param name="point">Starting point</param>
        /// <param name="vector">Vector to translate from the starting point.</param>
        /// <returns></returns>
        public static Point3d operator +(Point3d point, Vector3d vector)
        {
            return new Point3d(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }

        /// <summary>
        /// Add two points together to get a new point.
        /// </summary>
        /// <param name="point1">First point</param>
        /// <param name="point2">Second point</param>
        /// <returns></returns>
        public static Point3d operator +(Point3d point1, Point3d point2)
        {
            return new Point3d(point1.X + point2.X, point1.Y + point2.Y, point1.Z + point2.Z);
        }

        /// <summary>
        /// Subtract a vector from a point.
        /// </summary>
        /// <param name="point">Starting point</param>
        /// <param name="vector">Vector to subtract from the point.</param>
        /// <returns></returns>
        public static Point3d operator -(Point3d point, Vector3d vector)
        {
            return new Point3d(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
        }

        /// <summary>
        /// Create a new point by subtracting two points.
        /// </summary>
        /// <param name="pt1">Point 1</param>
        /// <param name="pt2">Point 2</param>
        /// <returns></returns>
        public static Point3d operator -(Point3d pt1, Point3d pt2)
        {
            return new Point3d(pt1.X - pt2.X, pt1.Y - pt2.Y, pt1.Z - pt2.Z);
        }

        /// <summary>
        /// Convert a Vector3d into a Point3d.
        /// </summary>
        /// <param name="vector">Vector to convert into a point</param>
        /// <returns></returns>
        public static explicit operator Point3d(Vector3d vector)
        {
            return new Point3d(vector.X, vector.Y, vector.Z);
        }

        public bool Rotate(Vector3d axis, double angleRadians)
        {
            try
            {
                Transform rotationTransform = Transform.Rotation(axis, angleRadians);
                double[,] matrix = rotationTransform.TransformMatrix;
                double x = X;
                double y = Y;
                double z = Z;
                X = (matrix[0, 0] * x) + (matrix[0, 1] * x) + (matrix[0, 2] * x);
                Y = (matrix[1, 0] * y) + (matrix[1, 1] * y) + (matrix[1, 2] * y);
                Z = (matrix[2, 0] * z) + (matrix[2, 1] * z) + (matrix[2, 2] * z);
                return true;
            }
            catch { return false; }
        }
    }

    public class Vector3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3d(Point3d startPoint, Point3d endPoint)
        {
            X = endPoint.X - startPoint.X;
            Y = endPoint.Y - startPoint.Y;
            Z = endPoint.Z - startPoint.Z;
        }

        public static Vector3d XAxis
        {
            get { return new Vector3d(1, 0, 0); }
        }

        public static Vector3d YAxis
        {
            get { return new Vector3d(0, 1, 0); }
        }

        public static Vector3d ZAxis
        {
            get { return new Vector3d(0, 0, 1); }
        }

        public override string ToString()
        {
            return "{" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + "}";
        }

        public bool Unitize()
        {
            try
            {
                double length = this.Length;
                X = X / length;
                Y = Y / length;
                Z = Z / length;

                return true;
            }
            catch { return false; }
        }

        public double Length
        {
            get 
            {
                double len = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
                return len; 
            }
        }

        public bool IsUnitVector()
        {
            if (this.Length == 1.0)
                return true;
            else
                return false;
        }

        public bool SetMagnitude(double length)
        {
            try
            {
                double mag = length / this.Length;
                X = X * mag;
                Y = Y * mag;
                Z = Z * mag;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static explicit operator Vector3d(Point3d pt)
        {
            return new Vector3d(pt.X, pt.Y, pt.Z);
        }

        public static Vector3d operator *(Vector3d vector, double scale)
        {
            // make a scaled copy of the vector
            Vector3d vect = new Vector3d(vector.X * scale, vector.Y * scale, vector.Z * scale);
            return vect;
        }

        public static Vector3d operator *(double scale, Vector3d vector)
        {
            // make a scaled copy of the vector
            Vector3d vect = new Vector3d(vector.X * scale, vector.Y * scale, vector.Z * scale);
            return vect;
        }

        public static Vector3d operator +(Vector3d vect1, Vector3d vect2)
        {
            // add the two vectors
            Vector3d vect = new Vector3d(vect1.X + vect2.X, vect1.Y + vect2.Y, vect1.Z + vect2.Z);
            return vect;
        }

        public static Vector3d operator -(Vector3d vect1, Vector3d vect2)
        {
            // subtract the second vector from the first.
            Vector3d vect = new Vector3d(vect1.X - vect2.X, vect1.Y - vect2.Y, vect1.Z - vect2.Z);
            return vect;
        }

        public static double DotProduct(Vector3d vect1, Vector3d vect2, bool unitize)
        {
            if (unitize)
            {
                vect1.Unitize();
                vect2.Unitize();
            }
            return (vect1.X * vect2.X + vect1.Y * vect2.Y + vect1.Z * vect2.Z);
        }

        public static Vector3d CrossProduct(Vector3d vect1, Vector3d vect2)
        {
            Vector3d vect = new Vector3d((vect1.Y * vect2.Z) - (vect1.Z * vect2.Y),
                                         (vect1.Z * vect2.X) - (vect1.X * vect2.Z),
                                         (vect1.X * vect2.Y) - (vect1.Y * vect2.X));
            return vect;
        }

        public double AngleTo(Vector3d vector)
        {
            Vector3d currentVect = this.Clone();
            double angle = Math.Acos(Vector3d.DotProduct(currentVect, vector, false) / (currentVect.Length * vector.Length));
            return angle;
        }

        public Vector3d Clone()
        {
            Vector3d vect = new Vector3d(X, Y, Z);
            return vect;
        }

        public bool IsParallelTo(Vector3d vector)
        {
            Vector3d vect3 = Vector3d.CrossProduct(this, vector);
            double length = vect3.Length;

            if (length <= double.Epsilon)
                return true;
            else
                return false;
        }

        public bool IsPerpendicularTo(Vector3d vector)
        {
            Vector3d vect1 = this.Clone();
            Vector3d vect2 = vector.Clone();

            double dotProduct = Vector3d.DotProduct(vect1, vect2, true);

            if (dotProduct <= double.Epsilon)
                return true;
            else
                return false;
        }

        public void Reverse()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        public bool Rotate(double angle, Vector3d axis)
        {
            try
            {
                Vector3d axisVector = axis.Clone();
                axisVector.Unitize();

                Vector3d vectToRotate = this.Clone();
                Vector3d rotatedVect = (1 - Math.Cos(angle)) * (Vector3d.DotProduct(vectToRotate, axisVector, false) * axisVector) + Math.Cos(angle) * vectToRotate + Math.Sin(angle) * Vector3d.CrossProduct(axis, vectToRotate);

                X = rotatedVect.X;
                Y = rotatedVect.Y;
                Z = rotatedVect.Z;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class Plane
    {
        public Point3d Origin { get; set; }
        public Vector3d XAxis { get; set; }
        public Vector3d YAxis { get; set; }
        public Vector3d ZAxis 
        {
            get
            {
                return ZAxis;
            }
            set
            {
                Vector3d normal = value;
                normal.Unitize();
                ZAxis = normal;

                // rotate the vector on an arbitrary axis to form a vector to get a perpendicular axis from
                Vector3d planarVect = ZAxis.Clone();
                planarVect.Rotate(Math.PI / 2, Vector3d.XAxis);

                // X axis will be the first perpendicular
                Vector3d perpendicularVect = Vector3d.CrossProduct(normal, planarVect);
                perpendicularVect.Unitize();
                XAxis = perpendicularVect;

                // Y axis will be the rotated perpendicular vector.
                Vector3d rotatedVect = perpendicularVect.Clone();
                rotatedVect.Rotate(Math.PI / 2, normal);
                rotatedVect.Unitize();
                YAxis = rotatedVect;
            }
        }

        public Vector3d Normal
        {
            get { return ZAxis; }
            set 
            {
                Vector3d normal = value;
                normal.Unitize();
                ZAxis = normal;

                // rotate the vector on an arbitrary axis to form a vector to get a perpendicular axis from
                Vector3d planarVect = ZAxis.Clone();
                planarVect.Rotate(Math.PI / 2, Vector3d.XAxis);

                // X axis will be the first perpendicular
                Vector3d perpendicularVect = Vector3d.CrossProduct(normal, planarVect);
                perpendicularVect.Unitize();
                XAxis = perpendicularVect;

                // Y axis will be the rotated perpendicular vector.
                Vector3d rotatedVect = perpendicularVect.Clone();
                rotatedVect.Rotate(Math.PI / 2, normal);
                rotatedVect.Unitize();
                YAxis = rotatedVect;
            }
        }
        
        public Plane(Point3d origin, Vector3d normal)
        {
            Origin = origin;
            normal.Unitize();
            ZAxis = normal;

            // rotate the vector on an arbitrary axis to form a vector to get a perpendicular axis from
            Vector3d planarVect = ZAxis.Clone();
            planarVect.Rotate(Math.PI / 2, Vector3d.XAxis);

            // X axis will be the first perpendicular
            Vector3d perpendicularVect = Vector3d.CrossProduct(normal, planarVect);
            perpendicularVect.Unitize();
            XAxis = perpendicularVect;

            // Y axis will be the rotated perpendicular vector.
            Vector3d rotatedVect = perpendicularVect.Clone();
            rotatedVect.Rotate(Math.PI / 2, normal);
            rotatedVect.Unitize();
            YAxis = rotatedVect;
        }

        public Plane(Point3d origin, Vector3d xAxis, Vector3d yAxis)
        {
            Vector3d zVect = Vector3d.CrossProduct(xAxis, yAxis);

            if (xAxis.AngleTo(yAxis) == Math.PI)
            {
                // build the Plane
                Origin = origin;
                zVect.Unitize();
                xAxis.Unitize();
                yAxis.Unitize();

                ZAxis = zVect;
                YAxis = yAxis;
                XAxis = xAxis;
            }
        }

        public Plane(Point3d point1, Point3d point2, Point3d point3)
        {
            // Set the origin and X axis based on the incoming points.
            Origin = point1;
            XAxis = new Vector3d(point1, point2);

            // Create a temporary Y axis so that we may get the normal of the plane via cross product
            Vector3d tempY = new Vector3d(point1, point3);
            ZAxis = Vector3d.CrossProduct(XAxis, tempY);

            // Create a true Y axis by rotating the X axis 
            tempY = XAxis.Clone();
            tempY.Rotate(Math.PI / 2, ZAxis);
            YAxis = tempY;
        }

        public static Plane XYPlane(Point3d origin)
        {
            Plane xy = new Plane(origin, Vector3d.XAxis, Vector3d.YAxis);
            return xy;
        }

        public static Plane YZPlane(Point3d origin)
        {
            Plane yz = new Plane(origin, Vector3d.YAxis, Vector3d.ZAxis);
            return yz;
        }

        public static Plane XZPlane(Point3d origin)
        {
            Plane xz = new Plane(origin, Vector3d.XAxis, Vector3d.ZAxis);
            return xz;
        }

        public Point3d ProjectTo(Point3d point)
        {
            Vector3d origToPt = new Vector3d(Origin, point);
            double angle = ZAxis.AngleTo(origToPt);
            if (angle <= Math.PI / 2)
            {
                // Point is 'above' the plane
                double alpha = Math.PI / 2 - angle;
                double length = Math.Sin(alpha) * origToPt.Length;
                Vector3d translate = ZAxis.Clone();
                translate.Reverse();
                translate.SetMagnitude(length);

                Point3d pt = point + translate;
                return pt;
            }
            else
            {
                // Point is 'below' the plane
                double alpha = Math.PI - angle;
                double length = Math.Sin(alpha) * origToPt.Length;
                Vector3d translate = ZAxis.Clone();
                translate.SetMagnitude(length);

                Point3d pt = point + translate;
                return pt;
            }
        }
    }

    public class ControlPoint
    {
        public Point3d Location { get; set; }
        public double Weight { get; set; }

        public ControlPoint(Point3d point, double weight)
        {
            Location = point;
            Weight = weight;
        }
    }

    public class Interval
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Mid
        {
            get { return ((Max - Min) / 2) + Min; }
        }

        public double Length
        {
            get { return Max - Min; }
        }

        /// <summary>
        /// Create a default interval of 0.0 to 1.0
        /// </summary>
        public Interval()
        {
            Min = 0.0;
            Max = 1.0;
        }

        /// <summary>
        /// Create a domain between the minimum and maximum value's given
        /// </summary>
        /// <param name="min">Minimum interval value</param>
        /// <param name="max">Maximum interval value</param>
        public Interval(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return "{ " + Min.ToString() + " To " + Max.ToString() + " }";
        }
    }

    public class Interval2d
    {
        public Interval UInterval { get; set; }
        public Interval VInterval { get; set; }
        
        public double UMax
        {
            get
            {
                return UInterval.Max;
            }
            set
            {
                UInterval.Max = value;
            }
        }

        public double UMin
        {
            get
            {
                return UInterval.Min;
            }
            set
            {
                UInterval.Min = value;
            }
        }

        public double VMin
        {
            get
            {
                return VInterval.Min;
            }
            set
            {
                VInterval.Min = value;
            }
        }

        public double VMax
        {
            get
            {
                return VInterval.Max;
            }
            set
            {
                VInterval.Max = value;
            }
        }

        public double ULength
        {
            get { return UMax - UMin; }
        }

        public double VLength
        {
            get { return VMax - VMin; }
        }

        /// <summary>
        /// Create a default 2d interval of 0.0 to 1.0 in both U and V directions
        /// </summary>
        public Interval2d()
        {
            UInterval = new Interval();
            VInterval = new Interval();
        }

        /// <summary>
        /// Create a 2D interval using the provided values.
        /// </summary>
        /// <param name="uMin">Minimum value in the U interval.</param>
        /// <param name="uMax">Maximum value in the U interval.</param>
        /// <param name="vMin">Minimum value in the V interval.</param>
        /// <param name="vMax">Maximum value in the V interval.</param>
        public Interval2d(double uMin, double uMax, double vMin, double vMax)
        {
            UInterval = new Interval(uMin, uMax);
            VInterval = new Interval(vMin, vMax);
        }

        public override string ToString()
        {
            return "{ U:" + UMin.ToString() + " To " + UMax.ToString() + "; V:" + VMin.ToString() + " To " + VMax.ToString() + " }";
        }
    }
}
