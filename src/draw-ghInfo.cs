using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace drawgh
{
    public class draw_ghInfo : GH_AssemblyInfo
    {
        public override string Name => "Sketch";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "Quick set of tools for custom preview and image creation from grasshopper.";

        public override Guid Id => new Guid("37348BD4-3B4C-4CA9-AAF0-3D7B859678DE");

        //Return a string identifying you or your company.
        public override string AuthorName => "Joel Hilmersson";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "d.j.hilmersson@gmail.com";
    }
}