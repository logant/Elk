using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino;

namespace Elk.GHComponents
{
    public class CommonGHProcessors
    {
        public static double GetRhinoUnitScale(RhinoDoc rhinoDoc)
        {
            double scale = 1.0;
            UnitSystem us = rhinoDoc.ModelUnitSystem;
            string units = us.ToString();
            switch (units)
            {
                case "None":
                    scale = 1;
                    break;
                case "Microns":
                    scale = 1000000;
                    break;
                case "Millimeters":
                    scale = 1000;
                    break;
                case "Centimeters":
                    scale = 100;
                    break;
                case "Meters":
                    scale = 1;
                    break;
                case "Kilometers":
                    scale = 0.001;
                    break;
                case "Microinches":
                    scale = 39370100;
                    break;
                case "Mils":
                    scale = 39370.0787;
                    break;
                case "Inches":
                    scale = 39.3701;
                    break;
                case "Feet":
                    scale = 3.28084;
                    break;
                case "Miles":
                    scale = 0.000621371;
                    break;
                case "Angstroms":
                    scale = 10000000000;
                    break;
                case "Nanometers":
                    scale = 1000000000;
                    break;
                case "Decimeters":
                    scale = 10;
                    break;
                case "Dekameters":
                    scale = 0.1;
                    break;
                case "Hectometers":
                    scale = 0.01;
                    break;
                case "Megameters":
                    scale = 0.000001;
                    break;
                case "Gigameters":
                    scale = 0.000000001;
                    break;
                case "Yards":
                    scale = 1.09361;
                    break;
                case "PrinterPoint":
                    scale = 1;
                    break;
                case "PrinterPica":
                    scale = 1;
                    break;
                case "NauticalMile":
                    scale = 0.000539957;
                    break;
                case "Astronomical":
                    scale = 0.00000000000668458712;
                    break;
                case "Lightyears":
                    scale = 0.000000000000000105702341;
                    break;
                case "Parsecs":
                    scale = 0.0000000000000000324077929;
                    break;
                default:
                    break;
            }

            return scale;
        }
    }
}
