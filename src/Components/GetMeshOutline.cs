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
    public class GetMeshOutline : GH_Component
    {
        Mesh mesh = null;
        Curve[] outline = null;
        bool continuousUpdate = true;

        public GetMeshOutline()
          : base("Get Mesh Outline", "meshOutline",
            "Get the outlines of a mesh, at the current display view.",
             GeneralUtils.PluginName,"Draw")
        {
        }

        //protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        //{
        //    Menu_AppendItem(menu, "Continuous Refresh?", Menu_PanelTypeChanged, true, continuousUpdate).Tag = "Refresh";
        //}


        //private void Menu_PanelTypeChanged(object sender, EventArgs e)
        //{
        //    if (sender is ToolStripMenuItem item && item.Tag is "Refresh")
        //        continuousUpdate = !continuousUpdate;
        //    this.ExpireSolution(true);
        //}

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh to draw outlines for", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Refresh", "r", "Refresh outline?", GH_ParamAccess.item, true);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Outline", "O", "Outline of mesh in current viewport.",GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool refr = true;

            DA.GetData(0, ref mesh);
            DA.GetData(1, ref refr);

            if (refr)
            {
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

            DA.SetDataList(0, this.outline);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("052a3feb-d781-460f-8850-b186e3b30196"); }
        }
    }
}
