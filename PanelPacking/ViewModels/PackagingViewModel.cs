using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanelPacking
{
    public class PackagingViewModel
    {        
            public string SystemUserID { get; set; }

            //public DateTime IssueDate { get; set; }

            //[NotMapped]
            //public string PIssueDate { get { return IssueDate.Year != 1 ? IssueDate.ToShortDateString() : ""; } }
            public string IssueTime { get; set; } = string.Empty;

            public string PackID { get; set; } = string.Empty;
            public string LotNumber { get; set; } = string.Empty;
            public int LineNumber { get; set; }
            public List<PanelsViewModel> FL_Packaging_Panels { get; set; }
            public string PanelType { get; set; } = string.Empty;
            public string VisualCategory { get; set; } = string.Empty;
            public string Date { get; set; }

        
    }
}
