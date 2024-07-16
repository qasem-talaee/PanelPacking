using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanelPacking
{
    public class PanelsViewModel
    {
        public string PanelID { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Mxp { get; set; } = string.Empty;
        public string ExitTime { get; set; } = string.Empty;
        public int Sequence { get; set; }
    }
}
