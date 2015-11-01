using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using Elk.Common;

namespace Elk.GHComponents
{
    public class HighwayComponentOBSOLETE : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the HighwayComponent class.
        /// </summary>
        public HighwayComponentOBSOLETE()
            : base("Highways", "HWY",
                "Get highway paths from the OSM file.",
                "Extra", "Elk2")
        {
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.hidden;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new OSMPointParam(), "OSM Point Data", "O", "Holds preprocessed point and identification data from the OSM file.", GH_ParamAccess.list);
            pManager.AddTextParameter("OSM Data (text)", "X", "XML Data pulled directly from the OSM file.", GH_ParamAccess.item, string.Empty);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Motorways", "M", "Motorway (Major Highways) from the OSM file.", GH_ParamAccess.tree);
            pManager.AddPointParameter("Motorway Links", "ML", "Motorway (Major Highways) from the OSM file.", GH_ParamAccess.tree);
            pManager.AddPointParameter("Trunks", "T", "Motorway (Major Highways) from the OSM file.", GH_ParamAccess.tree);
            pManager.AddPointParameter("Trunk Links", "TL", "Motorway (Major Highways) from the OSM file.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Feature Key", "K", "Key to identify building's identified in the OSM file.", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Data storage
            DataTree<Point3d> wayPoints = new DataTree<Point3d>();
            DataTree<string> wayKeys = new DataTree<string>();

            // input data
            List<OSMPointData> osmData = new List<OSMPointData>();
            string xmlString = string.Empty;

            DA.GetDataList(0, osmData);
            DA.GetData(1, ref xmlString);


            if (xmlString != string.Empty && osmData.Count >= 1)
            {
                // Convert the OSMPointData to a standard OSMPoint
                List<OSMPoint> osmPoints = new List<OSMPoint>();
                foreach (OSMPointData opd in osmData)
                {
                    osmPoints.Add(opd.Value);
                }


                List<OSMWay> motorWays = ElkLib.GatherWays("highway", new string[] { "motorway" }, xmlString, osmPoints);
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{082f23f2-8ba1-444c-bb4c-7b6e402543ed}"); }
        }
    }
}