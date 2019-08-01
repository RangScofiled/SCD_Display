namespace WindowsFormsApplication3.View
{
    partial class VisualSCD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualSCD));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.networkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSCDFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DialogOpen = new System.Windows.Forms.OpenFileDialog();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.networkToolStripMenuItem});
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.ItemAdded += new System.Windows.Forms.ToolStripItemEventHandler(this.MainMenu_ItemAdded);
            // 
            // networkToolStripMenuItem
            // 
            this.networkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showNetworkToolStripMenuItem,
            this.loadSCDFileToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.networkToolStripMenuItem.Name = "networkToolStripMenuItem";
            resources.ApplyResources(this.networkToolStripMenuItem, "networkToolStripMenuItem");
            // 
            // showNetworkToolStripMenuItem
            // 
            this.showNetworkToolStripMenuItem.Name = "showNetworkToolStripMenuItem";
            resources.ApplyResources(this.showNetworkToolStripMenuItem, "showNetworkToolStripMenuItem");
            this.showNetworkToolStripMenuItem.Click += new System.EventHandler(this.showNetworkToolStripMenuItem_Click);
            // 
            // loadSCDFileToolStripMenuItem
            // 
            this.loadSCDFileToolStripMenuItem.Name = "loadSCDFileToolStripMenuItem";
            resources.ApplyResources(this.loadSCDFileToolStripMenuItem, "loadSCDFileToolStripMenuItem");
            this.loadSCDFileToolStripMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // DialogOpen
            // 
            resources.ApplyResources(this.DialogOpen, "DialogOpen");
            // 
            // VisualSCD
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.MainMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "VisualSCD";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSCDFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showNetworkToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog DialogOpen;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    }
}

