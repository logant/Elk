using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using Elk.Common;
using Dynamo.Models;
using DSCoreNodesUI;
using System.ComponentModel;



namespace Elk
{
    public static class OSM
    {
        static string way = "building";
        static Elk.Common.ElkLib.FeatureType wayFT = ElkLib.FeatureType.Building;

        static List<string> selectedTypes = new List<string>();

        static List<string> featureTypes = (Elk.Common.ElkLib.featureKeys).ToList();

        static bool osmTrigger = false;

        private static readonly BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// Process OpenStreetMap exports for use in other Elk components
        /// </summary>
        /// <param name="filePath">File path for the OSM file.</param>
        /// <returns name="OSM">Holds point and identification OSM data.</returns>
        /// <returns name="XML">The full XML data from the OSM File.</returns>
        /// <returns name="Loc">MIN and MAX longitude domain fround in the OSM file.</returns>
        /// <search>elk, location, loc, latitude, longitude, osm</search>
        [MultiReturn("OSM", "XML", "Loc")]
        public static Dictionary<string, object> Location(string filePath)
        {
            List<OSMPoint> osmPoints = new List<OSMPoint>();
            LINE.Geometry.Interval2d latlon = new LINE.Geometry.Interval2d();
            string dataStream = string.Empty;

            // Get the Revit Units for scale
            double scale = 1.0;
            string exception = null;
            try
            {
                scale = Elk.DynamoCommon.GetRevitUnitScale();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }
            if (exception == null)
                exception = scale.ToString();

            string path = filePath;

            if (path != null && System.IO.File.Exists(path))
            {
                ElkLib.NodePreProcess(path, scale, out dataStream, out osmPoints, out latlon);
            }

            return new Dictionary<string, object>
            {
                {"OSM", osmPoints},
                {"XML", path},
                {"Loc", latlon}
                //{"test", exception}
            };
        }
                
        /// <summary>
        /// This component will pull osm data related to a selected FeatureType.
        /// </summary>
        /// <param name="OSM">OSM Points from the Location node</param>
        /// <param name="XML">XML String from the Location node.</param>
        /// <returns name="Points">Points representing from Ways, Multipolygons, and Nodes.</returns>
        /// <returns name="Keys">Keys that represent all tag data from the OSM file.</returns>
        /// <search>elk, building, bldg, build, osm</search>
        [MultiReturn("Points", "Keys")]
        public static Dictionary<string, object> OSMData(List<OSMPoint> OSM, string File, string featureType, List<string> subTypes)
        {
            // Convert the string featureType to the FeatureType wayFT.
            if (featureType != way)
            {
                wayFT = ElkLib.ByName(featureType);
                way = wayFT.ToString();
            }

            selectedTypes = subTypes;
            if (selectedTypes.Count == 0)
            {
                selectedTypes = new List<string> { "*" };
            }

            way = wayFT.ToString().ToLower();

            // Gather the ways
            List<OSMWay> collectedWays = ElkLib.GatherWays(way, selectedTypes.ToArray(), File, OSM);
            Point[][][] wayPoints = new Point[collectedWays.Count][][];
            List<List<string>> tags = new List<List<string>>();
            try
            {
                // Convert all of the OSMPoints to Dynamo Points
                for (int i = 0; i < collectedWays.Count; i++)
                {
                    OSMWay currentWay = collectedWays[i];
                    wayPoints[i] = new Point[currentWay.Points.Count][];
                    for (int j = 0; j < currentWay.Points.Count; j++)
                    {
                        try
                        {
                            List<LINE.Geometry.Point3d> pointList = currentWay.Points[j];
                            wayPoints[i][j] = new Point[pointList.Count];
                            for (int k = 0; k < pointList.Count; k++)
                            {
                                LINE.Geometry.Point3d pt = pointList[k];
                                try
                                {
                                    wayPoints[i][j][k] = Point.ByCoordinates(pt.X, pt.Y, pt.Z);
                                }
                                catch
                                {
                                    wayPoints[i][j][k] = Point.Origin();
                                }
                            }
                        }
                        catch { }
                    }

                    // Collect all of the tags
                    List<string> tagList = new List<string>();
                    foreach (KeyValuePair<string, string> tag in currentWay.Tags)
                    {
                        try
                        {
                            string currentTag = tag.Key + ":" + tag.Value;
                            tagList.Add(currentTag);
                        }
                        catch { }
                    }
                    tags.Add(tagList);
                }
            }
            catch { }

            return new Dictionary<string, object>
            {
                {"Points", wayPoints},
                {"Keys", tags}
            };
        }

        /// <summary>
        /// This node will return a list of valid feature types.
        /// </summary>
        /// <returns>A list of feature types that you can select via List.GetItemAtIndex</returns>
        /// <search>elk,osm,featuretype,feature type,features</search>
        public static List<string> Features()
        {
            return Elk.Common.ElkLib.featureKeys.ToList();
        }

        /// <summary>
        /// Get the subfeatures of a desired feature.
        /// </summary>
        /// <param name="feature">Feature you want to get subfeatures for.</param>
        /// <param name="subIndexes">A list of integers related to the subfeature indexs of the items you want.</param>
        /// <returns></returns>
        [MultiReturn("all", "selected")]
        public static Dictionary<string, List<string>> SubFeatures(string feature, List<int> subIndexes)
        {
            #region Get Available Types
            List<string> availableTemp = new List<string>();
            ElkLib.FeatureType featureType = ElkLib.ByName(feature);
            switch (featureType)
            {
                case ElkLib.FeatureType.Aerialway:
                    availableTemp = Elk.Common.ElkLib.Aerialways;
                    break;
                case ElkLib.FeatureType.Aeroway:
                    availableTemp = Elk.Common.ElkLib.Aeroways;
                    break;
                case ElkLib.FeatureType.Amenity:
                    availableTemp = Elk.Common.ElkLib.Amenities;
                    break;
                case ElkLib.FeatureType.Barrier:
                    availableTemp = Elk.Common.ElkLib.Barrier;
                    break;
                case ElkLib.FeatureType.Boundary:
                    availableTemp = Elk.Common.ElkLib.Boundary;
                    break;
                case ElkLib.FeatureType.Building:
                    availableTemp = Elk.Common.ElkLib.Building;
                    break;
                case ElkLib.FeatureType.Craft:
                    availableTemp = Elk.Common.ElkLib.Craft;
                    break;
                case ElkLib.FeatureType.Emergency:
                    availableTemp = Elk.Common.ElkLib.Emergency;
                    break;
                case ElkLib.FeatureType.Geological:
                    availableTemp = Elk.Common.ElkLib.Geological;
                    break;
                case ElkLib.FeatureType.Highway:
                    availableTemp = Elk.Common.ElkLib.Highway;
                    break;
                case ElkLib.FeatureType.Historic:
                    availableTemp = Elk.Common.ElkLib.Historic;
                    break;
                case ElkLib.FeatureType.Landuse:
                    availableTemp = Elk.Common.ElkLib.Landuse;
                    break;
                case ElkLib.FeatureType.Leisure:
                    availableTemp = Elk.Common.ElkLib.Leisure;
                    break;
                case ElkLib.FeatureType.ManMade:
                    availableTemp = Elk.Common.ElkLib.ManMade;
                    break;
                case ElkLib.FeatureType.Military:
                    availableTemp = Elk.Common.ElkLib.Military;
                    break;
                case ElkLib.FeatureType.Natural:
                    availableTemp = Elk.Common.ElkLib.Natural;
                    break;
                case ElkLib.FeatureType.Office:
                    availableTemp = Elk.Common.ElkLib.Office;
                    break;
                case ElkLib.FeatureType.Place:
                    availableTemp = Elk.Common.ElkLib.Places;
                    break;
                case ElkLib.FeatureType.Power:
                    availableTemp = Elk.Common.ElkLib.Power;
                    break;
                case ElkLib.FeatureType.PublicTransport:
                    availableTemp = Elk.Common.ElkLib.PublicTransport;
                    break;
                case ElkLib.FeatureType.Railway:
                    availableTemp = Elk.Common.ElkLib.Railway;
                    break;
                case ElkLib.FeatureType.Route:
                    availableTemp = Elk.Common.ElkLib.Route;
                    break;
                case ElkLib.FeatureType.Shop:
                    availableTemp = Elk.Common.ElkLib.Shop;
                    break;
                case ElkLib.FeatureType.Sport:
                    availableTemp = Elk.Common.ElkLib.Sport;
                    break;
                case ElkLib.FeatureType.Tourism:
                    availableTemp = Elk.Common.ElkLib.Toursim;
                    break;
                case ElkLib.FeatureType.Waterway:
                    availableTemp = Elk.Common.ElkLib.Waterway;
                    break;
            };
            availableTemp.Sort();
            #endregion

            List<string> selected = new List<string>();
            if (subIndexes == null || subIndexes.Count == 0)
            {
                selected = new List<string> { "*" };
            }
            else
            {
                foreach (int index in subIndexes)
                {
                    try
                    {
                        selected.Add(availableTemp[index]);
                    }
                    catch { }
                }
            }

            return new Dictionary<string, List<string>>
            {
                {"all", availableTemp},
                {"selected", selected}
            };
        }

        

    }


    public static class Topography
    {
        /// <summary>
        /// Get a Topography from SRTM or IMG files
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="location"></param>
        /// <returns name="Points">A multi-dimensional array of points that make a surface</returns>
        /// <returns name="Curves">An array of equally spaced curves that make a surface</returns>
        /// <returns name="Surface">The resulting surface from the elevationd ata and specified latitude and longitude domains.</returns>
        /// <search>elk,topography,topo,srtm,usgs</search>
        [MultiReturn("Info", "Points", "Curves", "Surface")]
        public static Dictionary<string, object> CreateTopo(string filePath, LINE.Geometry.Interval2d location = null)
        {
            // Get the file information
            List<string> fileInfo = Elk.Common.ElkLib.TopoFileInfo(filePath);

            // output parameters
            List<List<Point>> topoPoints = null;
            List<NurbsCurve> curves = null;
            NurbsSurface ns = null;
            PolySurface ps = null;
            
            // try to get the scale
            double scale = 1.0;
            try
            {
                scale = Elk.DynamoCommon.GetRevitUnitScale();
            }
            catch { }



            if (filePath != null && System.IO.File.Exists(filePath) && location != null)
            {
                List<List<LINE.Geometry.Point3d>> pts = ElkLib.ProcessTopoFile(filePath, scale, location);

                Point[][] surfPoints = new Point[pts.Count][];
                topoPoints = new List<List<Point>>();
                curves = new List<NurbsCurve>();
                for (int i = 0; i < pts.Count; i++)
                {
                    List<LINE.Geometry.Point3d> rowPoints = pts[i];
                    List<Point> crvPts = new List<Point>();

                    for (int j = 0; j < rowPoints.Count; j++)
                    {
                        Point dynPoint = Point.ByCoordinates(rowPoints[j].X, rowPoints[j].Y, rowPoints[j].Z);
                        crvPts.Add(dynPoint);
                    }
                    surfPoints[i] = crvPts.ToArray();
                    topoPoints.Add(crvPts);
                    NurbsCurve nc = NurbsCurve.ByPoints(crvPts, 3);
                    //PolyCurve pc = PolyCurve.ByPoints(crvPts, false);
                    curves.Add(nc);
                }
                try
                {

                    ns = NurbsSurface.ByPoints(surfPoints, 3, 3);
                }
                catch
                {
                    ns = null;
                }
                return new Dictionary<string, object>
                {
                    {"Info", fileInfo},
                    {"Points", topoPoints},
                    {"Curves", curves},
                    {"Surface", ns}
                };
            }
            else
            {
                return new Dictionary<string, object>
            {
                {"Info", fileInfo},
                {"Points", null},
                {"Curves", null},
                {"Surface", null}
            };
            }
            
        }

        /// <summary>
        /// Deconstruct the latitude/longitude from an Elk Location component.
        /// </summary>
        /// <param name="location">2d Domain for the region</param>
        /// <returns name="south">Southern latitude coordinate</returns>
        /// <returns name="north">Northern latitude coordinate</returns>
        /// <returns name="west">Western longitude coordinate</returns>
        /// <returns name="east">Western longitude coordinate</returns>
        /// <search>deconstruct,location,deloc,elk,topo,topography</search>
        [MultiReturn("west", "east", "south", "north")]
        public static Dictionary<string, double> DeconstructLocation(LINE.Geometry.Interval2d location)
        {
            return new Dictionary<string, double>
            {
                {"west", location.UMin},
                {"east", location.UMax},
                {"south", location.VMin},
                {"north", location.VMax}
            };
        }

        /// <summary>
        /// Construct a location of latitude/longitude for the Elk topography component.
        /// </summary>
        /// <param name="west">Western longitude parameter (Min)</param>
        /// <param name="east">Eastern longitude parameter (Max)</param>
        /// <param name="south">Southern latitude parameter (Min)</param>
        /// <param name="north">Northern latitude parameter (Max)</param>
        /// <returns name="location">The location's 2 dimneional domain (latitude/longitude)</returns>
        /// <search>location,construct,elk,topography</search>
        public static LINE.Geometry.Interval2d ConstructLocation(double west, double east, double south, double north)
        {
            LINE.Geometry.Interval2d loc = new LINE.Geometry.Interval2d(west, east, south, north);
            return loc;
        }
    }
}
