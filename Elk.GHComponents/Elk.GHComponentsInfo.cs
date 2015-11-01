using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Elk.GHComponents
{
  public class ElkGHComponentsInfo : GH_AssemblyInfo
  {
    public override string Name
    {
      get
      {
        return "ElkGHComponents";
      }
    }
    public override Bitmap Icon
    {
      get
      {
        //Return a 24x24 pixel bitmap to represent this GHA library.
          return Properties.Resources.Elk_24x24;
      }
    }
    public override string Description
    {
      get
      {
        //Return a short string describing the purpose of this GHA library.
        return "OpenStreetMap and USGS Data Importers";
      }
    }
    public override Guid Id
    {
      get
      {
        return new Guid("e23e5827-fd6d-4074-988d-1f250878cd56");
      }
    }

    public override string AuthorName
    {
      get
      {
        //Return a string identifying you or your company.
        return "Timothy Logan";
      }
    }
    public override string AuthorContact
    {
      get
      {
        //Return a string representing your preferred contact details.
        return "elkdesigntech@gmail.com";
      }
    }
  }
}
