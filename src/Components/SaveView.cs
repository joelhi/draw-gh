using System;
using System.Collections.Generic;
using System.Drawing;
using drawgh.Utils;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Display;
using Rhino.Geometry;

namespace drawgh.Components
{
    public class SaveView : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SaveView()
          : base("SaveView", "Nickname",
            "SaveView description",
            GeneralUtils.PluginName, "Save")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "path", "Path to save file at.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "scale", "Scale", GH_ParamAccess.item, 1.0);
            pManager.AddBooleanParameter("Save", "save", "Save image", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double scale = 1;
            string path = "";
            bool save = false;

            DA.GetData(0, ref path);
            DA.GetData(1, ref scale);
            DA.GetData(2, ref save);

            var s = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.Size;

            var newHeight = s.Height * scale;
            var newWidth = s.Width * scale;

            s.Height = (int)newHeight;
            s.Width = (int)newWidth;

            if (save)
            { 
                var bitmap = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.CaptureToBitmap(s, false, false, false);

                bitmap.Save(path);
            }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c535dcaa-b44f-457c-8d6e-5bb5884bb221"); }
        }
    }
}
