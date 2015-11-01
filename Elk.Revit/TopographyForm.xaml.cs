using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Elk.Common;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;

namespace Elk.Revit
{
    /// <summary>
    /// Interaction logic for TopographyForm.xaml
    /// </summary>
    public partial class TopographyForm : Window
    {
        LinearGradientBrush brush = null;

        string latMinStr;
        string latMaxStr;
        string lonMinStr;
        string lonMaxStr;

        double latMin;
        double latMax;
        double lonMin;
        double lonMax;

        bool format = true;
        int formatSize;
        double unitScale;

        ExternalCommandData commandData;
        Document doc;
        List<XYZ> surfPts = new List<XYZ>();

        public TopographyForm(ExternalCommandData cdata)
        {
            commandData = cdata;
            doc = commandData.Application.ActiveUIDocument.Document;

            InitializeComponent();

            // Get the Units
            Units docUnits = doc.GetUnits();
            FormatOptions fo = docUnits.GetFormatOptions(UnitType.UT_Length);
            DisplayUnitType dut = fo.DisplayUnits;
            switch (dut)
            {
                case Autodesk.Revit.DB.DisplayUnitType.DUT_CENTIMETERS:
                    unitScale = 100;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMAL_FEET:
                    unitScale = 3.28084;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMAL_INCHES:
                    unitScale = 39.3701;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMETERS:
                    unitScale = 10;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES:
                    unitScale = 3.28084;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_FRACTIONAL_INCHES:
                    unitScale = 39.3701;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_MILLIMETERS:
                    unitScale = 1000;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_METERS:
                    unitScale = 1.0;
                    break;
                case Autodesk.Revit.DB.DisplayUnitType.DUT_METERS_CENTIMETERS:
                    unitScale = 1.0;
                    break;
                default:
                    unitScale = 1.0;
                    break;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (brush == null)
            {
                brush = EnterBrush();
            }
            closeButtonRect.Fill = brush;
        }

        private void closeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            closeButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the lat and long domain
            double minLat = Convert.ToDouble(minLatTextBox.Text);
            double maxLat = Convert.ToDouble(maxLatTextBox.Text);
            double minLon = Convert.ToDouble(minLonTextBox.Text);
            double maxLon = Convert.ToDouble(maxLonTextBox.Text);

            LINE.Geometry.Interval2d topoDomain = new LINE.Geometry.Interval2d(minLon, maxLon, minLat, maxLat);

            // output parameters
            List<List<XYZ>> topoPoints = null;
            
            string filePath = fileTextBox.Text;


            if (filePath != null && System.IO.File.Exists(filePath))
            {
                List<List<LINE.Geometry.Point3d>> pts = ElkLib.ProcessTopoFile(filePath, unitScale, topoDomain);

                List<XYZ> points = new List<XYZ>();
                for (int i = 0; i < pts.Count; i++)
                {
                    List<LINE.Geometry.Point3d> rowPoints = pts[i];

                    for (int j = 0; j < rowPoints.Count; j++)
                    {
                        XYZ revPoint = new XYZ(rowPoints[j].X, rowPoints[j].Y, rowPoints[j].Z);
                        points.Add(revPoint);
                    }
                }
                using (Transaction trans = new Transaction(doc, "Elk Create Topography"))
                {
                    trans.Start();

                    if (points.Count > 2)
                    {
                        TopographySurface topo = TopographySurface.Create(doc, points);
                    }
                    trans.Commit();
                }
            }
            Close();
        }

        private void okButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (brush == null)
            {
                brush = EnterBrush();
            }
            okButtonRect.Fill = brush;
        }

        private void okButton_MouseLeave(object sender, MouseEventArgs e)
        {
            okButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog())
            {
                dlg.Title = "Select an Elevation file...";
                dlg.Filter = "Elevation Data File (*.hgt, *.tif, *.img)|*.hgt; *.tif; *.img|All Files (*.*)|*.*";
                dlg.RestoreDirectory = true;

                System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.Cancel)
                {
                    fileTextBox.Text = dlg.FileName;
                }
            }
        }

        private void browseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (brush == null)
            {
                brush = EnterBrush();
            }
            browseButtonRect.Fill = brush;
        }

        private void browseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            browseButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void minLatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            latMinStr = minLatTextBox.Text;
        }

        private void maxLatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            latMaxStr = maxLatTextBox.Text;
        }

        private void minLonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lonMinStr = minLonTextBox.Text;
        }

        private void maxLonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lonMaxStr = maxLonTextBox.Text;
        }

        private LinearGradientBrush EnterBrush()
        {
            LinearGradientBrush b = new LinearGradientBrush();
            b.StartPoint = new System.Windows.Point(0, 0);
            b.EndPoint = new System.Windows.Point(0, 1);
            b.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(255, 195, 195, 195), 0.0));
            b.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(255, 245, 245, 245), 1.0));

            return b;
        }
    }
}
