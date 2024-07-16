using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PanelPacking.Helpres
{
    public class CustomGridDataRowBehavior : GridDataRowBehavior
    {
        public override bool ProcessKey(KeyEventArgs keys)
        {
            if (keys.KeyCode == Keys.Up || keys.KeyCode == Keys.Down)
            {
                var oldCurrent = this.GridControl.CurrentRow;
                bool res = base.ProcessKey(keys);
                var newCurrent = this.GridControl.CurrentRow;

                if (newCurrent != null && newCurrent.Index > -1)
                {
                    for (int i = 0; i < oldCurrent.Cells.Count; i++)
                    {
                        object oldValue = oldCurrent.Cells[i].Value;
                        object newValue = newCurrent.Cells[i].Value;
                        newCurrent.Cells[i].Value = oldValue;
                        oldCurrent.Cells[i].Value = newValue;
                    }
                }

                return res;
            }

            return base.ProcessKey(keys);
        }
    }
}
