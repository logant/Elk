using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINE.Geometry
{
    public enum CurveType
    {
        Undefined = -1,

        Line = 0,

        Arc = 1,

        Circle = 2,

        Polyline = 3,

        NurbSpline = 4,

        HermiteSpline = 5
    }

    public class Curve
    {
        public List<Point3d> Points { get; set; }
        public CurveType CurveType { get; set; }
        public int Degree { get; set; }

        public Curve()
        {
        }

        public bool IsPeriodic()
        {
            if (Points.FirstOrDefault().DistanceTo(Points.LastOrDefault()) < double.Epsilon)
                return true;
            else
                return false;
        }

        // TODO: Figure out how to properly cast Curve to other curve types and from other curve types to Curve.
        //public static explicit operator Curve(Line line)
        //{
        //    Curve crv = new Curve();
        //    crv.Points = line.Points;
        //    crv.CurveType = CurveType.Line;
        //    return crv;
        //}

        //public static explicit operator Curve(Arc arc)
        //{
        //    Curve crv = new Curve();
        //    crv.Points = new List<Point3d> {arc.PointAtStart, arc.PointAtEnd, arc.MidPoint};
        //    crv.CurveType = CurveType.Arc;
        //    return crv;
        //}
    }

    public class Line : Curve
    {
        public Point3d PointAtStart
        {
            get { return Points[0]; }
            set { Points[0] = value; }
        }

        public Point3d PointAtEnd
        {
            get { return Points[1]; }
            set { Points[1] = value; }
        }

        /// <summary>
        /// Typical Line constructor with Start and End point.
        /// Direction of line will be determined by a vector 
        /// from the Start to End.
        /// </summary>
        /// <param name="start">Point3d start point for the Line</param>
        /// <param name="end">Point3d end point of the Line.</param>
        public Line(Point3d start, Point3d end)
        {
            Points = new List<Point3d> { start, end };
            CurveType = LINE.Geometry.CurveType.Line;
        }

        /// <summary>
        /// Line constructor via Start, Direction and Length
        /// </summary>
        /// <param name="start">Point3d starting point of the line</param>
        /// <param name="direction">Vector3d direction of the Line.  Can be unitized</param>
        /// <param name="length">double Length of the line along the specified direction vector.</param>
        public Line(Point3d start, Vector3d direction, double length)
        {
            direction.SetMagnitude(length);
            Points = new List<Point3d> { start, start + direction };
            CurveType = LINE.Geometry.CurveType.Line;
        }

        public Vector3d Direction
        {
            get { return new Vector3d(Points[0], Points[1]); }
        }


        public Vector3d UnitTangent
        {
            get
            {
                Vector3d v = new Vector3d(Points[0], Points[1]);
                v.Unitize();
                return v;
            }
        }

        public double Length
        {
            get { return Points[0].DistanceTo(Points[1]); }
        }

        public Point3d PointAt(double t)
        {
            double len = this.Direction.Length * t;
            Vector3d v = this.Direction;
            v.SetMagnitude(len);
            return (Points[0] + v);
        }

        public double ParameterAt(Point3d pt)
        {
            Point3d pt1 = Points[0] - pt;
            Point3d pt2 = Points[1] - Points[0];

            Vector3d v1 = (Vector3d)pt1;
            Vector3d v2 = (Vector3d)pt2;
            double dotProd = Vector3d.DotProduct(v1, v2, false);

            double t = -(dotProd / ((Math.Abs(pt2.X) * Math.Abs(pt2.X)) + (Math.Abs(pt2.Y) * Math.Abs(pt2.Y)) + (Math.Abs(pt2.Z) * Math.Abs(pt2.Z))));
            return t;
        }

        // TODO: Figure out how to cast from Line to Curve and vice versa
        //public static explicit operator Line(Curve crv)
        //{
        //    if (crv.CurveType == CurveType.Line)
        //    {
        //        return new Line(crv.Points[0], crv.Points[1]);
        //    }
        //    else
        //        return null;
        //}

        public Point3d ClosestPoint(Point3d pt)
        {
            Point3d pt1 = Points[0];
            Point3d pt2 = Points[1];

            Vector3d v1 = new Vector3d(pt1, pt2);
            Vector3d v2 = new Vector3d(pt1, pt);
            Vector3d v3 = new Vector3d(pt2, pt);
            double angle = v1.AngleTo(v2);
            Vector3d rev1 = v1.Clone();
            rev1.Reverse();
            double angle2 = rev1.AngleTo(v3);
            
            if (angle > Math.PI / 2)
            {
                return pt1;
            }
            else if (angle < Math.PI / 2 && angle2 < Math.PI / 2)
            {
                double len = Math.Cos(angle) * v2.Length;
                Vector3d transVect = v1.Clone();
                transVect.SetMagnitude(len);
                Point3d point = pt1 + transVect;
                return point;
            }
            else if (v2.Length > v1.Length)
            {
                return pt2;
            }
            else
            {
                Vector3d transVect = v1.Clone();
                transVect.SetMagnitude(v2.Length);
                Point3d point = pt1 + transVect;
                return point;
            }
        }
    }

    public class Arc : Curve
    {
        private Point3d _pointAtStart;
        private Point3d _pointAtEnd;
        private Point3d _centerPoint;
        private Point3d _midPoint;
        private Plane _plane;
        private double _radius;

        public Point3d PointAtStart { get { return _pointAtStart; } }
        public Point3d PointAtEnd { get { return _pointAtEnd; } }
        public Point3d Center { get { return _centerPoint; } }
        public Plane Plane { get { return _plane; } }
        public Point3d MidPoint { get { return _midPoint; } }

        private Point3d _intPt = null;

        public double Radius { get { return _radius; } }

        /// <summary>
        /// Create an arc from an origin plan, radius, and angle.
        /// </summary>
        /// <param name="plane">Origin plane of the arc.</param>
        /// <param name="radius">Radius of the arc.</param>
        /// <param name="angle">Angle of the arc in radians.</param>
        public Arc(Plane plane, double radius, double angle)
        {
            _plane = plane;
            _centerPoint = _plane.Origin;
            _radius = radius;
            
            // Get start point
            Vector3d startVect = _plane.XAxis;
            startVect.SetMagnitude(radius);
            _pointAtStart = _centerPoint + startVect;

            // End Point
            _pointAtEnd = _pointAtStart;
            _pointAtEnd.Rotate(_plane.ZAxis, angle);

            // Midpoint
            _midPoint = _pointAtStart;
            _midPoint.Rotate(_plane.ZAxis, angle / 2);
        }
        
        /// <summary>
        /// Create an arc on an XY plane using centerpoint, radius, and angle.
        /// </summary>
        /// <param name="center">Centerpoint of the arc.</param>
        /// <param name="radius">Radius of the arc.</param>
        /// <param name="angle">Angle of the arc in radians.</param>
        public Arc(Point3d center, double radius, double angle)
        {
            Plane plane = new Plane(center, Vector3d.ZAxis);
            _plane = plane;
            _centerPoint = _plane.Origin;
            _radius = radius;

            // Get start point
            Vector3d startVect = _plane.XAxis;
            startVect.SetMagnitude(radius);
            _pointAtStart = _centerPoint + startVect;

            // End Point
            _pointAtEnd = _pointAtStart;
            _pointAtEnd.Rotate(_plane.ZAxis, angle);

            // Midpoint
            _midPoint = _pointAtStart;
            _midPoint.Rotate(_plane.ZAxis, angle / 2);
        }

        /// <summary>
        /// Create an Arc from a start, end, and intermediate point
        /// </summary>
        /// <param name="startPoint">Start point of the arc.</param>
        /// <param name="endPoint">End point of the arc.</param>
        /// <param name="intermediatePoint">Point the start and end points somewhere along the arc.</param>
        public Arc(Point3d startPoint, Point3d endPoint, Point3d intermediatePoint)
        {
            _pointAtStart = startPoint;
            _pointAtEnd = endPoint;
            _intPt = intermediatePoint;

            CalculateRadius();
            CalculateCenterPoint();
        }

        private void CalculateRadius()
        {
            // Calculate area of a triangle via Heron's Formula
            double halfPerim = (PointAtStart.DistanceTo(_intPt) + _intPt.DistanceTo(PointAtEnd) + PointAtEnd.DistanceTo(PointAtStart)) / 2;
            double area = Math.Sqrt(halfPerim * (halfPerim - (PointAtStart.DistanceTo(_intPt))) * (halfPerim - (_intPt.DistanceTo(PointAtEnd))) * (halfPerim - (PointAtEnd.DistanceTo(PointAtStart))));

            _radius = (PointAtStart.DistanceTo(_intPt) * _intPt.DistanceTo(PointAtEnd) * PointAtEnd.DistanceTo(PointAtStart)) / (area * 4);
        }

        private void CalculateCenterPoint()
        {
            Vector3d vect1 = new Vector3d(_intPt, PointAtStart);
            Vector3d vect2 = new Vector3d(_intPt, PointAtEnd);
            Point3d midPt1 = new Point3d((PointAtStart.X + _intPt.X) / 2, (PointAtStart.Y + _intPt.Y) / 2, (PointAtStart.Z + _intPt.Z) / 2);
            Point3d midPt2 = new Point3d((PointAtEnd.X + _intPt.X) / 2, (PointAtEnd.Y + _intPt.Y) / 2, (PointAtEnd.Z + _intPt.Z) / 2);

            Vector3d normal = Vector3d.CrossProduct(vect1, vect2);

            vect1.Rotate(Math.PI / 2, normal);
            vect2.Rotate(-Math.PI / 2, normal);
            vect1.SetMagnitude(Radius);
            vect2.SetMagnitude(Radius);

            double vectorScalar = Vector3d.CrossProduct(new Vector3d(midPt2, midPt1), vect2).Length / Vector3d.CrossProduct(vect1, vect2).Length;
            vect1 = vect1 * vectorScalar;
            _centerPoint = midPt1 + vect1;
        }

        public double Length()
        {
            Vector3d startVect = new Vector3d(_centerPoint, _pointAtStart);
            Vector3d endVect = new Vector3d(_centerPoint, _pointAtEnd);

            double angle = startVect.AngleTo(endVect);
            return (angle * _radius);
        }

        // TODO: Figure out how to cast from Arc to Curve and vice versa.
        //public static explicit operator Arc(Curve crv)
        //{
        //    if (crv.CurveType == CurveType.Arc)
        //    {
        //        Arc arc = new Arc(crv.Points[0], crv.Points[1], crv.Points[2]);
        //        return arc;
        //    }
        //    else
        //        return null;
        //}

        public Point3d ClosestPoint(Point3d pt)
        {
            // Project the point onto a plane
            Point3d projected = _plane.ProjectTo(pt);

            // Get a vector from the _centerPoint to the projected point and
            // Scale it according to the arc's radius.
            Vector3d vect = new Vector3d(_centerPoint, projected);
            vect.SetMagnitude(_radius);

            // Move the _centerPoint by the vector
            Point3d cp = _centerPoint + vect;

            // Compare the distance of this closest point and the start and end 
            // points to the original projected point.  Return the point corresponding
            // to the shorted distance.
            List<Point3d> possiblePoints = new List<Point3d> { cp, _pointAtStart, _pointAtEnd };
            possiblePoints.Sort((x, y) => x.DistanceTo(projected).CompareTo(y.DistanceTo(projected)));

            return possiblePoints[0];
        }
    }
}
