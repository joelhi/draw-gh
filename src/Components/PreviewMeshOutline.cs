using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using drawgh.Utils;
using System.Drawing;

namespace drawgh.Components
{
    public class PreviewMeshOutline : GH_Component
    {

        Mesh mesh = null;
        Color col = Color.Black;
        int t = 1;
        Curve[] outline;

        public PreviewMeshOutline()
          : base("Preview Mesh Outline", "meshOutline",
            "Preview the outlines of a mesh, with a specific colour and thickness.",
            "Draw", GeneralUtils.PluginName)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh to draw outlines for", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "col", "Colour for outline display", GH_ParamAccess.item, Color.Black);
            pManager.AddIntegerParameter("Thickness", "t", "Thickness of line preview", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.GetData(0, ref mesh);
            DA.GetData(1, ref col);
            DA.GetData(2, ref t);

            if (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.IsParallelProjection)
            {
                // Get the active Rhino Viewport
                var viewplane = new Plane(Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraTarget, Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraZ);

                // Move towards camera so curve not blocked by object
                viewplane.Translate(viewplane.ZAxis * 0.8 * viewplane.Origin.DistanceTo(Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraLocation));

                // Generate outlines based on this viewport.
                outline = this.mesh.GetOutlines(viewplane).Select(pl => pl.ToNurbsCurve()).ToArray();
            }
            else
            {
                // Get the active Rhino Viewport
                var viewplane = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;

                // Generate outlines based on this viewport.
                outline = this.mesh.GetOutlines(viewplane).Select(pl => pl.ToNurbsCurve()).ToArray();
            }
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

            
            if (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.IsParallelProjection)
            {
                // Get the active Rhino Viewport
                var viewplane = new Plane(Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraTarget, Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraZ);

                // Move towards camera so curve not blocked by object
                viewplane.Translate(viewplane.ZAxis * 0.8 * viewplane.Origin.DistanceTo(Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraLocation));

                // Generate outlines based on this viewport.
                outline = this.mesh.GetOutlines(viewplane).Select(pl => pl.ToNurbsCurve()).ToArray();
            }
            else
            {
                // Get the active Rhino Viewport
                var viewplane = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;

                // Generate outlines based on this viewport.
                outline = this.mesh.GetOutlines(viewplane).Select(pl => pl.ToNurbsCurve()).ToArray();
            }

            // Draw curves
            for (int i = 0; i < outline.Length; i++)
                dp.DrawCurve(outline[i], this.col, this.t);
            
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("387383a1-90ab-4614-895f-ed37c25beff9"); }
        }
    }
}
