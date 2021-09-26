using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using drawgh.Utils;
using System.Drawing;

using System.Windows;
using System.Windows.Forms;

namespace drawgh.Components
{
    public class DrawCurvePattern : GH_Component, IGH_PreviewObject
    {

        public DrawCurvePattern()
          : base("DrawCurvePattern", "Nickname",
            "DrawCurvePattern description",
            "Draw", GeneralUtils.PluginName)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "crv", "Curve To Draw", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "col", "Colour for outline display", GH_ParamAccess.item, Color.Black);
            pManager.AddIntegerParameter("Thickness", "t", "Thickness of line preview", GH_ParamAccess.item, 1);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            var dp = args.Display;

            dp.DrawPatternedLine(null,Color.Beige,)
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("d4431596-ce9f-40f6-8253-cc849f25e362"); }
        }
    }
}
