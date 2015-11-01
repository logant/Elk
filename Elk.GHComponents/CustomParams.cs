using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Elk.Common;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;

namespace Elk.GHComponents
{
    public class OSMPointData : GH_Goo<OSMPoint>
    {
        public OSMPointData()
        {
            this.Value = null;
        }

        public OSMPointData(OSMPoint osmPoint)
        {
            this.Value = osmPoint;
        }

        public OSMPointData(OSMPointData osmSource)
        {
            this.Value = osmSource.Value;
        }

        public override IGH_Goo Duplicate()
        {
            return new OSMPointData(this);
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override string ToString()
        {
            try
            {
                return "OSM Point " + this.Value.Point.ToString();
            }
            catch
            {
                return null;
            }
        }

        public override string TypeDescription
        {
            get { return "OSM Point Data"; }
        }

        public override string TypeName
        {
            get { return "OSM Point"; }
        }
    }

    public class OSMPointParam : GH_PersistentParam<OSMPointData>
    {
        public OSMPointParam() : base("OSMPoints", "OSM", "OSM Point Data", "Extra", "Elk2") { }

        protected override GH_GetterResult Prompt_Plural(ref List<OSMPointData> values)
        {
            if (this.Prompt_ManageCollection(values))
                return GH_GetterResult.success;
            else
                return GH_GetterResult.cancel;
        }   

        protected override GH_GetterResult Prompt_Singular(ref OSMPointData value)
        {
            value = new OSMPointData();
            return GH_GetterResult.success;
        }

        private bool Prompt_ManageCollection(List<OSMPointData> values)
        {
            return true;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1a49a6fe-0c96-4119-8f0b-e6b797426416"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.hidden;
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return base.Icon;
            }
        }
    }

    public class OSMWayData : GH_Goo<OSMWay>
    {
        public OSMWayData()
        {
            this.Value = null;
        }

        public OSMWayData(OSMWay osmWay)
        {
            this.Value = osmWay;
        }

        public OSMWayData(OSMWayData source)
        {
            this.Value = source.Value;
        }

        public override IGH_Goo Duplicate()
        {
            return new OSMWayData(this);
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override string ToString()
        {
            try
            {
                return "OSMWay: [" + this.Value.Type + ":" + this.Value.Name + "]";
            }
            catch
            {
                return null;
            }
        }

        public override string TypeDescription
        {
            get { return "OSM Way Data"; }
        }

        public override string TypeName
        {
            get { return "OSM Way"; }
        }
    }

    public class OSMWayParam : GH_PersistentParam<OSMWayData>
    {
        public OSMWayParam() : base("OSMWay", "Way", "OSM Way Data", "Extra", "Elk2") { }

        protected override GH_GetterResult Prompt_Plural(ref List<OSMWayData> values)
        {
            if (this.Prompt_ManageCollection(values))
                return GH_GetterResult.success;
            else
                return GH_GetterResult.cancel;
        }   

        protected override GH_GetterResult Prompt_Singular(ref OSMWayData value)
        {
            value = new OSMWayData();
            return GH_GetterResult.success;
        }

        private bool Prompt_ManageCollection(List<OSMWayData> values)
        {
            return true;
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("daaa4684-0c3e-4367-8a67-62fbcd44e4a2"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.hidden;
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return base.Icon;
            }
        }
    }
}
