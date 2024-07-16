namespace PanelPacking.Views
{
    partial class frm_edit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.office2010BlueTheme1 = new Telerik.WinControls.Themes.Office2010BlueTheme();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.btn_edit_row = new Telerik.WinControls.UI.RadButton();
            this.SNedit_input = new System.Windows.Forms.NumericUpDown();
            this.mxpEdit_input = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_edit_row)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SNedit_input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mxpEdit_input)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView1
            // 
            this.radGridView1.AutoScroll = true;
            this.radGridView1.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radGridView1.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnChooser = false;
            this.radGridView1.MasterTemplate.AllowColumnReorder = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowDragToGroup = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.AllowReorder = false;
            gridViewTextBoxColumn1.AllowSort = false;
            gridViewTextBoxColumn1.DataType = typeof(int);
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.Expression = "";
            gridViewTextBoxColumn1.FieldName = "MXP";
            gridViewTextBoxColumn1.HeaderText = "Max Power";
            gridViewTextBoxColumn1.Name = "MXP";
            gridViewTextBoxColumn1.Width = 75;
            gridViewTextBoxColumn2.AllowGroup = false;
            gridViewTextBoxColumn2.AllowReorder = false;
            gridViewTextBoxColumn2.AllowSort = false;
            gridViewTextBoxColumn2.FieldName = "SN";
            gridViewTextBoxColumn2.HeaderText = "Serial Number";
            gridViewTextBoxColumn2.Name = "SN";
            gridViewTextBoxColumn3.AllowGroup = false;
            gridViewTextBoxColumn3.AllowReorder = false;
            gridViewTextBoxColumn3.AllowSort = false;
            gridViewTextBoxColumn3.FieldName = "VC";
            gridViewTextBoxColumn3.HeaderText = "VC";
            gridViewTextBoxColumn3.Name = "VC";
            gridViewTextBoxColumn4.AllowGroup = false;
            gridViewTextBoxColumn4.AllowReorder = false;
            gridViewTextBoxColumn4.AllowSort = false;
            gridViewTextBoxColumn4.FieldName = "PT";
            gridViewTextBoxColumn4.HeaderText = "Panel Type";
            gridViewTextBoxColumn4.Name = "PT";
            gridViewTextBoxColumn5.AllowGroup = false;
            gridViewTextBoxColumn5.AllowReorder = false;
            gridViewTextBoxColumn5.AllowSort = false;
            gridViewTextBoxColumn5.FieldName = "ExitTime";
            gridViewTextBoxColumn5.HeaderText = "ExitTime";
            gridViewTextBoxColumn5.Name = "ExitTime";
            this.radGridView1.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.radGridView1.MasterTemplate.EnableFiltering = true;
            this.radGridView1.MasterTemplate.ShowFilteringRow = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ReadOnly = true;
            this.radGridView1.Size = new System.Drawing.Size(477, 528);
            this.radGridView1.TabIndex = 3;
            this.radGridView1.ThemeName = "Office2010Blue";
            this.radGridView1.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.grid_cell_click);
            // 
            // radButton1
            // 
            this.radButton1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton1.Location = new System.Drawing.Point(202, 546);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(287, 24);
            this.radButton1.TabIndex = 4;
            this.radButton1.Text = "Save Changes";
            this.radButton1.ThemeName = "Office2010Blue";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radButton2
            // 
            this.radButton2.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton2.Location = new System.Drawing.Point(12, 546);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(89, 24);
            this.radButton2.TabIndex = 5;
            this.radButton2.Text = "Cancel";
            this.radButton2.ThemeName = "Office2010Blue";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton3
            // 
            this.radButton3.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton3.Location = new System.Drawing.Point(107, 546);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(89, 24);
            this.radButton3.TabIndex = 6;
            this.radButton3.Text = "Reverse";
            this.radButton3.ThemeName = "Office2010Blue";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.btn_edit_row);
            this.radGroupBox1.Controls.Add(this.SNedit_input);
            this.radGroupBox1.Controls.Add(this.mxpEdit_input);
            this.radGroupBox1.Controls.Add(this.label2);
            this.radGroupBox1.Controls.Add(this.label1);
            this.radGroupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radGroupBox1.HeaderText = "Edit Information";
            this.radGroupBox1.Location = new System.Drawing.Point(496, 12);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(308, 229);
            this.radGroupBox1.TabIndex = 7;
            this.radGroupBox1.Text = "Edit Information";
            this.radGroupBox1.ThemeName = "Office2010Blue";
            // 
            // btn_edit_row
            // 
            this.btn_edit_row.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_edit_row.Location = new System.Drawing.Point(9, 185);
            this.btn_edit_row.Name = "btn_edit_row";
            this.btn_edit_row.Size = new System.Drawing.Size(294, 24);
            this.btn_edit_row.TabIndex = 6;
            this.btn_edit_row.Text = "Update";
            this.btn_edit_row.ThemeName = "Office2010Blue";
            this.btn_edit_row.Click += new System.EventHandler(this.btn_edit_row_Click);
            // 
            // SNedit_input
            // 
            this.SNedit_input.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SNedit_input.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SNedit_input.Location = new System.Drawing.Point(9, 129);
            this.SNedit_input.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.SNedit_input.Name = "SNedit_input";
            this.SNedit_input.Size = new System.Drawing.Size(294, 29);
            this.SNedit_input.TabIndex = 5;
            this.SNedit_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mxpEdit_input
            // 
            this.mxpEdit_input.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mxpEdit_input.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.mxpEdit_input.Location = new System.Drawing.Point(9, 62);
            this.mxpEdit_input.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mxpEdit_input.Name = "mxpEdit_input";
            this.mxpEdit_input.Size = new System.Drawing.Size(294, 29);
            this.mxpEdit_input.TabIndex = 4;
            this.mxpEdit_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Serial Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "MXP";
            // 
            // frm_edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 580);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frm_edit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Packs";
            this.Load += new System.EventHandler(this.frm_edit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_edit_row)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SNedit_input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mxpEdit_input)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.Themes.Office2010BlueTheme office2010BlueTheme1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Telerik.WinControls.UI.RadButton btn_edit_row;
        private System.Windows.Forms.NumericUpDown SNedit_input;
        private System.Windows.Forms.NumericUpDown mxpEdit_input;
    }
}