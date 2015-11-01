using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using adWin = Autodesk.Windows;
using System.Windows.Media.Imaging;

namespace Elk.Revit
{
    [Transaction(TransactionMode.Manual)]
    class TopographyApp : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Path to the applicaiton
                string path = typeof(TopographyApp).Assembly.Location;

                List<RibbonPanel> panels = application.GetRibbonPanels();
                RibbonPanel panel = null;
                foreach (RibbonPanel rp in panels)
                {
                    if (rp.Name == "Elk DT")
                    {
                        panel = rp;
                        break;
                    }
                }
                if (panel == null)
                {
                    panel = application.CreateRibbonPanel("Elk DT");
                }

                BitmapSource bms = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Topo.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                PushButtonData topoButtonData = new PushButtonData(
                    "Elk Topo", "Elk\nTopo", path, "Elk.Revit.TopographyCmd")
                {
                    LargeImage = bms,
                    ToolTip = "Generate a topo using USGS data.",
                };
                
                panel.AddItem(topoButtonData);

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }
        }
    }
}
