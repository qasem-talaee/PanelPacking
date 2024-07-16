using PanelPacking.Helpres;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using static Stimulsoft.Report.StiOptions.Designer;

namespace PanelPacking.Views
{
    public partial class frm_edit : Form
    {
        string path;
        string UserId;
        string UpdateUrl;
        string LineNumber;
        List<view_model_barcode> panelList = new List<view_model_barcode>();
        private Logger _logger = new Logger();
        public frm_edit(string path, string UserId, string UpdateUrl, string LineNumber)
        {
            InitializeComponent();
            try
            {
                string[] out1 = File.ReadAllLines(path);
                var cell = out1[0].Split('-');
                if (cell.Length == 6)
                {
                    this.path = path;
                    this.UserId = UserId;
                    this.UpdateUrl = UpdateUrl;
                    this.LineNumber = LineNumber;
                }
                else
                {
                    MessageBox.Show("Report Files has incorrect format.Please Contact to the IT Department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Report Files has incorrect format.Please Contact to the IT Department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }

        private void frm_edit_Load(object sender, EventArgs e)
        {
            radGridView1.BestFitColumns(BestFitColumnMode.DisplayedCells);
            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            BaseGridBehavior gridBehavior = radGridView1.GridBehavior as BaseGridBehavior;
            gridBehavior.UnregisterBehavior(typeof(GridViewDataRowInfo));
            gridBehavior.RegisterBehavior(typeof(GridViewDataRowInfo), new CustomGridDataRowBehavior());

            string[] out1 = File.ReadAllLines(path);
            for (int i = 0; i < out1.Length; i++)
            {
                var cell = out1[i].Split('-');
                panelList.Add(new view_model_barcode
                {
                    MXP = Convert.ToInt32(cell[0]),
                    SN = cell[1],
                    VC = cell[2],
                    PT = cell[3],
                    ExitTime = cell[4].ToString(),
                    Grade = cell[5]
                });
            }

            radGridView1.DataSource = null;
            radGridView1.DataSource = panelList;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("آیا می خواهید پنل ها را پک کنید؟", "تاییده", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    string datedir = DateTime.Now.ToString("yyyyMMdd");
                    string newPath = Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + datedir;
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }

                    List<PanelsViewModel> PanelviewList = new List<PanelsViewModel>();
                    for (int i = 0; i < panelList.Count(); i++)
                    {
                        PanelviewList.Add(new PanelsViewModel
                        {
                            PanelID = string.Empty,
                            SerialNumber = panelList[i].SN,
                            Mxp = panelList[i].MXP.ToString(),
                            ExitTime = panelList[i].ExitTime.ToString(),
                            Sequence = panelList.Count() - i,
                        });
                    }
                    string PackID = path.Split('\\').Last().Split('.')[0].ToString();
                    PackagingViewModel packagingViewModel = new PackagingViewModel();
                    packagingViewModel.FL_Packaging_Panels = new List<PanelsViewModel>();
                    packagingViewModel.SystemUserID = UserId;
                    packagingViewModel.IssueTime = string.Empty;
                    packagingViewModel.PackID = PackID;
                    packagingViewModel.LotNumber = panelList[0].Grade;
                    packagingViewModel.LineNumber = Convert.ToInt32(LineNumber);
                    packagingViewModel.FL_Packaging_Panels = PanelviewList;
                    packagingViewModel.PanelType = panelList[0].PT;
                    packagingViewModel.VisualCategory = panelList[0].VC;
                    packagingViewModel.Date = string.Empty;

                    if (File.Exists(Environment.CurrentDirectory + "\\Report\\Report.mrt") && File.Exists(Environment.CurrentDirectory + "\\Report\\Logo.png"))
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(UpdateUrl);

                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                var response = client.PostAsJsonAsync("flow/UpdatePackagingLog/", packagingViewModel).Result;
                                var res = await response.Content.ReadAsAsync<PackagingViewModel>();

                                string date = res.Date;

                                var report = new StiReport();
                                packagingViewModel.Date = date;
                                report.RegBusinessObject("p", packagingViewModel);
                                report.RegBusinessObject("pn", packagingViewModel.FL_Packaging_Panels);

                                report.Load(Environment.CurrentDirectory + "\\Report\\Report.mrt");
                                report.Render();
                                string fileName = string.Format("{0}.jpg", PackID);
                                string dateDir = DateTime.Now.ToString("yyyyMMdd");
                                if (!Directory.Exists(Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + dateDir))
                                {
                                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + dateDir);
                                }
                                report.ExportDocument(StiExportFormat.ImageJpeg, Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + dateDir + "\\" + fileName);
                                report.Show(true);

                                List<string> o1 = new List<string>();
                                this._logger.writeLogPackEdit(PackID);
                                panelList.ForEach(x => o1.Add(x.MXP.ToString() + "-" + x.SN + "-" + x.VC + "-" + x.PT + "-" + x.ExitTime.ToString() + "-" + x.Grade));

                                File.WriteAllLines(Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + dateDir + string.Format("\\{0}.txt", PackID), o1);

                                this.Close();

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Print Exception : " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("فایل ریپورت پیدا نشد.با پشتیبانی نرم افزار تماس بگیرید", "تاییدیه", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Your inputs are in wrong format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("آیا می خواهید پک را برعکس کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this._logger.writeLogPackEditReverse(path.Split('\\').Last().Split('.')[0].ToString());
                this.panelList.Reverse();
                radGridView1.DataSource = null;
                radGridView1.DataSource = panelList;
            }
        }

        private void btn_edit_row_Click(object sender, EventArgs e)
        {
            if(mxpEdit_input.Value != 0 || SNedit_input.Value != 0)
            {
                string newMxp = mxpEdit_input.Value.ToString();
                string newSN = SNedit_input.Value.ToString();
                DialogResult dialogResult = MessageBox.Show("آیا می خواهید آپدیت کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    this._logger.writeLogPackEditPanel(radGridView1.SelectedRows[0].Cells[1].Value.ToString() + "\tMXP=" + newMxp + "\tSN=" + newSN);
                    radGridView1.SelectedRows[0].Cells[0].Value = Convert.ToInt32(newMxp);
                    radGridView1.SelectedRows[0].Cells[1].Value = newSN;
                }
            }
            else
            {
                MessageBox.Show("برای آپدیت ابتدا یک سطر را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grid_cell_click(object sender, GridViewCellEventArgs e)
        {
            if (radGridView1.SelectedRows.Count() > 0)
            {
                int MXP = Convert.ToInt32(radGridView1.SelectedRows[0].Cells[0].Value);
                string SN = radGridView1.SelectedRows[0].Cells[1].Value.ToString();
                mxpEdit_input.Value = MXP;
                SNedit_input.Value = Convert.ToInt32(SN);
            }
        }
    }
}
