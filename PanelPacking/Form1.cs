using PanelPacking.Helpres;
using PanelPacking.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PanelPacking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string[] Config;
        private void btn_login_Click(object sender, EventArgs e)
        {
            if(in_username.Text == "")
            {
                MessageBox.Show("Please enter your username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else if(in_pass.Text == "")
            {
                MessageBox.Show("Please enter your password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                LoginHelper loginHelper = new LoginHelper();
                if (File.Exists(Environment.CurrentDirectory + "\\Config.ini"))
                {
                    Config = File.ReadAllLines(Environment.CurrentDirectory + "\\Config.ini");
                    var reuturnValue = loginHelper.Login(in_username.Text, in_pass.Text, Config[1]);
                    if(reuturnValue.IsValid)
                    {
                        in_pass.Text = "";
                        in_username.Text = "";
                        this.Hide();
                        var main = new frm_Main();
                        main.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Your username or password is incorrect.Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Application.Exit();
                }
                    
            }
            
        }
    }
}
