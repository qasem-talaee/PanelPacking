using Newtonsoft.Json.Linq;
using PanelPacking.Helpres;
using PanelPacking.ViewModels;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using static Stimulsoft.Base.StiDataLoaderHelper;

using static Telerik.WinControls.VirtualKeyboard.VirtualKeyboardNativeMethods;

namespace PanelPacking.Views
{
    public partial class frm_Main : Form
    {
        public frm_Main()
        {
            InitializeComponent();
        }

        private SerialPort _serial;
        private Logger _logger = new Logger();
        public string[] Config { get; set; }
        List<PanelConfigValue> PolyConfig = new List<PanelConfigValue>();
        List<PanelConfigValue> MonoConfig = new List<PanelConfigValue>();
        List<PanelConfigValue> MonoLikeConfig = new List<PanelConfigValue>();

        List<view_model_barcode> panelsvm = new List<view_model_barcode>();
        List<view_model_barcode> panelprintsvm = new List<view_model_barcode>();

        List<imageview> imageview = new List<imageview>();
        string UserId;
        int MaxPanelNumberInPack;
        private void frm_Main_Load(object sender, EventArgs e)
        {
            this._logger.writeLogLogin(UserId);
            #region Load Panel Configs
            PolyConfig.Add(new PanelConfigValue() { Index = 0, Min = 0, Max = 1000 });
            MonoConfig.Add(new PanelConfigValue() { Index = 0, Min = 0, Max = 1000 });
            MonoLikeConfig.Add(new PanelConfigValue() { Index = 0, Min = 0, Max = 1000 });

            if (File.Exists(Environment.CurrentDirectory + "\\config_mono.ini"))
            {
                int i = 0;
                foreach (var item in File.ReadAllLines(Environment.CurrentDirectory + "\\config_mono.ini").ToList())
                {
                    MonoConfig.Add(new PanelConfigValue()
                    {
                        Index = ++i,
                        Min = Convert.ToInt32(item.Split('-')[0]),
                        Max = Convert.ToInt32(item.Split('-')[1]),
                    });
                }
            }


            if (File.Exists(Environment.CurrentDirectory + "\\config_monolike.ini"))
            {
                int i = 0;
                foreach (var item in File.ReadAllLines(Environment.CurrentDirectory + "\\config_monolike.ini").ToList())
                {
                    MonoLikeConfig.Add(new PanelConfigValue()
                    {
                        Index = ++i,
                        Min = Convert.ToInt32(item.Split('-')[0]),
                        Max = Convert.ToInt32(item.Split('-')[1]),
                    });
                }
            }


            if (File.Exists(Environment.CurrentDirectory + "\\config_poly.ini"))
            {
                int i = 0;
                foreach (var item in File.ReadAllLines(Environment.CurrentDirectory + "\\config_poly.ini").ToList())
                {
                    PolyConfig.Add(new PanelConfigValue()
                    {
                        Index = ++i,
                        Min = Convert.ToInt32(item.Split('-')[0]),
                        Max = Convert.ToInt32(item.Split('-')[1]),
                    });
                }
            }

            if (File.Exists(Environment.CurrentDirectory + "\\config_pack.ini"))
            {
                this.MaxPanelNumberInPack = Int32.Parse(File.ReadAllLines(Environment.CurrentDirectory + "\\config_pack.ini").ToList()[0].Replace("\n", "").Replace(" ", ""));
                this.num_MaxPanelInPack.Value = this.MaxPanelNumberInPack;
            }
            #endregion

            #region Load Programm Config
            if (File.Exists(Environment.CurrentDirectory + "\\Config.ini"))
                Config = File.ReadAllLines(Environment.CurrentDirectory + "\\Config.ini");
            else
                Application.Exit();
            #endregion
            try
            {
                _serial = new SerialPort("COM" + Config[0], 9600, Parity.None, 8, StopBits.One);
                _serial.Handshake = Handshake.None;
                _serial.ReadTimeout = 1500;
                _serial.Encoding = Encoding.ASCII;
                _serial.DtrEnable = true;
                _serial.RtsEnable = true;
                _serial.Open();
                _serial.DataReceived += _serial_DataReceived;
            }
            catch
            {
                MessageBox.Show("پورت کام پیدا نشد.با پشتیبانی نرم افزار تماس بگیرید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this._logger.writeLogErrorScanner("COM NOT FOUND");
                this.Close();
            }


            // radGridView1.DataSource = panelsvm;
            drl_panelType.SelectedIndex = 0;
            today_work_grid.BestFitColumns(BestFitColumnMode.DisplayedCells);
            current_pack_grid.BestFitColumns(BestFitColumnMode.DisplayedCells);
            today_pack_image_grid.BestFitColumns(BestFitColumnMode.DisplayedCells);
            readfromfile();

            //register the custom row  behavior
            this.today_work_grid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.current_pack_grid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.today_pack_image_grid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            BaseGridBehavior gridBehavior = current_pack_grid.GridBehavior as BaseGridBehavior;
            gridBehavior.UnregisterBehavior(typeof(GridViewDataRowInfo));
            gridBehavior.RegisterBehavior(typeof(GridViewDataRowInfo), new CustomGridDataRowBehavior());
            UserId = user_info();
            show_packs_list();
            lbl_last_panel.Text = "";
            today_work_grid.GroupDescriptors.Add("VC", ListSortDirection.Ascending);

            this.count_pack_lbl.Text = "Count : " + panelprintsvm.Count().ToString();

        }

        private string user_info()
        {
            string[] user = File.ReadAllLines(Environment.CurrentDirectory + "\\session");
            this.Text = string.Format("Welcome " + user[1] + " in shift " + user[2]);
            return user[0];

        }
        //[MXP:309][Vmp:36.7][Ipm:8.41][Voc:45.0][Isc:8.96][Msv:IEC1500V/UL1500V][MSF:15A][Grade:Lot 7][SN:140202315][VC:A][PT:Multi]
        private void show_packs_list()
        {
            int i = 0;
            string dateDir = DateTime.Now.ToString("yyyyMMdd");
            if (Directory.Exists(Environment.CurrentDirectory + "\\PrintHistory\\" + dateDir))
            {
                DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory + "\\PrintHistory\\" + dateDir);
                FileInfo[] Files = d.GetFiles("*.jpg");
                imageview.RemoveAll(x => x != null);
                foreach (FileInfo file in Files)
                {
                    imageview.Add(new imageview
                    {
                        count = ++i,
                        name = file.Name,
                    });
                }
            }

            if (Directory.Exists(Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + dateDir))
            {
                DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + dateDir);
                FileInfo[] Files = d.GetFiles("*.jpg");
                foreach (FileInfo file in Files)
                {
                    imageview.Add(new imageview
                    {
                        count = ++i,
                        name = "Up " + file.Name,
                    });
                }
            }
            today_pack_image_grid.DataSource = null;
            today_pack_image_grid.DataSource = imageview;
        }

        private async void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            byte[] data = new byte[1024];
            int bytesRead = _serial.Read(data, 0, data.Length);

            var res = Encoding.ASCII.GetString(data, 0, bytesRead);
            if (res.Length > 15)
            {

                var o = res.Replace("]", "").Replace("\n", "");
                var i = o.Split('[');
                var mxp = i[1].Split(':')[1];
                var Vmp = i[2].Split(':')[1];
                var Ipm = i[3].Split(':')[1];
                var Voc = i[4].Split(':')[1];
                var Isc = i[5].Split(':')[1];
                var Msv = i[6].Split(':')[1];
                var MSF = i[7].Split(':')[1];
                var Grade = i[8].Split(':')[1];
                var SN = i[9].Split(':')[1];
                var VC = i[10].Split(':')[1];
                var PT = i[11].Split(':')[1].Replace(" ", "").Replace("\r", "");


                var vm = new view_model_barcode()
                {
                    MXP = Convert.ToInt32(mxp),
                    Vmp = Vmp,
                    Ipm = Ipm,
                    Voc = Voc,
                    Isc = Isc,
                    Msv = Msv,
                    MSF = MSF,
                    Grade = Grade,
                    SN = SN,
                    VC = VC,
                    PT = PT.Replace("Multi", "Poly"),
                    ExitTime = DateTime.Now.ToString(),
                };
                if (panelsvm.Where(x => x.SN == vm.SN).Count() == 0 && panelprintsvm.Where(x => x.SN == vm.SN).Count() == 0)
                {
                    string url = Config[1];
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(url);

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = client.GetAsync($"flow/CheckPanelExist/{vm.SN}").Result;
                        var resExist = response.Content.ReadAsStringAsync().Result;
                        if(resExist.Contains("-"))
                        {
                            DialogResult dialogResult = MessageBox.Show("این پنل در لاگشیت موجود است.آیا می خواهید اضافه کنید؟\nPackID : " + resExist + "\nSerial Number : " + vm.SN, "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                panelsvm.Add(vm);
                                SetControlText(this, vm);
                                this._logger.writeLogScanYes(vm.SN);
                            }
                            else
                            {
                                this._logger.writeLogScanNo(vm.SN);
                            }
                        }
                        else
                        {
                            panelsvm.Add(vm);
                            SetControlText(this, vm);
                            this._logger.writeLogScan(vm.SN);
                        }
                        this.savetofile();
                    }
                }
                else
                {
                    MessageBox.Show("این پنل قبلا اسکن شده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SetControlText(Control control, view_model_barcode text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Control, view_model_barcode>(SetControlText), new object[] { control, text });
            }
            else
            {
                lbl_last_panel.Text = "Serial Number : " + text.SN + "\r\n" + "MXP : " + text.MXP + "\r\n" + "VC : " + text.VC;

                //this.radGridView1.DataSource = null;
                var s = (PanelConfigValue)this.drl_grades.SelectedItem.Tag;
                this.today_work_grid.DataSource = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower());
                //CheckForPrint();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                panelsvm.Add(new view_model_barcode()
                {
                    MXP = rnd.Next(300, 350),
                    PT = "Poly",
                    VC = "A",
                    SN = rnd.Next(100000, 888888).ToString(),
                    Grade = "Lot",
                    ExitTime = "2:22",
                });

                panelsvm.Add(new view_model_barcode()
                {
                    MXP = rnd.Next(400, 500),
                    PT = "Mono",
                    VC = "B",
                    SN = rnd.Next(100000, 888888).ToString(),
                    ExitTime = "2:22",
                    Grade = "Lot",
                });

                panelsvm.Add(new view_model_barcode()
                {
                    MXP = rnd.Next(250, 360),
                    PT = "MonoLike",
                    VC = "A",
                    SN = rnd.Next(100000, 888888).ToString(),
                    Grade = "Lot",
                    ExitTime = "2:22",
                });
            }

            today_work_grid.DataSource = null;
            today_work_grid.DataSource = panelsvm;
        }

        private void drl_panelType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            drl_grades.Items.Clear();

            if (drl_panelType.SelectedText.ToLower() == "mono")
            {
                foreach (var item in MonoConfig)
                {
                    drl_grades.Items.Add(new RadListDataItem()
                    {
                        Text = item.Index + " " + item.Min.ToString() + "-" + item.Max.ToString() + ":",
                        Tag = item
                    });
                }
            }

            if (drl_panelType.SelectedText.ToLower() == "monolike")
            {
                foreach (var item in MonoLikeConfig)
                {
                    drl_grades.Items.Add(new RadListDataItem()
                    {
                        Text = item.Index + " " + item.Min.ToString() + "-" + item.Max.ToString() + ":",
                        Tag = item
                    });
                }
            }

            if (drl_panelType.SelectedText.ToLower() == "poly")
            {
                foreach (var item in PolyConfig)
                {
                    drl_grades.Items.Add(new RadListDataItem()
                    {
                        Text = item.Index + " " + item.Min.ToString() + "-" + item.Max.ToString() + ":",
                        Tag = item
                    });
                }
            }
            drl_grades.SelectedIndex = 0;
            try
            {
                var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                today_work_grid.DataSource = panelsvm
                   .Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower())
                   .OrderBy(x => DateTime.Parse(x.ExitTime));
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Please Contact to IT Department", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("لطفا با پشتیبانی نرم افزار تماس بگیرید \r\n" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void drl_grades_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (drl_grades.SelectedItem != null)
            {
                var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                today_work_grid.DataSource = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower());
            }
            try
            {
                var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                today_work_grid.DataSource = panelsvm
                   .Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower())
                   .OrderBy(x => DateTime.Parse(x.ExitTime));
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Please Contact to IT Department", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("لطفا با پشتیبانی نرم افزار تماس بگیرید \r\n" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {

            if (e.Column != null)
            {
                //.Cells[4].Value
                try
                {
                    exit_time_label.Text = "Exit Time : " + today_work_grid.SelectedRows[0].Cells[4].Value.ToString();
                }
                catch { }
                if (e.Column.GetType() == typeof(GridViewCommandColumn))
                {
                    var btn = (GridViewCommandColumn)e.Column;

                    if (btn.Name == "btn_Remove")
                    {
                        DialogResult dialogResult = MessageBox.Show("آیا می خواهید حذف کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            string SN = today_work_grid.SelectedRows[0].Cells[1].Value.ToString();
                            panelsvm.Remove(panelsvm.FirstOrDefault(x => x.SN == SN));
                            var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;

                            today_work_grid.DataSource = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower());
                            this._logger.writeLogRemoveToday(SN);
                            savetofile();
                        }

                    }
                    if (btn.Name == "btn_ForPrint")
                    {
                        if (today_work_grid.SelectedRows.Count() > 0)
                        {
                            if (panelprintsvm.Where(x => x.SN == today_work_grid.SelectedRows[0].Cells[1].Value.ToString()).Count() == 0)
                            {
                                panelprintsvm.Add(new view_model_barcode()
                                {
                                    MXP = Convert.ToInt32(today_work_grid.SelectedRows[0].Cells[0].Value),
                                    SN = today_work_grid.SelectedRows[0].Cells[1].Value.ToString(),
                                    VC = today_work_grid.SelectedRows[0].Cells[2].Value.ToString(),
                                    PT = today_work_grid.SelectedRows[0].Cells[3].Value.ToString(),
                                    ExitTime = today_work_grid.SelectedRows[0].Cells[4].Value.ToString(),
                                    Grade = panelsvm.FirstOrDefault(x => x.SN == today_work_grid.SelectedRows[0].Cells[1].Value.ToString()).Grade,
                                });
                                this._logger.writeLogAddToPack(today_work_grid.SelectedRows[0].Cells[1].Value.ToString());

                                current_pack_grid.DataSource = null;
                                current_pack_grid.DataSource = panelprintsvm;

                                panelsvm.Remove(panelsvm.FirstOrDefault(x => x.SN == today_work_grid.SelectedRows[0].Cells[1].Value.ToString()));

                                var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                                today_work_grid.DataSource = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower());

                                this.count_pack_lbl.Text = "Count : " + panelprintsvm.Count().ToString();
                                savetofile();
                            }
                            else
                            {
                                MessageBox.Show("این پنل قبلا اسکن شده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void radGridView2_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.GetType() == typeof(GridViewCommandColumn))
            {
                if (current_pack_grid.SelectedRows.Count() > 0)
                {
                    if(panelsvm.Where(x => x.SN == current_pack_grid.SelectedRows[0].Cells[1].Value.ToString()).Count() == 0)
                    {
                        panelsvm.Add(new view_model_barcode()
                        {
                            MXP = Convert.ToInt32(current_pack_grid.SelectedRows[0].Cells[0].Value),
                            SN = current_pack_grid.SelectedRows[0].Cells[1].Value.ToString(),
                            VC = current_pack_grid.SelectedRows[0].Cells[2].Value.ToString(),
                            PT = current_pack_grid.SelectedRows[0].Cells[3].Value.ToString(),
                            ExitTime = current_pack_grid.SelectedRows[0].Cells[4].Value.ToString(),
                            Grade = panelprintsvm.Where(x => x.SN == current_pack_grid.SelectedRows[0].Cells[1].Value.ToString()).ToList()[0].Grade,
                        });
                        this._logger.writeLogRemoveFromPack(current_pack_grid.SelectedRows[0].Cells[1].Value.ToString());

                        var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                        //  radGridView1.DataSource = null;
                        //panelsvm = panelsvm.OrderByDescending(x => DateTime.TryParse(x.ExitTime)).ToList();
                        today_work_grid.DataSource = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower());

                        panelprintsvm.Remove(panelprintsvm.FirstOrDefault(x => x.SN == current_pack_grid.SelectedRows[0].Cells[1].Value.ToString()));

                        current_pack_grid.DataSource = null;
                        current_pack_grid.DataSource = panelprintsvm;

                        this.count_pack_lbl.Text = "Count : " + panelprintsvm.Count().ToString();
                        savetofile();
                    }
                    else
                    {
                        MessageBox.Show("این پنل قبلا اسکن شده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    try
                    {
                        var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                        today_work_grid.DataSource = panelsvm
                           .Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower())
                           .OrderBy(x => DateTime.Parse(x.ExitTime));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Please Contact to IT Department", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show("لطفا با پشتیبانی نرم افزار تماس بگیرید \r\n" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void savetofile()
        {
            List<string> o1 = new List<string>();
            List<string> o2 = new List<string>();
            panelsvm.ForEach(x => o1.Add(x.MXP.ToString() + "-" + x.SN + "-" + x.VC + "-" + x.PT + "-" + x.ExitTime.ToString() + "-" + x.Grade));
            panelprintsvm.ForEach(x => o2.Add(x.MXP.ToString() + "-" + x.SN + "-" + x.VC + "-" + x.PT + "-" + x.ExitTime.ToString() + "-" + x.Grade));

            string folderPath = Environment.CurrentDirectory + "\\LogGrid\\" + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.Copy(Environment.CurrentDirectory + "\\gridAll.txt", folderPath + "\\gridAll (" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-" + Guid.NewGuid() + ").txt");
            File.Copy(Environment.CurrentDirectory + "\\gridSort.txt", folderPath + "\\gridSort (" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-" + Guid.NewGuid() + ").txt");

            File.WriteAllLines(Environment.CurrentDirectory + "\\gridAll.txt", o1);
            File.WriteAllLines(Environment.CurrentDirectory + "\\gridSort.txt", o2);

            File.WriteAllText(Environment.CurrentDirectory + "\\config_pack.ini", this.num_MaxPanelInPack.Value.ToString());
        }

        public void readfromfile()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\gridAll.txt") && File.Exists(Environment.CurrentDirectory + "\\gridSort.txt"))
            {
                string[] in1 = File.ReadAllLines(Environment.CurrentDirectory + "\\gridAll.txt");

                for (int i = 0; i < in1.Length; i++)
                {
                    var cell = in1[i].Split('-');
                    panelsvm.Add(new view_model_barcode()
                    {
                        MXP = Convert.ToInt32(cell[0]),
                        SN = cell[1],
                        VC = cell[2],
                        PT = cell[3],
                        ExitTime = cell[4],
                        Grade = cell[5]
                    });
                }
                in1 = File.ReadAllLines(Environment.CurrentDirectory + "\\gridSort.txt");
                for (int i = 0; i < in1.Length; i++)
                {
                    var cell = in1[i].Split('-');
                    panelprintsvm.Add(new view_model_barcode()
                    {
                        MXP = Convert.ToInt32(cell[0]),
                        SN = cell[1],
                        VC = cell[2],
                        PT = cell[3],
                        ExitTime = cell[4],
                        Grade = cell[5]
                    });
                }

                if (drl_grades.SelectedItem != null)
                {

                    var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                    today_work_grid.DataSource = null;
                    today_work_grid.DataSource = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower());

                    current_pack_grid.DataSource = null;
                    current_pack_grid.DataSource = panelprintsvm;
                }
            }

        }

        private void frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            savetofile();
            _serial.Close();
            this._logger.writeLogClose(UserId.ToString());
            LoginHelper login = new LoginHelper();
            login.LogOut();

        }

        private void radGridView3_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.GetType() == typeof(GridViewCommandColumn))
            {
                if (today_pack_image_grid.SelectedRows.Count() > 0)
                {
                    string name = today_pack_image_grid.SelectedRows[0].Cells[1].Value.ToString();
                    string dateDir = DateTime.Now.ToString("yyyyMMdd");
                    if (name.Split(' ').Length > 1)
                    {
                        string path = Environment.CurrentDirectory + "\\PrintHistory\\Update\\" + dateDir + "\\" + name.Split(' ')[1];
                        System.Diagnostics.Process.Start(path);
                    }
                    else
                    {
                        string path = Environment.CurrentDirectory + "\\PrintHistory\\" + dateDir + "\\" + name;
                        System.Diagnostics.Process.Start(path);
                    }
                }
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog s = new OpenFileDialog();
            s.InitialDirectory = Environment.CurrentDirectory + "\\PrintHistory\\";
            s.RestoreDirectory = true;
            s.Filter = "|*.txt";
            DialogResult result = s.ShowDialog();
            if (result == DialogResult.OK)
            {
                this._logger.writeLogGoToEdit(s.FileName);
                var frm_edit = new frm_edit(s.FileName, UserId, Config[1], Config[2]);
                frm_edit.ShowDialog();
                show_packs_list();
            }
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            string url = Config[1];
            List<PanelsViewModel> PanelviewList = new List<PanelsViewModel>();
            if (panelprintsvm.Count == 0)
            {
                MessageBox.Show("جدول پک خالی است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int i = 0; i < panelprintsvm.Count(); i++)
                {
                    PanelviewList.Add(new PanelsViewModel
                    {
                        PanelID = string.Empty,
                        SerialNumber = panelprintsvm[i].SN,
                        Mxp = panelprintsvm[i].MXP.ToString(),
                        ExitTime = panelprintsvm[i].ExitTime.ToString(),
                        Sequence = panelprintsvm.Count() - i,
                    });
                }

                PackagingViewModel packagingViewModel = new PackagingViewModel();
                packagingViewModel.FL_Packaging_Panels = new List<PanelsViewModel>();
                packagingViewModel.SystemUserID = UserId;
                packagingViewModel.IssueTime = string.Empty;
                packagingViewModel.PackID = string.Empty;
                packagingViewModel.LotNumber = panelprintsvm[0].Grade;
                packagingViewModel.LineNumber = Convert.ToInt32(Config[2]);
                packagingViewModel.FL_Packaging_Panels = PanelviewList;
                packagingViewModel.PanelType = panelprintsvm[0].PT;
                packagingViewModel.VisualCategory = panelprintsvm[0].VC;
                packagingViewModel.Date = string.Empty;

                DialogResult dialogResult = MessageBox.Show("آیا می خواهید پنل ها را پک کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (File.Exists(Environment.CurrentDirectory + "\\Report\\Report.mrt") && File.Exists(Environment.CurrentDirectory + "\\Report\\Logo.png"))
                    {
                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(url);

                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                var response = client.PostAsJsonAsync("flow/AddPackagingLog/", packagingViewModel).Result;
                                var res = await response.Content.ReadAsAsync<PackagingViewModel>();

                                string PackID = res.PackID;
                                string date = res.Date;

                                var report = new StiReport();
                                packagingViewModel.PackID = PackID;
                                packagingViewModel.Date = date;
                                report.RegBusinessObject("p", packagingViewModel);
                                report.RegBusinessObject("pn", packagingViewModel.FL_Packaging_Panels);

                                report.Load(Environment.CurrentDirectory + "\\Report\\Report.mrt");
                                report.Render();
                                string fileName = string.Format("{0}.jpg", PackID);
                                string dateDir = DateTime.Now.ToString("yyyyMMdd");
                                if (!Directory.Exists(Environment.CurrentDirectory + "//PrintHistory/" + dateDir))
                                {
                                    Directory.CreateDirectory(Environment.CurrentDirectory + "//PrintHistory/" + dateDir);
                                }
                                report.ExportDocument(StiExportFormat.ImageJpeg, Environment.CurrentDirectory + "//PrintHistory/" + dateDir + "/" + fileName);
                                
                                //report.ExportDocument(StiExportFormat.Word2007, Environment.CurrentDirectory + "//PrintHistory/" + dateDir + "/" + fileName+".doc");
                                report.Show(true);

                                List<string> o1 = new List<string>();

                                panelprintsvm.ForEach(x => o1.Add(x.MXP.ToString() + "-" + x.SN + "-" + x.VC + "-" + x.PT + "-" + x.ExitTime.ToString() + "-" + x.Grade));
                                this._logger.writeLogPack(PackID);
                                File.WriteAllLines(Environment.CurrentDirectory + "\\PrintHistory\\" + dateDir + string.Format("\\{0}.txt", PackID), o1);
                                panelprintsvm.RemoveAll(x => x != null);
                                current_pack_grid.DataSource = null;
                                current_pack_grid.DataSource = panelprintsvm;

                                show_packs_list();
                                savetofile();

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Print Exception : " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("فایل ریپورت پیدا نشد.با پشتیبانی نرم افزار تماس بگیرید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }

        private void drl_grades_PopupOpened(object sender, EventArgs e)
        {
            //foreach (var item in drl_grades.Items)
            //{
            //    var s = (PanelConfigValue)item.Tag;
            //    var A_panels = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower() && x.VC == "A");
            //    var B_panels = panelsvm.Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower() && x.VC == "B");
            //    var A_resCount = A_panels.Count();
            //    var B_resCount = B_panels.Count();
            //    #region Update DropDown
            //    if (item.Text.IndexOf("(") != -1)
            //    {
            //        int start = item.Text.IndexOf("(");
            //        int end = item.Text.IndexOf(")");
            //        item.Text = item.Text.Remove(start - 1, end - start + 2);
            //        item.Text = item.Text.Split(':')[0] + " (#A = " + A_resCount + " - #B = " + B_resCount + ")";
            //    }
            //    else
            //    {
            //        item.Text = item.Text.Split(':')[0] + " (#A = " + A_resCount + " - #B = " + B_resCount + ")";
            //    }
            //    #endregion

            //}
        }

        private void radGridView1_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            CheckForPrint();

            foreach (var item in drl_grades.Items)
            {
                var s = (PanelConfigValue)item.Tag;
                var A_panels = panelsvm.Where(x => x.PT.ToLower() == drl_panelType.Text.ToLower() && x.VC == "A").Where(x => x.MXP >= s.Min && x.MXP <= s.Max).ToList();
                var B_panels = panelsvm.Where(x => x.PT.ToLower() == drl_panelType.Text.ToLower() && x.VC == "B").Where(x => x.MXP >= s.Min && x.MXP <= s.Max).ToList();
                var A_resCount = A_panels.Count();
                var B_resCount = B_panels.Count();

                #region Update DropDown
                if (item.Text.IndexOf("(") != -1)
                {
                    int start = item.Text.IndexOf("(");
                    int end = item.Text.IndexOf(")");
                    item.Text = item.Text.Remove(start - 1, end - start + 2);
                    item.Text = item.Text.Split(':')[0] + " (A = " + A_resCount + " - B = " + B_resCount + ")";
                }
                else
                {
                    item.Text = item.Text.Split(':')[0] + " (A = " + A_resCount + " - B = " + B_resCount + ")";
                }
                #endregion

            }
        }

        public void CheckForPrint()
        {
            bool forFlag = false;
            if (panelprintsvm.Count != num_MaxPanelInPack.Value)
            {
                foreach (var pts in drl_panelType.Items)
                {
                    if (forFlag)
                    {
                        break;
                    }
                    var Grades = new List<PanelConfigValue>();

                    if (pts.Text.ToLower() == "mono") { Grades = MonoConfig; }

                    if (pts.Text.ToLower() == "monolike") { Grades = MonoLikeConfig; }

                    if (pts.Text.ToLower() == "poly") { Grades = PolyConfig; }

                    foreach (var item in Grades)
                    {
                        var s = item;
                        if (s.Min != 0)
                        {
                            var A_panels = panelsvm.Where(x => x.PT.ToLower() == pts.Text.ToLower() && x.VC == "A").Where(x => x.MXP >= s.Min && x.MXP <= s.Max).ToList();
                            var B_panels = panelsvm.Where(x => x.PT.ToLower() == pts.Text.ToLower() && x.VC == "B").Where(x => x.MXP >= s.Min && x.MXP <= s.Max).ToList();
                            var A_resCount = A_panels.Count();
                            var B_resCount = B_panels.Count();

                            if (A_panels.Count() == Convert.ToInt32(num_MaxPanelInPack.Value))
                            {
                                if (panelprintsvm.Count > 0)
                                {
                                    panelsvm.AddRange(panelprintsvm);
                                    panelprintsvm.Clear();
                                }

                                panelprintsvm.AddRange(A_panels);
                                A_panels.ToList().ForEach(x => panelsvm.Remove(x));

                                today_work_grid.DataSource = null;
                                today_work_grid.DataSource = panelsvm;
                                current_pack_grid.DataSource = null;
                                panelprintsvm.Reverse();
                                current_pack_grid.DataSource = panelprintsvm;

                                MessageBox.Show("Panels was added to pack and they are ready to print.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //button2.PerformClick();
                                forFlag = true;
                                break;

                            }
                            if (B_panels.Count() == Convert.ToInt32(num_MaxPanelInPack.Value))
                            {
                                if (panelprintsvm.Count > 0)
                                {
                                    panelsvm.AddRange(panelprintsvm);
                                    panelprintsvm.Clear();
                                }

                                panelprintsvm.AddRange(B_panels);
                                B_panels.ToList().ForEach(x => panelsvm.Remove(x));

                                today_work_grid.DataSource = null;
                                today_work_grid.DataSource = panelsvm;
                                current_pack_grid.DataSource = null;

                                panelprintsvm.Reverse();
                                current_pack_grid.DataSource = panelprintsvm;

                                MessageBox.Show("Panels was added to pack and they are ready to print.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //button2.PerformClick();
                                forFlag = true;
                                break;
                            }
                        }
                    }
                }
                //----
                if (panelsvm.Count > 0)
                {
                    savetofile();
                }
            }
        }

        private void btn_CurrentGrid_sort_Click(object sender, EventArgs e)
        {
            try
            {
                var s = (PanelConfigValue)drl_grades.SelectedItem.Tag;
                today_work_grid.DataSource = panelsvm
                   .Where(x => x.MXP >= s.Min && x.MXP <= s.Max && x.PT.ToLower() == drl_panelType.Text.ToLower())
                   .OrderBy(x => DateTime.Parse(x.ExitTime));
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Please Contact to IT Department", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("لطفا با پشتیبانی نرم افزار تماس بگیرید \r\n" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
