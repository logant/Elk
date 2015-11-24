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
        #region Curve Properties [Degree, IsClosed, IsPeriodic, CurveType, PointAtStart, PointAtEnd, TangentAtStart, TangentAtEnd]
        public int Degree { get; set; }

        public bool IsClosed
        {
            get
            { 
                bool closed = false;

                switch (CurveType)
                {
                    case CurveType.Line:
                        closed = false;
                        break;
                    case CurveType.Arc:
                        closed = false;
                        break;
                    case CurveType.Circle:
                        closed = true;
                        break;
                    case CurveType.Polyline:
                        if (PointAtStart.DistanceTo(PointAtEnd) < double.Epsilon)
                            closed = true;
                        else
                            closed = false;
                        break;
                    case CurveType.NurbSpline:
                        if (PointAtStart.DistanceTo(PointAtEnd) < double.Epsilon)
                            closed = true;
                        else
                            closed = false;
                        break;
                    case CurveType.HermiteSpline:
                        if (PointAtStart.DistanceTo(PointAtEnd) < double.Epsilon)
                            closed = true;
                        else
                            closed = false;
                        break;
                    default:
                        closed = false;
                        break;
                }

                return closed;
            }
        }

        public bool IsPeriodic { get; set; }
        
        //public virtual bool IsPeriodic()
        //{
        //    bool periodic = false;

        //    switch (CurveType)
        //    {
        //        case CurveType.Line:
        //            periodic = false;
        //            break;
        //        case CurveType.Arc:
        //            periodic = false;
        //            break;
        //        case CurveType.Circle:
        //            periodic = true;
        //            break;
        //        case CurveType.Polyline:
        //            periodic = false;
        //            break;
        //        case CurveType.NurbSpline:
        //            if (PointAtStart.DistanceTo(PointAtEnd) < double.Epsilon)
        //            {
        //                int dupPoints = (Degree - 1) / 2;
        //                bool testPeriodic = true;
        //                for(int i = 0; i < dupPoints; i++)
        //                {
        //                    if (Points[i].DistanceTo(Points[Points.Count - (dupPoints + i)]) > double.Epsilon)
        //                    {
        //                        testPeriodic = false;
        //                        break;
        //                    }
        //                }
        //                periodic = testPeriodic;
        //            }
        //            else
        //                periodic = false;
        //            break;
        //        case CurveType.HermiteSpline:
        //            periodic = false;
        //            break;
        //        default:
        //            periodic = false;
        //            break;
        //    }
        //    return periodic;
        //}

        public CurveType CurveType { get; set; }

        public virtual Point3d PointAtStart
        {
            get { return Points[0]; }
        }

        public virtual Point3d PointAtEnd
        {
            get { return Points[Points.Count - 1]; }
        }

        public Vector3d TangentAtStart
        {
            get
            {
                Vector3d tangent = null;

                switch (CurveType)
                {
                    case CurveType.Line:
                        Vector3d lineVect = new Vector3d(PointAtStart, PointAtEnd);
                        lineVect.Unitize();
                        tangent = lineVect;
                        break;
                    case CurveType.Arc:
                        //tangent = false;
                        
                        break;
                    case CurveType.Circle:
                        //tangent = true;
                        break;
                    case CurveType.Polyline:
                        //if (PointAtStart.DistanceTo(PointAtEnd) < double.Epsilon)
                        //    tangent = true;
                        //else
                        //    tangent = false;
                        break;
                    case CurveType.NurbSpline:
                        //if (PointAtStart.DistanceTo(PointAtEnd) < double.Epsilon)
                        //    tangent = true;
                        //else
                        //    tangent = false;
                        break;
                    case CurveType.HermiteSpline:
                        //if (PointAtStart.DistanceTo(PointAtEnd) < double.Epsilon)
                        //    tangent = true;
                        //else
                        //    tangent = false;
                        break;
                    default:
                        tangent = null;
                        break;
                }

                return tangent;
            }
        }

        public Vector3d TangentAtEnd
        {
            get
            {
                return Vector3d.XAxis;
            }
        }
        #endregion

        public Curve()
        {
        }


        internal List<Point3d> Points { get; set; }
        internal List<double> Weights { get; set; }
        internal List<double> Knots { get; set; }

        #region Curve Methods [ParameterAt, PointAt, TangentAt, Length, ClosestPoint, IsPlanar, ToNurbsCurve]
        public virtual double ParameterAt(Point3d point)
        {
            double parameter = 0.0;
            switch(CurveType)
            {
                case CurveType.Line:
                    Point3d pt1 = Points[0] - point;
                    Point3d pt2 = Points[1] - Points[0];

                    Vector3d v1 = (Vector3d)pt1;
                    Vector3d v2 = (Vector3d)pt2;
                    double dotProd = Vector3d.DotProduct(v1, v2, false);

                    double t = -(dotProd / ((Math.Abs(pt2.X) * Math.Abs(pt2.X)) + (Math.Abs(pt2.Y) * Math.Abs(pt2.Y)) + (Math.Abs(pt2.Z) * Math.Abs(pt2.Z))));
                    parameter = t;
                    break;
            }
            return parameter;
        }

        public virtual Point3d PointAt(double parameter)
        {
            Point3d point = Point3d.Origin;

            switch(CurveType)
            {
                case CurveType.Line:
                    break;
                case CurveType.Arc:
                    break;
                case CurveType.Circle:
                    break;
                case CurveType.Polyline:
                    break;
                case CurveType.NurbSpline:
                    break;
                case CurveType.HermiteSpline:
                    break;
                default:
                    break;
            }

            return point;
        }

        public virtual Vector3d TangentAt(double parameter)
        {
            Vector3d tangent = Vector3d.XAxis;

            switch (CurveType)
            {
                case CurveType.Line:
                    break;
                case CurveType.Arc:
                    break;
                case CurveType.Circle:
                    break;
                case CurveType.Polyline:
                    break;
                case CurveType.NurbSpline:
                    break;
                case CurveType.HermiteSpline:
                    break;
                default:
                    break;
            }

            return tangent;
        }

        public virtual double Length
        {
            get
            {
                double length = 0.0;
                switch (CurveType)
                {
                    case CurveType.Line:
                        length = PointAtStart.DistanceTo(PointAtEnd);
                        break;
                    case CurveType.Arc:
                        break;
                    case CurveType.Circle:
                        break;
                    case CurveType.Polyline:
                        break;
                    case CurveType.NurbSpline:
                        break;
                    case CurveType.HermiteSpline:
                        break;
                    default:
                        break;
                }
                return length;
            }
        }

        public virtual Point3d ClosestPoint(Point3d point)
        {
            Point3d closestPoint = Point3d.Origin;

            switch (CurveType)
            {
                case CurveType.Line:
                    Line line = null;
                    TryCastLine(out line);
                    if (line != null)
                        closestPoint = line.ClosestPoint(point);
                    break;
                case CurveType.Arc:
                    Arc arc = null;
                    TryCastArc(out arc);
                    if (arc != null)
                        closestPoint = arc.ClosestPoint(point);
                    break;
                case CurveType.Circle:
                    break;
                case CurveType.Polyline:
                    break;
                case CurveType.NurbSpline:
                    break;
                case CurveType.HermiteSpline:
                    break;
                default:
                    break;
            }

            return closestPoint;
        }

        public virtual bool IsPlanar()
        {
            bool planar = false;

            switch (CurveType)
            {
                case CurveType.Line:
                    planar = true;
                    break;
                case CurveType.Arc:
                    planar = true;
                    break;
                case CurveType.Circle:
                    planar = true;
                    break;
                case CurveType.Polyline:
                    break;
                case CurveType.NurbSpline:
                    break;
                case CurveType.HermiteSpline:
                    break;
                default:
                    break;
            }

            return planar;
        }

        internal virtual NurbsCurve ToNurbsCurve(Curve c)
        {
            NurbsCurve nc = null;
            switch(CurveType)
            {
                case CurveType.Line:
                    break;
                case CurveType.Arc:
                    break;
                case CurveType.Circle:
                    break;
                case CurveType.Polyline:
                    break;
                case CurveType.NurbSpline:
                    nc = (NurbsCurve)c;
                    break;
                case CurveType.HermiteSpline:
                    break;
            }
            return nc;
        }
        #endregion

        #region CastingMethods [TryCastLine, TryCastArc, TryCastCircle, TryCastPolyLine]
        public bool TryCastLine(out Line line)
        {
            if (CurveType == CurveType.Line)
            {
                line = (Line)this;
                return true;
            }
            else
            {
                line = null;
                return false;
            }
        }

        public bool TryCastArc(out Arc arc)
        {
            if(CurveType == CurveType.Arc)
            {
                arc = (Arc)this;
                return true;
            }
            else
            {
                arc = null;
                return false;
            }
        }

        public bool TryCastCircle(out Circle circle)
        {
            if(CurveType == CurveType.Circle)
            {
                circle = (Circle)this;
                return true;
            }
            else
            {
                circle = null;
                return false;
            }
        }

        public bool TryCastPolyline(out Polyline pline)
        {
            if (CurveType == CurveType.Polyline)
            {
                pline = (Polyline)this;
                return true;
            }
            else
            {
                pline = null;
                return false;
            }
        }
        #endregion
    }

    public class Line : Curve
    {
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
            Degree = 1;
            CurveType = CurveType.Line;
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
            Degree = 1;
            CurveType = CurveType.Line;
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

        public override double Length
        {
            get { return Points[0].DistanceTo(Points[1]); }
        }

        public override Point3d PointAt(double t)
        {
            double len = this.Direction.Length * t;
            Vector3d v = this.Direction;
            v.SetMagnitude(len);
            return (Points[0] + v);
        }

        public override double ParameterAt(Point3d pt)
        {
            Point3d pt1 = Points[0] - pt;
            Point3d pt2 = Points[1] - Points[0];

            Vector3d v1 = (Vector3d)pt1;
            Vector3d v2 = (Vector3d)pt2;
            double dotProd = Vector3d.DotProduct(v1, v2, false);

            double t = -(dotProd / ((Math.Abs(pt2.X) * Math.Abs(pt2.X)) + (Math.Abs(pt2.Y) * Math.Abs(pt2.Y)) + (Math.Abs(pt2.Z) * Math.Abs(pt2.Z))));
            return t;
        }

        public override Point3d ClosestPoint(Point3d pt)
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

        public override Point3d PointAtStart { get { return _pointAtStart; } }
        public override Point3d PointAtEnd { get { return _pointAtEnd; } }
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

            Points = new List<Point3d> { _pointAtStart, _centerPoint, _pointAtEnd };
            Degree = 2;
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

            Points = new List<Point3d> { _pointAtStart, _midPoint, _pointAtEnd };
            Degree = 2;
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

            Points = new List<Point3d> { _pointAtStart, _midPoint, _pointAtEnd };
            Degree = 2;
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

        public override double Length
        {
            get
            {
                Vector3d startVect = new Vector3d(_centerPoint, _pointAtStart);
                Vector3d endVect = new Vector3d(_centerPoint, _pointAtEnd);

                double angle = startVect.AngleTo(endVect);
                return (angle * _radius);
            }
        }

        public override Point3d ClosestPoint(Point3d pt)
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

    public class Circle : Curve
    {
        private Plane _plane;
        private double _radius;

        public Plane Plane
        {
            get { return _plane; }
            set { _plane = value; }
        }

        public Point3d Origin
        {
            get { return _plane.Origin; }
        }

        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public double Diameter
        {
            get { return _radius * 2; }
            set { _radius = value / 2;  }
        }

        public override double Length
        {
            get { return 2 * Math.PI * _radius; }
        }

        public Circle(Plane plane, double radius)
        {
            _plane = plane;
            _radius = radius;
        }

        public Circle(Point3d ptA, Point3d ptB, Point3d ptC)
        {
            CalculateRadius(ptA, ptB, ptC);
            CalculateCenterPoint(ptA, ptB, ptC);
        }

        public Circle(Point3d centerPt, double radius)
        {
            _plane = Plane.XYPlane(centerPt);
            _radius = radius;
        }

        public void CalculateRadius(Point3d startPt, Point3d intPt, Point3d endPt)
        {
            // Calculate area of a triangle via Heron's Formula
            double halfPerim = (startPt.DistanceTo(intPt) + intPt.DistanceTo(endPt) + endPt.DistanceTo(startPt)) / 2;
            double area = Math.Sqrt(halfPerim * (halfPerim - (startPt.DistanceTo(intPt))) * (halfPerim - (intPt.DistanceTo(endPt))) * (halfPerim - (endPt.DistanceTo(startPt))));

            _radius = (startPt.DistanceTo(intPt) * intPt.DistanceTo(endPt) * endPt.DistanceTo(startPt)) / (area * 4);
        }

        private void CalculateCenterPoint(Point3d startPt, Point3d intPt, Point3d endPt)
        {
            // Create vectors from the intermediate point and the outside points
            Vector3d vect1 = new Vector3d(intPt, startPt);
            Vector3d vect2 = new Vector3d(intPt, endPt);

            // Get the midpoint between outside points and intermediate point along their respective vectors
            Point3d midPt1 = new Point3d((startPt.X + intPt.X) / 2, (startPt.Y + intPt.Y) / 2, (startPt.Z + intPt.Z) / 2);
            Point3d midPt2 = new Point3d((endPt.X + intPt.X) / 2, (endPt.Y + intPt.Y) / 2, (endPt.Z + intPt.Z) / 2);

            // Get the planar normal between these three points
            Vector3d normal = Vector3d.CrossProduct(vect1, vect2);

            // Create a vector rotated 90°.  These vectors will point from the midpoints to the center.
            vect1.Rotate(Math.PI / 2, normal);
            vect2.Rotate(-Math.PI / 2, normal);
            vect1.SetMagnitude(_radius);
            vect2.SetMagnitude(_radius);

            // Determine the point along the vectors where the two vectors cross.  The scalar can then be used to determine a scalar to get to the actual centerpoint.
            double vectorScalar = Vector3d.CrossProduct(new Vector3d(midPt2, midPt1), vect2).Length / Vector3d.CrossProduct(vect1, vect2).Length;
            vect1 = vect1 * vectorScalar;
            Point3d centerPt = midPt1 + vect1;

            // Generate the plane for the circle
            _plane = new Plane(centerPt, normal);
        }
    }

    public class Polyline : Curve
    {
        private List<Point3d> _vertices;

        public List<Point3d> Vertices
        {
            get { return _vertices; }
        }
        
        public Polyline(List<Point3d> vertices, bool closed)
        {
            if(vertices.Count > 1)
            {
                if(closed)
                {
                    if(vertices[0].DistanceTo(vertices[vertices.Count - 1]) < double.Epsilon)
                    {
                        vertices.Add(vertices[0]);
                    }
                }
                _vertices = vertices;
            }
        }

        public Polyline(List<Point3d> vertices)
        {
            if (vertices.Count > 1)
            {
                if (vertices[0].DistanceTo(vertices[vertices.Count - 1]) < double.Epsilon)
                {
                    vertices.Add(vertices[0]);
                }
                _vertices = vertices;
            }
        }

        public override double Length
        {
            get
            {
                double length = 0;

                for (int i = 0; i < _vertices.Count; i++)
                {
                    if (i == _vertices.Count - 1)
                    {
                        if (this.IsClosed)
                        {
                            Point3d pt1 = _vertices[i];
                            length += pt1.DistanceTo(_vertices[0]);
                        }
                        else
                        {
                            Point3d pt1 = _vertices[i];
                            length += pt1.DistanceTo(_vertices[i + 1]);
                        }
                    }
                }
                return length;
            }
        }

        public bool IsPlanar(double tolerance)
        {
            if (_vertices.Count <= 3)
                return true;

            // Get a normal vector to test all points
            Vector3d vect1 = new Vector3d(_vertices[0], _vertices[1]);
            Vector3d vect2 = new Vector3d(_vertices[1], _vertices[2]);
            Vector3d normVector = Vector3d.CrossProduct(vect1, vect2);

            for (int i = 1; i < _vertices.Count - 2; i++)
            {
                vect1 = new Vector3d(_vertices[i], _vertices[i + 1]);
                vect2 = new Vector3d(_vertices[i], _vertices[i + 2]);
                Vector3d testVector = Vector3d.CrossProduct(vect1, vect2);
                Vector3d reversedTestVector = testVector.Clone();
                reversedTestVector.Reverse();

                if (Math.Abs(testVector.AngleTo(normVector)) > tolerance && Math.Abs(normVector.AngleTo(reversedTestVector)) > tolerance)
                    return false;
            }

            return true;
        }
    }

    public class NurbsCurve : Curve
    {
        private List<ControlPoint> _controlPoints;

        public List<ControlPoint> ControlPoints
        {
            get { return _controlPoints; }
            set { _controlPoints = value; }
        }



        public NurbsCurve(List<Point3d> points, int degree)
        {
            List<ControlPoint> controlPoints = new List<ControlPoint>();
            foreach(Point3d pt in points)
            {
                controlPoints.Add(new ControlPoint(pt, 1.0));
                Points.Add(pt);
                Weights.Add(1.0);
            }
            _controlPoints = controlPoints;
            
            // Create the knots
            if (points[0].DistanceTo(points[points.Count - 1]) > double.Epsilon)
                Knots = CreateOpenKnots(degree, points.Count);
            else
                Knots = CreateClosedKnots(degree, points.Count);

            Degree = degree;
            CurveType = CurveType.NurbSpline;
        }

        public NurbsCurve(List<Point3d> points, List<double> weights, List<double> knots, int degree, bool closed)
        {
            List<ControlPoint> controlPoints = new List<ControlPoint>();
            if(weights.Count == points.Count)
            {
                for(int i = 0; i < weights.Count; i++)
                {
                    ControlPoint cp = new ControlPoint(points[i], weights[i]);
                    controlPoints.Add(cp);
                }
                if(closed && points[0].DistanceTo(points[points.Count - 1]) > double.Epsilon)
                {
                    controlPoints.Add(controlPoints[0]);
                }
            }

            _controlPoints = controlPoints;
            Weights = weights;
            Points = points;
            Degree = degree;
            Knots = knots;
            CurveType = CurveType.NurbSpline;
        }

        public List<double> CreateOpenKnots(int degree, int ptCount)
        {
            List<double> knots = new List<double>();

            int knotCount = degree + ptCount + 1;
            int firstKnots = degree + 1;
            int lastKnots = degree + 1;
            int midKnots = knotCount - firstKnots - lastKnots;

            for(int i = 0; i < firstKnots; i++)
            {
                knots.Add(0.0);
            }

            for(int i = 0; i < midKnots; i++)
            {
                knots.Add((double)i + 1.0);
            }

            for(int i = 0; i < lastKnots; i++)
            {
                knots.Add((double)midKnots + 1.0);
            }
            return knots;
        }

        public List<double> CreateClosedKnots(int degree, int ptCount)
        {
            List<double> knots = new List<double>();

            int knotCount = degree + ptCount + 1;
            for (int i = 0; i < knotCount; i++)
            {
                knots.Add((double)i);
            }

            return knots;
        }
    }
}
