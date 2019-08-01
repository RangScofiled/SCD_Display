namespace WindowsFormsApplication3.View
{
    partial class ConfigView
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
            this.components = new System.ComponentModel.Container();
            this.NetCfgTab = new System.Windows.Forms.TabControl();
            this.MainPage = new System.Windows.Forms.TabPage();
            this.SubNetLineTable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.DevIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubNetDevTable = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IED = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Switch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ODF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubNetPictureBox = new System.Windows.Forms.PictureBox();
            this.IEDNet = new System.Windows.Forms.TabPage();
            this.IEDOutVirLineTab = new System.Windows.Forms.DataGridView();
            this.IEDDsRelayEnaTab = new System.Windows.Forms.DataGridView();
            this.DsRelayEnaIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDInVirLineTab = new System.Windows.Forms.DataGridView();
            this.IEDVirLineIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDTreeView = new System.Windows.Forms.TreeView();
            this.IEDPicTab = new System.Windows.Forms.TabControl();
            this.IEDPortPic = new System.Windows.Forms.TabPage();
            this.IedCBAddrLbl = new System.Windows.Forms.Label();
            this.PortPicture = new System.Windows.Forms.PictureBox();
            this.IEDRelayPic = new System.Windows.Forms.TabPage();
            this.RelayPicture = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveCfgFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.IEDOutIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDOutPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDOutAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDOutDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDExtAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDExtDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IEDCB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetCfgTab.SuspendLayout();
            this.MainPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubNetLineTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubNetDevTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubNetPictureBox)).BeginInit();
            this.IEDNet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IEDOutVirLineTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IEDDsRelayEnaTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IEDInVirLineTab)).BeginInit();
            this.IEDPicTab.SuspendLayout();
            this.IEDPortPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PortPicture)).BeginInit();
            this.IEDRelayPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RelayPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // NetCfgTab
            // 
            this.NetCfgTab.Controls.Add(this.MainPage);
            this.NetCfgTab.Controls.Add(this.IEDNet);
            this.NetCfgTab.Location = new System.Drawing.Point(0, 0);
            this.NetCfgTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NetCfgTab.Name = "NetCfgTab";
            this.NetCfgTab.SelectedIndex = 0;
            this.NetCfgTab.Size = new System.Drawing.Size(1900, 1200);
            this.NetCfgTab.TabIndex = 0;
            // 
            // MainPage
            // 
            this.MainPage.Controls.Add(this.SubNetLineTable);
            this.MainPage.Controls.Add(this.dataGridView1);
            this.MainPage.Controls.Add(this.SubNetDevTable);
            this.MainPage.Controls.Add(this.SubNetPictureBox);
            this.MainPage.Location = new System.Drawing.Point(4, 25);
            this.MainPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MainPage.Name = "MainPage";
            this.MainPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MainPage.Size = new System.Drawing.Size(1892, 1171);
            this.MainPage.TabIndex = 0;
            this.MainPage.Text = "SubNetwork";
            this.MainPage.UseVisualStyleBackColor = true;
            // 
            // SubNetLineTable
            // 
            this.SubNetLineTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.SubNetLineTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.SubNetLineTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubNetLineTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Line});
            this.SubNetLineTable.Location = new System.Drawing.Point(1050, 363);
            this.SubNetLineTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SubNetLineTable.Name = "SubNetLineTable";
            this.SubNetLineTable.RowTemplate.Height = 27;
            this.SubNetLineTable.Size = new System.Drawing.Size(460, 322);
            this.SubNetLineTable.TabIndex = 1;
            this.SubNetLineTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SubNetLineTable_CellClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Index";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 76;
            // 
            // Line
            // 
            this.Line.HeaderText = "Line";
            this.Line.Name = "Line";
            this.Line.Width = 68;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DevIndex});
            this.dataGridView1.Location = new System.Drawing.Point(1533, 50);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(460, 400);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SubNetDevTable_CellClick);
            // 
            // DevIndex
            // 
            this.DevIndex.HeaderText = "Index";
            this.DevIndex.Name = "DevIndex";
            // 
            // SubNetDevTable
            // 
            this.SubNetDevTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.SubNetDevTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.SubNetDevTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubNetDevTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.IED,
            this.Switch,
            this.ODF});
            this.SubNetDevTable.Location = new System.Drawing.Point(1050, 50);
            this.SubNetDevTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SubNetDevTable.Name = "SubNetDevTable";
            this.SubNetDevTable.RowTemplate.Height = 27;
            this.SubNetDevTable.Size = new System.Drawing.Size(460, 309);
            this.SubNetDevTable.TabIndex = 1;
            this.SubNetDevTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SubNetDevTable_CellClick);
            // 
            // Index
            // 
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.Width = 76;
            // 
            // IED
            // 
            this.IED.HeaderText = "IED_Name";
            this.IED.Name = "IED";
            // 
            // Switch
            // 
            this.Switch.HeaderText = "Switch_Name";
            this.Switch.Name = "Switch";
            this.Switch.Width = 124;
            // 
            // ODF
            // 
            this.ODF.HeaderText = "ODF_Name";
            this.ODF.Name = "ODF";
            // 
            // SubNetPictureBox
            // 
            this.SubNetPictureBox.Location = new System.Drawing.Point(15, 50);
            this.SubNetPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SubNetPictureBox.Name = "SubNetPictureBox";
            this.SubNetPictureBox.Size = new System.Drawing.Size(1000, 1000);
            this.SubNetPictureBox.TabIndex = 0;
            this.SubNetPictureBox.TabStop = false;
            this.SubNetPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SubNetPictureBox_MouseClick);
            this.SubNetPictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SubNetPictureBox_MouseDoubleClick);
            // 
            // IEDNet
            // 
            this.IEDNet.Controls.Add(this.IEDOutVirLineTab);
            this.IEDNet.Controls.Add(this.IEDDsRelayEnaTab);
            this.IEDNet.Controls.Add(this.IEDInVirLineTab);
            this.IEDNet.Controls.Add(this.IEDTreeView);
            this.IEDNet.Controls.Add(this.IEDPicTab);
            this.IEDNet.Location = new System.Drawing.Point(4, 25);
            this.IEDNet.Name = "IEDNet";
            this.IEDNet.Padding = new System.Windows.Forms.Padding(3);
            this.IEDNet.Size = new System.Drawing.Size(1892, 1171);
            this.IEDNet.TabIndex = 1;
            this.IEDNet.Text = "IED Picture";
            this.IEDNet.UseVisualStyleBackColor = true;
            // 
            // IEDOutVirLineTab
            // 
            this.IEDOutVirLineTab.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IEDOutVirLineTab.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IEDOutIndex,
            this.IEDOutPort,
            this.IEDOutAddr,
            this.IEDOutDesc,
            this.IEDExtAddr,
            this.IEDExtDesc,
            this.IEDCB});
            this.IEDOutVirLineTab.Location = new System.Drawing.Point(924, 237);
            this.IEDOutVirLineTab.Name = "IEDOutVirLineTab";
            this.IEDOutVirLineTab.RowTemplate.Height = 27;
            this.IEDOutVirLineTab.Size = new System.Drawing.Size(587, 200);
            this.IEDOutVirLineTab.TabIndex = 3;
            this.IEDOutVirLineTab.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.IEDOutVirLineTab_CellClick);
            // 
            // IEDDsRelayEnaTab
            // 
            this.IEDDsRelayEnaTab.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IEDDsRelayEnaTab.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DsRelayEnaIndex});
            this.IEDDsRelayEnaTab.Location = new System.Drawing.Point(924, 443);
            this.IEDDsRelayEnaTab.Name = "IEDDsRelayEnaTab";
            this.IEDDsRelayEnaTab.RowTemplate.Height = 27;
            this.IEDDsRelayEnaTab.Size = new System.Drawing.Size(587, 209);
            this.IEDDsRelayEnaTab.TabIndex = 3;
            // 
            // DsRelayEnaIndex
            // 
            this.DsRelayEnaIndex.HeaderText = "Index";
            this.DsRelayEnaIndex.Name = "DsRelayEnaIndex";
            this.DsRelayEnaIndex.Width = 76;
            // 
            // IEDInVirLineTab
            // 
            this.IEDInVirLineTab.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IEDInVirLineTab.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IEDVirLineIndex});
            this.IEDInVirLineTab.Location = new System.Drawing.Point(924, 31);
            this.IEDInVirLineTab.Name = "IEDInVirLineTab";
            this.IEDInVirLineTab.RowTemplate.Height = 27;
            this.IEDInVirLineTab.Size = new System.Drawing.Size(587, 200);
            this.IEDInVirLineTab.TabIndex = 3;
            this.IEDInVirLineTab.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.IEDInVirLineTab_CellClick);
            this.IEDInVirLineTab.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.IEDInVirLineTab_CellParsing);
            // 
            // IEDVirLineIndex
            // 
            this.IEDVirLineIndex.HeaderText = "Index";
            this.IEDVirLineIndex.Name = "IEDVirLineIndex";
            this.IEDVirLineIndex.Width = 76;
            // 
            // IEDTreeView
            // 
            this.IEDTreeView.Location = new System.Drawing.Point(3, 6);
            this.IEDTreeView.Name = "IEDTreeView";
            this.IEDTreeView.Size = new System.Drawing.Size(190, 650);
            this.IEDTreeView.TabIndex = 2;
            this.IEDTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.IEDTreeView_AfterSelect);
            // 
            // IEDPicTab
            // 
            this.IEDPicTab.Controls.Add(this.IEDPortPic);
            this.IEDPicTab.Controls.Add(this.IEDRelayPic);
            this.IEDPicTab.Location = new System.Drawing.Point(199, 6);
            this.IEDPicTab.Name = "IEDPicTab";
            this.IEDPicTab.SelectedIndex = 0;
            this.IEDPicTab.Size = new System.Drawing.Size(719, 650);
            this.IEDPicTab.TabIndex = 1;
            // 
            // IEDPortPic
            // 
            this.IEDPortPic.Controls.Add(this.IedCBAddrLbl);
            this.IEDPortPic.Controls.Add(this.PortPicture);
            this.IEDPortPic.Location = new System.Drawing.Point(4, 25);
            this.IEDPortPic.Name = "IEDPortPic";
            this.IEDPortPic.Padding = new System.Windows.Forms.Padding(3);
            this.IEDPortPic.Size = new System.Drawing.Size(711, 621);
            this.IEDPortPic.TabIndex = 0;
            this.IEDPortPic.Text = "Port_Picture";
            this.IEDPortPic.UseVisualStyleBackColor = true;
            // 
            // IedCBAddrLbl
            // 
            this.IedCBAddrLbl.AutoSize = true;
            this.IedCBAddrLbl.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IedCBAddrLbl.Location = new System.Drawing.Point(500, 222);
            this.IedCBAddrLbl.Name = "IedCBAddrLbl";
            this.IedCBAddrLbl.Size = new System.Drawing.Size(0, 19);
            this.IedCBAddrLbl.TabIndex = 2;
            // 
            // PortPicture
            // 
            this.PortPicture.Location = new System.Drawing.Point(5, 6);
            this.PortPicture.Name = "PortPicture";
            this.PortPicture.Size = new System.Drawing.Size(700, 610);
            this.PortPicture.TabIndex = 1;
            this.PortPicture.TabStop = false;
            // 
            // IEDRelayPic
            // 
            this.IEDRelayPic.Controls.Add(this.RelayPicture);
            this.IEDRelayPic.Location = new System.Drawing.Point(4, 25);
            this.IEDRelayPic.Name = "IEDRelayPic";
            this.IEDRelayPic.Padding = new System.Windows.Forms.Padding(3);
            this.IEDRelayPic.Size = new System.Drawing.Size(711, 621);
            this.IEDRelayPic.TabIndex = 1;
            this.IEDRelayPic.Text = "Relay_Picture";
            this.IEDRelayPic.UseVisualStyleBackColor = true;
            // 
            // RelayPicture
            // 
            this.RelayPicture.Location = new System.Drawing.Point(6, 6);
            this.RelayPicture.Name = "RelayPicture";
            this.RelayPicture.Size = new System.Drawing.Size(700, 610);
            this.RelayPicture.TabIndex = 0;
            this.RelayPicture.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // IEDOutIndex
            // 
            this.IEDOutIndex.HeaderText = "Index";
            this.IEDOutIndex.Name = "IEDOutIndex";
            this.IEDOutIndex.Width = 76;
            // 
            // IEDOutPort
            // 
            this.IEDOutPort.HeaderText = "OutPort";
            this.IEDOutPort.Name = "IEDOutPort";
            // 
            // IEDOutAddr
            // 
            this.IEDOutAddr.HeaderText = "OutAddr";
            this.IEDOutAddr.Name = "IEDOutAddr";
            this.IEDOutAddr.Width = 92;
            // 
            // IEDOutDesc
            // 
            this.IEDOutDesc.HeaderText = "OutDesc";
            this.IEDOutDesc.Name = "IEDOutDesc";
            this.IEDOutDesc.Width = 92;
            // 
            // IEDExtAddr
            // 
            this.IEDExtAddr.HeaderText = "ExtAddr";
            this.IEDExtAddr.Name = "IEDExtAddr";
            this.IEDExtAddr.Width = 92;
            // 
            // IEDExtDesc
            // 
            this.IEDExtDesc.HeaderText = "ExtDesc";
            this.IEDExtDesc.Name = "IEDExtDesc";
            this.IEDExtDesc.Width = 92;
            // 
            // IEDCB
            // 
            this.IEDCB.HeaderText = "ControlBlock";
            this.IEDCB.Name = "IEDCB";
            // 
            // ConfigView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1945, 1055);
            this.Controls.Add(this.NetCfgTab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigView";
            this.Load += new System.EventHandler(this.ConfigView_Load);
            this.NetCfgTab.ResumeLayout(false);
            this.MainPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SubNetLineTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubNetDevTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubNetPictureBox)).EndInit();
            this.IEDNet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IEDOutVirLineTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IEDDsRelayEnaTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IEDInVirLineTab)).EndInit();
            this.IEDPicTab.ResumeLayout(false);
            this.IEDPortPic.ResumeLayout(false);
            this.IEDPortPic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PortPicture)).EndInit();
            this.IEDRelayPic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RelayPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl NetCfgTab;
        private System.Windows.Forms.TabPage MainPage;
        private System.Windows.Forms.PictureBox SubNetPictureBox;
        private System.Windows.Forms.DataGridView SubNetDevTable;
        private System.Windows.Forms.DataGridView SubNetLineTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DevIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn IED;
        private System.Windows.Forms.DataGridViewTextBoxColumn Switch;
        private System.Windows.Forms.DataGridViewTextBoxColumn ODF;
        private System.Windows.Forms.TabPage IEDNet;
        private System.Windows.Forms.TabControl IEDPicTab;
        private System.Windows.Forms.TabPage IEDPortPic;
        private System.Windows.Forms.PictureBox PortPicture;
        private System.Windows.Forms.TabPage IEDRelayPic;
        private System.Windows.Forms.PictureBox RelayPicture;
        private System.Windows.Forms.DataGridView IEDInVirLineTab;
        private System.Windows.Forms.TreeView IEDTreeView;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDVirLineIndex;
        private System.Windows.Forms.DataGridView IEDOutVirLineTab;
        private System.Windows.Forms.Label IedCBAddrLbl;
        private System.Windows.Forms.DataGridView IEDDsRelayEnaTab;
        private System.Windows.Forms.DataGridViewTextBoxColumn DsRelayEnaIndex;
        private System.Windows.Forms.SaveFileDialog SaveCfgFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDOutIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDOutPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDOutAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDOutDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDExtAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDExtDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn IEDCB;
    }
}