using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;

using Elk.Common;
using Grasshopper.Kernel.Data;

namespace Elk.GHComponents
{
    public class OSMComponent : GH_Component
    {
        bool show3d = false;
        bool individualOutputs = false;

        string way = "building";
        Elk.Common.ElkLib.FeatureType wayFT = ElkLib.FeatureType.Building;
        System.Windows.Forms.ToolStripComboBox tsComboBox;
        System.Windows.Forms.ToolStripMenuItem show3dMenuItem = new System.Windows.Forms.ToolStripMenuItem();

        List<string> selectedTypes = new List<string>();
        List<string> featureTypes = (Elk.Common.ElkLib.featureKeys).ToList();

        /// <summary>
        /// Initializes a new instance of the BuildingComponent class.
        /// </summary>
        public OSMComponent()
            : base("OSM Data", "OSM",
                "Get Point and Tag data from an OSM file.",
                Properties.Settings.Default.tabName, Properties.Settings.Default.panelName)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new OSMPointParam(), "OSM Point Data", "O", "Holds preprocessed point and identification data from the OSM file.", GH_ParamAccess.list);
            pManager.AddTextParameter("OSM File Path", "F", "Path to the OSM file.", GH_ParamAccess.item, string.Empty);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            if (selectedTypes.Count != 0)
            {
                AdjustParams();
            }
            else
            {
                pManager.AddPointParameter("Ways", "W", "Point collections that represent Ways, Multi-Polygon's, and Nodes from OSM.", GH_ParamAccess.tree);
                pManager.AddGenericParameter("Feature Key", "K", "Key to identify ways from the OSM file.", GH_ParamAccess.tree);
                //pManager.AddTextParameter("Feature Key", "K", "Key to identify ways identified in the OSM file.", GH_ParamAccess.tree);
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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

                List<OSMWay> collectedWays = ElkLib.GatherWays(way, selectedTypes.ToArray(), xmlString, osmPoints);

                if (!individualOutputs)
                {
                    // Data storage
                    DataTree<Point3d> wayPoints = new DataTree<Point3d>();
                    DataTree<string> wayKeys = new DataTree<string>();
                    
                    try
                    {
                        // convert the OSMWays to Rhino Point3d and string objects.
                        for (int i = 0; i < collectedWays.Count; i++)
                        {
                            OSMWay currentWay = collectedWays[i];
                            // Get the points
                            for (int j = 0; j < currentWay.Points.Count; j++)
                            {
                                try
                                {
                                    List<LINE.Geometry.Point3d> pointList = currentWay.Points[j];
                                    foreach (LINE.Geometry.Point3d point in pointList)
                                    {
                                        try
                                        {
                                            wayPoints.Add(new Point3d(point.X, point.Y, point.Z), new Grasshopper.Kernel.Data.GH_Path(new int[] { i, j }));
                                        }
                                        catch
                                        {
                                            wayPoints.Add(Point3d.Origin, new Grasshopper.Kernel.Data.GH_Path(new int[] { i, j }));
                                        }
                                    }
                                }
                                catch
                                {
                                    wayPoints.Add(Point3d.Origin, new Grasshopper.Kernel.Data.GH_Path(new int[] { i, j }));
                                }
                            }
                            //System.Windows.Forms.MessageBox.Show("Before Tags");
                            // Get the Tag Data
                            foreach (KeyValuePair<string, string> tag in currentWay.Tags)
                            {
                                try
                                {
                                    string currentTag = tag.Key + ":" + tag.Value;
                                    wayKeys.Add(currentTag, new Grasshopper.Kernel.Data.GH_Path(new int[] { i, 0 }));
                                }
                                catch
                                {
                                    wayKeys.Add("ERROR", new Grasshopper.Kernel.Data.GH_Path(new int[] { i, 0 }));
                                }
                            }
                        }

                    }
                    catch { }

                    // Just output to two default params.
                    DA.SetDataTree(0, wayPoints);
                    if (show3d && show3dMenuItem.Enabled)
                    {
                        // Generate the 3d buildings
                        DataTree<Brep> buildings = Generate3DBuildings(collectedWays);

                        DA.SetDataTree(1, buildings);
                        DA.SetDataTree(2, wayKeys);
                    }
                    else
                    {
                        DA.SetDataTree(1, wayKeys);
                    }
                }
                else
                {
                    // Organize the ways into their own data trees, leave the key as a single tree, just bump up the path.
                    DataTree<string> wayKeys = new DataTree<string>();
                    
                    for (int h = 0; h < selectedTypes.Count; h++)
                    {
                        // Data storage
                        DataTree<Point3d> wayPoints = new DataTree<Point3d>();
                        try
                        {
                            // convert the OSMWays to Rhino Point3d and string objects.
                            int foundWays = 0;
                            foreach(OSMWay currentWay in collectedWays)
                            {
                                if (currentWay.Type == selectedTypes[h])
                                {
                                    // Get the points
                                    for (int j = 0; j < currentWay.Points.Count; j++)
                                    {
                                        try
                                        {
                                            List<LINE.Geometry.Point3d> pointList = currentWay.Points[j];
                                            foreach (LINE.Geometry.Point3d point in pointList)
                                            {
                                                try
                                                {
                                                    wayPoints.Add(new Point3d(point.X, point.Y, point.Z), new Grasshopper.Kernel.Data.GH_Path(new int[] { foundWays, j }));
                                                }
                                                catch
                                                {
                                                    wayPoints.Add(Point3d.Origin, new Grasshopper.Kernel.Data.GH_Path(new int[] { foundWays, j }));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            wayPoints.Add(Point3d.Origin, new Grasshopper.Kernel.Data.GH_Path(new int[] { foundWays, j }));
                                        }
                                    }

                                    // Get the Tag Data
                                    foreach (KeyValuePair<string, string> tag in currentWay.Tags)
                                    {
                                        try
                                        {
                                            string currentTag = tag.Key + ":" + tag.Value;
                                            wayKeys.Add(currentTag, new Grasshopper.Kernel.Data.GH_Path(new int[] { h, foundWays, 0 }));
                                        }
                                        catch
                                        {
                                            wayKeys.Add("ERROR", new Grasshopper.Kernel.Data.GH_Path(new int[] { h, foundWays, 0 }));
                                        }
                                    }
                                    foundWays++;
                                }
                            }
                            // Assign the data to outputs
                            DA.SetDataTree(h, wayPoints);
                        }
                        catch { }

                        // Assign the Keys and possibly buildings to the output
                        if (show3d && show3dMenuItem.Enabled)
                        {
                            // Generate the 3d building breps
                            DataTree<Brep> buildings = Generate3DBuildings(collectedWays);

                            DA.SetDataTree(Params.Output.Count - 2, buildings);
                            DA.SetDataTree(Params.Output.Count - 1, wayKeys);
                        }
                        else
                        {
                            DA.SetDataTree(Params.Output.Count - 1, wayKeys);
                        }
                    }
                    
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
                return Properties.Resources.OSM_24X24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{8353bc0a-ba2e-4cd8-9f3b-067c501f49b9}"); }
        }

        public override void AppendAdditionalMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Component UI Location", UILocation_Clicked);
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Select Feature Sub-Types", SelectTypes_Clicked);
            
            show3dMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            show3dMenuItem.Text = "Create 3D Buildings";
            show3dMenuItem.Click += new EventHandler(Generate3D_Clicked);
            show3dMenuItem.Checked = show3d;
            if (wayFT == ElkLib.FeatureType.Building)
                show3dMenuItem.Enabled = true;
            else
                show3dMenuItem.Enabled = false;
            menu.Items.Add(show3dMenuItem);
            
            Menu_AppendSeparator(menu);
            // Add Feature Type ComboBox
            Menu_AppendItem(menu, "Feature Type:", FeatureType_Clicked);
            tsComboBox = new System.Windows.Forms.ToolStripComboBox();
            tsComboBox.ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            tsComboBox.ComboBox.BindingContext = new System.Windows.Forms.BindingContext();
            tsComboBox.ComboBox.Width = 230;
            tsComboBox.ComboBox.DataSource = featureTypes;
            tsComboBox.ComboBox.SelectedIndexChanged += new EventHandler(Feature_SelectedIndexChanged);
            tsComboBox.ComboBox.SelectedIndex = featureTypes.IndexOf(way);
            menu.Items.Add(tsComboBox);
        }

        private void UILocation_Clicked(object sender, EventArgs e)
        {
            ComponentLocationForm form = new ComponentLocationForm();
            form.ShowDialog();
        }

        public void FeatureType_Clicked(object sender, EventArgs e) { }

        private void Feature_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tempWay = tsComboBox.SelectedItem as string;
            if (tempWay != way)
            {
                way = tsComboBox.SelectedItem as string;
                string name = string.Empty;
                System.Globalization.TextInfo ti = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                if (tempWay.Contains("_"))
                {
                    string[] split = tempWay.Split(new char[] { '_' });
                    name = ti.ToTitleCase(split[0]);
                    name += ti.ToTitleCase(split[1]);
                }
                else
                {
                    name = ti.ToTitleCase(way);
                }
                selectedTypes.Clear();
                Enum.TryParse(name, out wayFT);
                if (wayFT != ElkLib.FeatureType.Building)
                    show3dMenuItem.Enabled = false;
                else
                    show3dMenuItem.Enabled = true;

                ExpireSolution(true);
            }
        }

        public void SelectTypes_Clicked(object sender, EventArgs e)
        {
            // Open the selection form
            Elk.Common.TypeSelectForm form = new TypeSelectForm(wayFT, selectedTypes, individualOutputs, true);
            form.ShowDialog();

            if (form.DialogResult.HasValue && form.DialogResult.Value)
            {
                // Get the selected types
                System.Collections.ObjectModel.ObservableCollection<string> tempTypes = form.SelectedTypes;
                bool tempOutputs = form.IndividualOutputs;
                bool expire = false;
                if (selectedTypes.Count == tempTypes.Count)
                {
                    foreach (string s in tempTypes)
                    {
                        if (!selectedTypes.Contains(s))
                        {
                            expire = true;
                            break;
                        }
                    }
                }
                else
                    expire = true;

                if (tempOutputs != individualOutputs)
                {
                    individualOutputs = tempOutputs;
                    expire = true;
                }

                if (expire)
                {
                    selectedTypes.Clear();
                    foreach (string s in tempTypes)
                    {
                        selectedTypes.Add(s);
                    }
                    AdjustParams();
                }
            }
        }

        public void Generate3D_Clicked(object sender, EventArgs e)
        {
            if (show3d)
                show3d = false;
            else
                show3d = true;
            AdjustParams();
            //ExpireSolution(true);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            try
            {
                show3d = reader.GetBoolean("Generate3d");
                individualOutputs = reader.GetBoolean("IndividualOutputs");
                way = reader.GetString("Feature");
                System.Globalization.TextInfo ti = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                wayFT = ElkLib.FeatureType.Building;
                bool tryEnum = Enum.TryParse(ti.ToTitleCase(way), out wayFT);
                
                if (wayFT == ElkLib.FeatureType.Building)
                    show3dMenuItem.Enabled = true;
                else
                {
                    show3dMenuItem.Enabled = false;
                    show3d = false;
                }
                
                selectedTypes = new List<string>();
                bool keepGoing = true;
                int i = 0;
                while (keepGoing)
                {
                    try
                    {
                        string selection = reader.GetString("SelectedTypes", i);
                        if (selection != null && selection != string.Empty)
                            selectedTypes.Add(selection);
                        else
                            keepGoing = false;
                    }
                    catch
                    {
                        keepGoing = false;
                    }
                    i++;
                }
                
                AdjustParams();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ReadError:\n" + ex.ToString());
            }
            return base.Read(reader);
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetBoolean("Generate3d", show3d);
            writer.SetBoolean("IndividualOutputs", individualOutputs);
            writer.SetString("Feature", way);
            for (int i = 0; i < selectedTypes.Count; i++)
            {
                writer.SetString("SelectedTypes", i, selectedTypes[i]);
            }
            return base.Write(writer);
        }

        private void AdjustParams()
        {
            
            #region Invidual Outputs
            if (individualOutputs)
            {
                int nonWayParams = 1;

                if (show3dMenuItem.Enabled && show3d)
                    nonWayParams = 2;

                if (Params.Output.Count < selectedTypes.Count + nonWayParams)
                {
                    // Add parameters
                    for (int i = Params.Output.Count - 1; i <= selectedTypes.Count - nonWayParams; i++)
                    {
                        try
                        {
                            IGH_Param param = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                            Params.RegisterOutputParam(param);
                        }
                        catch { }
                    }
                }
                else if (Params.Output.Count > selectedTypes.Count + nonWayParams)
                {
                    // Remove parameters
                    for (int i = Params.Output.Count - 1; i >= selectedTypes.Count + nonWayParams; i--)
                    {
                        try
                        {
                            Params.UnregisterOutputParameter(Params.Output[i]);
                        }
                        catch { }
                    }
                }

                // reset the parameters based on the selected types
                for (int i = 0; i < Params.Output.Count - nonWayParams; i++)
                {
                    try
                    {
                        Params.Output[i].Name = selectedTypes[i];
                        Params.Output[i].NickName = selectedTypes[i];
                        Params.Output[i].Access = GH_ParamAccess.tree;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Error\n" + ex.Message);
                    }
                }

                if (show3d && show3dMenuItem.Enabled)
                {
                    
                    Params.Output[Params.Output.Count - 1].Name = "Buildings";
                    Params.Output[Params.Output.Count - 1].NickName = "Bldg";
                    Params.Output[Params.Output.Count - 1].Description = "3d massing of buildings from the OSM data.";
                    Params.Output[Params.Output.Count - 1].Access = GH_ParamAccess.tree;
                }

                // Set the last output to be the way's Key
                Params.Output[Params.Output.Count - nonWayParams].Name = "Feature Key";
                Params.Output[Params.Output.Count - nonWayParams].NickName = "K";
                Params.Output[Params.Output.Count - nonWayParams].Description = "Key to identify ways from the OSM file.";
                Params.Output[Params.Output.Count - nonWayParams].Access = GH_ParamAccess.tree;
            }
            #endregion
            #region Single Outputs
            else
            {
                int paramCount = 2;
                if (show3d && show3dMenuItem.Enabled)
                    paramCount = 3;
                if (Params.Output.Count < paramCount)
                {
                    // Add parameters
                    for (int i = Params.Output.Count - 1; i < paramCount - 1; i++)
                    {
                        try
                        {
                            IGH_Param param = new Grasshopper.Kernel.Parameters.Param_GenericObject();
                            Params.RegisterOutputParam(param);
                        }
                        catch { }
                    }
                }
                else 
                if (Params.Output.Count > paramCount)
                {
                    // Remove parameters
                    for (int i = Params.Output.Count - 1; i >= paramCount; i--)
                    {
                        try
                        {
                            Params.UnregisterOutputParameter(Params.Output[i]);
                        }
                        catch { }
                    }
                }
                // Set the first output to be the ways
                Params.Output[0].Name = "Ways";
                Params.Output[0].NickName = "W";
                Params.Output[0].Description = "Point collections that represent Ways, Multi-Polygons, and Nodes from OSM.";
                Params.Output[0].Access = GH_ParamAccess.tree;
                int wayIndex = 1;
                if (show3d && show3dMenuItem.Enabled)
                {
                    wayIndex = 2;
                    Params.Output[1].Name = "Buildings";
                    Params.Output[1].NickName = "Bldg";
                    Params.Output[1].Description = "3d massing of buildings from the OSM data.";
                    Params.Output[1].Access = GH_ParamAccess.tree;
                }
                // Set the last output to be the way's Key
                Params.Output[wayIndex].Name = "Feature Key";
                Params.Output[wayIndex].NickName = "K";
                Params.Output[wayIndex].Description = "Key to identify ways from the OSM file.";
                Params.Output[wayIndex].Access = GH_ParamAccess.tree;
            }
            #endregion


            Params.OnParametersChanged();
            ExpireSolution(true);
        }

        public DataTree<Brep> Generate3DBuildings(List<OSMWay> ways)
        {
            double unitScale = CommonGHProcessors.GetRhinoUnitScale(Rhino.RhinoDoc.ActiveDoc);
            DataTree<Brep> breps = new DataTree<Brep>();

            for (int i = 0; i < ways.Count; i++)
            {
                OSMWay currentWay = ways[i];

                double height = 0;

                // Check if there is a building heigh identified
                foreach (KeyValuePair<string, string> tag in currentWay.Tags)
                {
                    if (tag.Key.ToLower() == "height")
                    {
                        try
                        {
                            string heightStr = tag.Value.ToLower();
                            if (heightStr.Contains("m"))
                            {
                                height = Convert.ToDouble(heightStr.Replace("m", string.Empty)) * unitScale;
                            }
                            else if (heightStr.Contains("\'") || heightStr.Contains("\""))
                            {
                                string[] heightArr = heightStr.Split(new char[] { '\'' });
                                if (heightArr.Count() == 1)
                                {
                                    height = Convert.ToDouble(heightArr[0]);
                                }
                                else
                                {
                                    double ft = Convert.ToDouble(heightArr[0]);
                                    double inch = Convert.ToDouble(heightArr[1].Replace("\"", string.Empty));
                                    ft += (inch / 12);

                                    double metric = ft * 0.30479999024640031211519001231392;

                                    height = metric * unitScale;
                                }
                            }
                            else
                            {
                                height = Convert.ToDouble(heightStr) * unitScale;
                            }
                        }
                        catch
                        { }
                        break;
                    }
                }

                Vector3d heightVect = new Vector3d(0, 0, height);

                // Create Polylines if possible
                List<Polyline> boundaryCrvs = new List<Polyline>();
                for (int j = 0; j < currentWay.Points.Count; j++)
                {
                    List<LINE.Geometry.Point3d> pointList = currentWay.Points[j];

                    List<Point3d> polyLinePoints = new List<Point3d>();
                    foreach (LINE.Geometry.Point3d point in pointList)
                    {
                        try
                        {
                            Point3d pt = new Point3d(point.X, point.Y, point.Z);
                            polyLinePoints.Add(pt);
                        }
                        catch { }
                    }
                    if (polyLinePoints.Count > 2)
                    {
                        Polyline pline = new Polyline(polyLinePoints);
                        if (!pline.IsClosed)
                        {
                            pline.Add(pline.First);
                        }
                        //Curve crv = pline.ToNurbsCurve() as Curve;
                        //crv.MakeClosed(0.001);
                        boundaryCrvs.Add(pline);
                    }
                }

                
                // Create the extrued masses as breps
                if (boundaryCrvs.Count == 1)
                {
                    try
                    {
                        Line[] ca = boundaryCrvs[0].GetSegments();
                        List<Curve> crvSegments = new List<Curve>();
                        foreach (Line line in ca)
                        {
                            crvSegments.Add(line.ToNurbsCurve());
                        }

                        List<Brep> unjoinedBreps = new List<Brep>();
                        Curve[] pc = PolyCurve.JoinCurves(crvSegments);
                        Brep[] brepArr = Brep.CreatePlanarBreps(pc);
                        unjoinedBreps.Add(brepArr[0]);

                        // Add the extruded surfaces
                        foreach (Curve crvSeg in crvSegments)
                        {
                            Surface srf = Surface.CreateExtrusion(crvSeg, heightVect);
                            if (srf != null)
                                unjoinedBreps.Add(srf.ToBrep());
                        }
                        //Surface srf2 = Surface.CreateExtrusion(pc[0], heightVect);
                        //Brep brep = srf2.ToBrep();
                        //unjoinedBreps.Add(brep);
                        Brep top = brepArr[0].DuplicateBrep();
                        top.Translate(heightVect);
                        unjoinedBreps.Add(top);
                        Brep[] joinedBreps = Brep.JoinBreps(unjoinedBreps, 0.001);

                        breps.Add(joinedBreps[0], new GH_Path(i, 0));
                        //breps.Add(brep.CapPlanarHoles(0.01), new GH_Path(i, 0));
                    }
                    catch
                    {
                        breps.Add(null, new GH_Path(i, 0));
                    }
                }
                else if (boundaryCrvs.Count > 1)
                {
                    try
                    {
                        
                        List<Curve> crvSegments = new List<Curve>();
                        List<Curve> polyCurves = new List<Curve>();
                        foreach (Polyline pl in boundaryCrvs)
                        {
                            Line[] ca = pl.GetSegments();
                            List<Curve> tempCrvs = new List<Curve>();
                            foreach (Line line in ca)
                            {
                                tempCrvs.Add(line.ToNurbsCurve());
                                crvSegments.Add(line.ToNurbsCurve());
                            }
                            polyCurves.Add(PolyCurve.JoinCurves(tempCrvs)[0]);
                        }
                        List<Brep> brepSegments = new List<Brep>();
                        // Create an initial brep from lofting
                        
                        Brep[] brepArr = Brep.CreatePlanarBreps(polyCurves);
                        Brep baseBrep = brepArr[0];
                        brepSegments.Add(baseBrep);
                        

                        // Add the extruded surfaces
                        foreach (Curve crvSeg in crvSegments)
                        {
                            Surface srf = Surface.CreateExtrusion(crvSeg, heightVect);
                            if (srf != null)
                                brepSegments.Add(srf.ToBrep());
                        }

                        // move the base surface up
                        Brep topBrep = baseBrep.DuplicateBrep();
                        topBrep.Translate(heightVect);
                        brepSegments.Add(topBrep);
                        //topBrep.Flip();
                        Brep[] joinedBreps = Brep.JoinBreps(brepSegments, 0.0001);
                        //BrepSolidOrientation solidOrientation = joinedBreps[0].get_SolidOrientation();
                        breps.Add(joinedBreps[0], new GH_Path(i, 0));
                    }
                    catch
                    {
                        breps.Add(null, new GH_Path(i, 0));
                    }
                }
                else
                {
                    // single point data
                    breps.Add(null, new GH_Path(i, 0));
                }
                //System.Windows.Forms.MessageBox.Show("Current 3d i: " + i.ToString() + "\nBranches: " + breps.BranchCount.ToString());
            }
            return breps;
        }
    }
}