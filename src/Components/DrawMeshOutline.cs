﻿using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using drawgh.Utils;
using System.Drawing;
using Rhino.Geometry.Intersect;

namespace drawgh.Components
{
    public class DrawMeshOutline : GH_Component, IGH_PreviewObject
    {

        int t = 1;
        Color col = Color.Black;
        Curve[] outline = null;
        Mesh projected = new Mesh();
        uint counter = 0;

        public DrawMeshOutline()
          : base("Draw Mesh Outline", "meshOutline",
            "Preview the outlines of a mesh, with a specific colour and thickness.",
            "Draw", GeneralUtils.PluginName)
        {
        }

        public override void CreateAttributes()
        {
            this.Attributes = new Grasshopper.Kernel.Attributes.GH_ComponentAttributes(this);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh to draw outlines for", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "col", "Colour for outline display", GH_ParamAccess.item, Color.Black);
            pManager.AddIntegerParameter("Thickness", "t", "Thickness of line preview", GH_ParamAccess.item, 1);
            //pManager.AddBooleanParameter("Refresh", "r", "Refresh, if display goes missing", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (counter == Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ChangeCounter)
            {
                this.Message = "Static View";
                this.OnPingDocument().ScheduleSolution(100, callback);
                return;
            }

            Mesh mesh = null;

            DA.GetData(0, ref mesh);
            DA.GetData(1, ref col);
            DA.GetData(2, ref t);

            if (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.IsParallelProjection)
            {
                // Get the active Rhino Viewport
                Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.GetFrustumNearPlane(out Plane viewplane);

                // Generate outlines based on this viewport.
                outline = mesh.GetOutlines(viewplane).Select(pl => pl.ToNurbsCurve()).ToArray();
            }
            else
            {
                // Get the active Rhino Viewport
                var viewplane = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;

                // Generate outlines based on this viewport.
                outline = mesh.GetOutlines(viewplane).Select(pl => pl.ToNurbsCurve()).ToArray();
            }

            this.Message = "View Updated";

            counter = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.ChangeCounter;
            this.OnPingDocument().ScheduleSolution(100, callback);

        }

        private void callback(GH_Document doc)
        {
            this.ExpireSolution(false);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            var dp = args.Display;

            if (outline is null)
                return;

            for (int i = 0; i < outline.Length; i++)
            {
                dp.DrawCurve(outline[i], col, t);
            }
        }



        //public override bool IsPreviewCapable => true;


        public override Guid ComponentGuid
        {
            get { return new Guid("387383a1-90ab-4614-895f-ed37c25beff9"); }
        }
    }
}