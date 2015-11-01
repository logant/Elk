using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Elk.Common;

namespace Elk.GHComponents
{
    public class LocationComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public LocationComponent()
            : base("Location", "Loc",
                "OpenStreetMap and Topograhy",
                Properties.Settings.Default.tabName, Properties.Settings.Default.panelName)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path (text)", "File", "File path for the OSM file.", GH_ParamAccess.item, string.Empty);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new OSMPointParam(), "OSM Point Data", "OSM", "Holds point and identification OSM data.");
            pManager.Register_StringParam("OSMFile", "File", "Path to the OSM file.");
            pManager.Register_IntervalParam("Longitude Domain", "Lon", "MIN and MAX longitude domain found in the OSM file.");
            pManager.Register_IntervalParam("Latitude Domain", "Lat", "MIN and MAX latitude domain found in the OSM file.");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get the Rhino Units to properly scale the lat/lon data into Rhino units
            double unitScale = CommonGHProcessors.GetRhinoUnitScale(Rhino.RhinoDoc.ActiveDoc);

            List<OSMPointData> outPoints = new List<OSMPointData>();
            List<OSMPoint> osmPoints = new List<OSMPoint>();
            LINE.Geometry.Interval2d latlon = new LINE.Geometry.Interval2d();
            string dataStream = string.Empty;
            Interval latDomain = new Interval();
            Interval lonDomain = new Interval();

            string path = null;

            DA.GetData(0, ref path);
            if (path != null && System.IO.File.Exists(path))
            {
                ElkLib.NodePreProcess(path, unitScale, out dataStream, out osmPoints, out latlon);
                foreach (OSMPoint op in osmPoints)
                {
                    OSMPointData od = new OSMPointData(op);
                    outPoints.Add(od);
                }
            }

            lonDomain = new Interval(latlon.UMin, latlon.UMax);
            latDomain = new Interval(latlon.VMin, latlon.VMax);
            
            // Output the data
            DA.SetDataList(0, outPoints);
            DA.SetData(1, path);
            DA.SetData(2, lonDomain);
            DA.SetData(3, latDomain);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Elk_24x24;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{360cdbb3-1b2b-46aa-aae6-b0b6dcb9aae8}"); }
        }

         public override void AppendAdditionalMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Component UI Location", UILocation_Clicked);
        }

         private void UILocation_Clicked(object sender, EventArgs e)
         {
             ComponentLocationForm form = new ComponentLocationForm();
             form.ShowDialog();
         }
    }
}
