using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsFormsApplication3.Controller;
using WindowsFormsApplication3.Common;
using WindowsFormsApplication3.Resources;

namespace WindowsFormsApplication3.View
{
    public partial class VisualSCD : Form, IMainView
    {
        /// <summary>
        /// This is the controller interface used to communicate to the controller.
        /// (Will be set by controller through IMainView.StartView. 
        /// </summary>
        private IMainViewController m_Controller = null;
        /// <summary>
        /// This is the configuration gui.
        /// </summary>
        private ConfigView m_ConfigView = new ConfigView();

        private string m_currentFile = null;
        private string m_filePath = null;

        public VisualSCD()
        {
            InitializeComponent();
        }

        void IMainView.StartView(IMainViewController control)
        {
            m_Controller = control;
            m_Controller.setMdiText(this.Text);
            Application.Run(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.showNetworkToolStripMenuItem_Click(sender, e);
        }

        private void showNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConfigView();
        }

        /// <summary>
        /// This callback will be called if the user selected the Open Configuration.
        /// </summary>
        /// <param name="sender">Standard event argument. Not used.</param>
        /// <param name="e">Standard event argument. Not used.</param>
        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (this.m_currentFile != null)
            {
                DialogOpen.InitialDirectory = this.m_filePath.Substring(0, this.m_filePath.LastIndexOf('\\'));
                DialogOpen.FileName = "";
            }
            else
            {
                DialogOpen.InitialDirectory = CommonViewRoutines.GetFilesDirectory();
            }

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                // hide all the other guis
                if (this.m_ConfigView.Visible)
                {
                    this.m_ConfigView.Close();
                    if (CheckFormIsOpen("ConfigView"))
                    {
                        return;
                    }
                    else
                    {
                        this.m_ConfigView.Dispose();
                    }
                }

                try
                {
                    m_Controller.OpenConfig(DialogOpen.FileName);
                    m_filePath = DialogOpen.FileName;
                    m_currentFile = DialogOpen.FileName.Substring(DialogOpen.FileName.LastIndexOf('\\') + 1);
                    Text = m_currentFile;
                    if (!this.m_ConfigView.Visible)
                    {
                        ShowConfigView();
                    }
                    saveAsToolStripMenuItem.Enabled = true;
                }
                catch
                {
                    MessageBox.Show(Messages.ErrorFileLoad);
                    if (!this.m_ConfigView.Visible)
                    {
                        ShowConfigView();
                    }
                }
            }
        }

        private bool CheckFormIsOpen(string Forms)
        {
            bool bResult = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == Forms)
                {
                    bResult = true;
                    break;
                }
            }
            return bResult;
        }

        /// <summary>
        /// This method checks if the configuration view has already been created,
        /// if not, the gui will be created.
        /// If the view has just been hidden, it will be shown again.
        /// </summary>
        private void ShowConfigView()
        {
            if (m_ConfigView.IsDisposed)
            {
                m_ConfigView = new ConfigView();
            }
            if (!m_ConfigView.Visible)
            {
                m_ConfigView.MdiParent = this;
                m_ConfigView.WindowState = FormWindowState.Maximized;
                m_ConfigView.Show();
            }
            this.m_Controller.SetConfigView(m_ConfigView);
        }

        private void MainMenu_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (e.Item.Text.Length == 0)
            {
                e.Item.Visible = false;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_ConfigView.Validate();

            DateTime dt = DateTime.Now;
            string str = string.Format("{0:yyyyMMdd}", dt);
            string fileName = m_currentFile.Substring(0, m_currentFile.IndexOf('.')) + str;
            string oldFilePath = DialogOpen.FileName;
            m_Controller.SaveCfgFile(fileName, oldFilePath);
        }
    }
}
