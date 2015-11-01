using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elk
{
    internal static class DynamoCommon
    {
        internal static double GetRevitUnitScale()
        {
            // Default units is assumed to be meters unless 
            double scale = 1.0;

            // Check the assemblies for the RevitAPI library
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (assemblies.Any(x => x.FullName.Contains("RevitServices")))
            {
                // Get the RevitAPI assembly
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly.GetName().Name == "RevitServices")
                    {
                        Autodesk.Revit.DB.Document doc = RevitServices.Persistence.DocumentManager.Instance.CurrentDBDocument;
                        Autodesk.Revit.DB.Units units = doc.GetUnits();
                        Autodesk.Revit.DB.FormatOptions fo = units.GetFormatOptions(Autodesk.Revit.DB.UnitType.UT_Length);
                        Autodesk.Revit.DB.DisplayUnitType dut = fo.DisplayUnits;
                        switch (dut)
                        {
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_CENTIMETERS:
                                scale = 100;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMAL_FEET:
                                scale = 3.28084;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMAL_INCHES:
                                scale = 39.3701;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_DECIMETERS:
                                scale = 10;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES:
                                scale = 3.28084;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_FRACTIONAL_INCHES:
                                scale = 39.3701;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_MILLIMETERS:
                                scale = 1000;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_METERS:
                                scale = 1.0;
                                break;
                            case Autodesk.Revit.DB.DisplayUnitType.DUT_METERS_CENTIMETERS:
                                scale = 1.0;
                                break;
                            default:
                                scale = 1.0;
                                break;
                        }
                    }
                }
            }
            else
                scale = 3.14;

            return scale;
        }
    }
}
