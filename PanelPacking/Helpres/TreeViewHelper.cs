using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace PanelPacking
{
    public static class TreeViewHelper
    {
        public static void SaveTreeView(this RadTreeView radTree)
        {
            radTree.SaveXML(Environment.CurrentDirectory + "\\Tree.xml");
        }

        public static void LoadTreeView(this RadTreeView radTree)
        {
            if (File.Exists(Environment.CurrentDirectory + "\\Tree.xml"))
                radTree.LoadXML(Environment.CurrentDirectory + "\\Tree.xml");
        }
    }
}
