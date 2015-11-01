//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.Windows.Controls;

//using DSCore;
//using Dynamo.Controls;
//using Dynamo.UI;
//using DSCoreNodesUI;
//using Dynamo.Core.Threading;
//using Dynamo.Models;
//using Dynamo.ViewModels;
//using ProtoCore.Mirror;
//using Dynamo.Wpf;
//using System.Windows.Input;


//namespace Elk.DynamoComponents
//{
//    [NodeName("OSMData")]
//    [NodeCategory("Elk.OSM")]
//    [NodeDescription("Get Point and Tag data from an OSM file.")]
//    public class OSMDataNode : NodeModel
//    {
//        bool show3d = false;
//        bool individualOutputs = false;

//        string way = "building";
//        Elk.Common.ElkLib.FeatureType wayFT = Common.ElkLib.FeatureType.Building;
        
//        List<string> selectedTypes = new List<string>();
//        List<string> featureTypes = (Elk.Common.ElkLib.featureKeys).ToList();

//        public bool Show3d 
//        {
//            get { return show3d; }
//            set { show3d = value; }
        
//        }

//        public bool IndividualOutputs
//        {
//            get { return individualOutputs; }
//            set { individualOutputs = value; }
//        }

//        public string Way 
//        {
//            get { return way; }
//            set { way = value; } 
//        }

//        public Elk.Common.ElkLib.FeatureType WayFT
//        {
//            get { return wayFT; }
//            set { wayFT = value; }
//        }

//        public List<string> SelectedTypes
//        {
//            get { return selectedTypes; }
//            set { selectedTypes = value; }
//        }

//        public List<string> FeatureTypes
//        {
//            get { return featureTypes; }
//            set { featureTypes = value; }
//        }

//        public OSMDataNode()
//        {
//            InitializePorts();
//        }

//        protected virtual void InitializePorts()
//        {
//            // Inputs
//            InPortData.Add(new PortData("OSMpts", "Holds preprocessed point and identification data from the Location node"));
//            InPortData.Add(new PortData("XML", "XML Data pulled directly from the OSM file.  Acquired from the Location node"));

//            // Outputs
//            if (individualOutputs && selectedTypes.Count > 0)
//            {

//            }
//            else
//            {
//                OutPortData.Add(new PortData("Ways", "Point collections that represent Ways, Multi-Polygons, and Nodes from OSM."));
//                OutPortData.Add(new PortData("Tags", "Tagged attributes that represent each related object in the OSM file."));
//            }
//            RegisterAllPorts();
//        }

        
//        public void AdjustParams()
//        {
//            InitializePorts();
//        }
//    }

//    internal class OSMDataViewCustomization : INodeViewCustomization<OSMDataNode>
//    {
//        private DynamoViewModel viewModel;
//        private OSMDataNode model;

//        public void CustomizeView(OSMDataNode nodeModel, NodeView nodeView)
//        {
//            //base.CustomizeView(nodeModel, nodeView);

//            model = nodeModel;
//            viewModel = nodeView.ViewModel.DynamoViewModel;

//            var subTypesItem = new MenuItem { Header = "Select Feature Sub-Types", IsCheckable = false };
//            nodeView.MainContextMenu.Items.Add(subTypesItem);
//            subTypesItem.Click += delegate { SelectSubTypes(); };
//            nodeView.UpdateLayout();

//            // Double Click capture
//            nodeView.MouseDown += view_MouseDown;
//        }

//        private void view_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            // If Double-Clicked...
//            if (e.ClickCount >= 2)
//            {
//                SelectSubTypes();
//                e.Handled = true;
//                System.Windows.Forms.MessageBox.Show("Testing Double-click");
//            }
//        }

//        private void SelectSubTypes()
//        {
//            // Open the selection form
//            Elk.Common.TypeSelectForm form = new Elk.Common.TypeSelectForm(model.WayFT, model.SelectedTypes, model.IndividualOutputs, false);
//            form.ShowDialog();

//            if (form.DialogResult.HasValue && form.DialogResult.Value)
//            {
//                // Get the selected types
//                System.Collections.ObjectModel.ObservableCollection<string> tempTypes = form.SelectedTypes;
//                bool tempOutputs = form.IndividualOutputs;
//                bool expire = false;
//                if (model.SelectedTypes.Count == tempTypes.Count)
//                {
//                    foreach (string s in tempTypes)
//                    {
//                        if (!model.SelectedTypes.Contains(s))
//                        {
//                            expire = true;
//                            break;
//                        }
//                    }
//                }
//                else
//                    expire = true;

//                if (tempOutputs !=  model.IndividualOutputs)
//                {
//                    model.IndividualOutputs = tempOutputs;
//                    expire = true;
//                }

//                if (expire)
//                {
//                    model.SelectedTypes.Clear();
//                    foreach (string s in tempTypes)
//                    {
//                        model.SelectedTypes.Add(s);
//                    }
//                    model.AdjustParams();
//                }
//            }   
//        }

//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
