using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINE.Geometry;
using System.Collections.ObjectModel;

namespace Elk.Common
{
    public class OSMPoint
    {
        // Properties
        public string ID { get; set; }

        public Point3d Point { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Key { get; set; }

        public string OSMKey { get; set; }

        public string OSMValue { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }

    public class OSMWay
    {
        public List<List<Point3d>> Points { get; set; }

        public string ID { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public double Height { get; set; }

        public double MinHeight { get; set; }

        public int Levels { get; set; }

        public int MinLevel { get; set; }

        public Dictionary<string, string> Tags { get; set; }
    }
}
