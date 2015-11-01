using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace Elk.GHComponents
{
    public class TopographyComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TopographyComponent class.
        /// </summary>
        public TopographyComponent()
            : base("Topography", "Topo",
                "Generate Topography surfaces from USGS IMG Raster files or from SRTM HGT files.",
                Properties.Settings.Default.tabName, Properties.Settings.Default.panelName)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "File", "Path to an HGT, GeoTiff, or IMG file", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Longitude", "Lon", "Longitiude domain.", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Latitude", "Lat", "Latitiude domain.", GH_ParamAccess.item);
            

            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("File Info", "Info", "File info related to bounding box domain", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "Pts", "Points, branched along longitude", GH_ParamAccess.tree);
            pManager.AddCurveParameter("Curves", "Crvs", "Curves representing the longitudinal lines", GH_ParamAccess.list);
            pManager.AddSurfaceParameter("Surface", "Srf", "Topo surface genreated from the curves.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = null;
            Interval latDomain = new Interval(0, 0);
            Interval lonDomain = new Interval(0, 0);

            DA.GetData(0, ref filePath);
            DA.GetData(1, ref lonDomain);
            DA.GetData(2, ref latDomain);

            // Get the Rhino Units to properly scale the lat/lon data into Rhino units
            double unitScale = CommonGHProcessors.GetRhinoUnitScale(Rhino.RhinoDoc.ActiveDoc);
            
            if (filePath != null && System.IO.File.Exists(filePath))
            {
                // Get the file domain
                List<string> fileInfo = Elk.Common.ElkLib.TopoFileInfo(filePath);
                
                DA.SetDataList(0, fileInfo);

                if (latDomain.Length > 0 && lonDomain.Length > 0)
                {
                    // Retrieve the topography point data
                    List<List<LINE.Geometry.Point3d>> pts = Elk.Common.ElkLib.ProcessTopoFile(filePath, unitScale, new LINE.Geometry.Interval2d(lonDomain.Min, lonDomain.Max, latDomain.Min, latDomain.Max));
                    DataTree<Point3d> topoPoints = new DataTree<Point3d>();
                    List<Point3d> flattenedPoints = new List<Point3d>();
                    List<Curve> curves = new List<Curve>();

                    // Convert the LINE.Geometry.Point3d to a Rhino.Geometry.Point3d and generate the Rhino Curves and Surface.
                    for (int i = 0; i < pts.Count; i++)
                    {
                        try
                        {
                            List<LINE.Geometry.Point3d> rowPoints = pts[i];
                            List<Point3d> crvPts = new List<Point3d>();
                            for (int j = 0; j < rowPoints.Count; j++)
                            {
                                try
                                {
                                    Point3d pt = new Point3d(rowPoints[j].X, rowPoints[j].Y, rowPoints[j].Z);
                                    crvPts.Add(pt);
                                    flattenedPoints.Add(pt);
                                    topoPoints.Add(pt, new GH_Path(i));
                                }
                                catch { }
                            }
                            curves.Add(Curve.CreateInterpolatedCurve(crvPts, 3));
                        }
                        catch { }
                    }

                    NurbsSurface surf = NurbsSurface.CreateFromPoints(flattenedPoints, topoPoints.BranchCount, topoPoints.Branches[0].Count, 3, 3);
                    DA.SetDataTree(1, topoPoints);
                    DA.SetDataList(2, curves);
                    DA.SetData(3, surf);
                }
            }

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Topo_24X24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7969ecfe-aeca-41d3-b03d-d4d79a3db6a5}"); }
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