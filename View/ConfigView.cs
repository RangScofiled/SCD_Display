using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using WindowsFormsApplication3.Controller;
using WindowsFormsApplication3.Model;
using WindowsFormsApplication3.Common;
using System.IO;

namespace WindowsFormsApplication3.View
{
    struct LinePos
    {
        public Point startP;
        public Point endP;
        public string name;
    }
    struct DevRect
    {
        public Point startP;
        //public int width;
        //public int height;
        public string name;
    }

    public partial class ConfigView : Form, IConfigView
    {
        private const string V = "Switch";
        private const int IedW = 30;
        private const int IedH = 30;
        private const int SwitchW = 60;
        private const int SwitchH = 20;
        private const int OdfW = 20;
        private const int OdfH = 60;

        private Button SubNetBtn = null;
        //private TabPage SubNetPage = null;
        //private TreeView[] IedTree = null;
        //private PictureBox[] IedPicture = null;
        //private DataGridView[] SubNetVirLines = null;

        //private int m_PagesCount = 0;

        Bitmap[] m_DevBmp = null;
        Bitmap[][] m_IedBmp = null;

        private IConfigController m_configControl = null;
        private SubNetValues m_SubNet = null;
        private AccValues m_AccData = null;
        private DevRect[][] m_IEDPoint = null;
        private DevRect[][] m_SwitchPoint = null;
        private DevRect[][] m_ODFPoint = null;
        private LinePos[][] m_SubLine = null;

        private int m_IEDCount = 0;
        private int m_SwitchCount = 0;
        private int m_ODFCount = 0;
        private int m_LineCount = 0;

        private DevRect[][][] m_IedPortRec = null;

        private LinePos[][][] m_IedLine = null;


        public ConfigView()
        {
            InitializeComponent();

            m_DevBmp = new Bitmap[6];
            m_IedBmp = new Bitmap[6][];
            m_IEDPoint = new DevRect[6][];
            m_SwitchPoint = new DevRect[6][];
            m_ODFPoint = new DevRect[6][];
            m_SubLine = new LinePos[6][];

            //IedTree = new TreeView[6];
            //IedPicture = new PictureBox[6];
            //SubNetVirLines = new DataGridView[6];
            m_IedPortRec = new DevRect[6][][];
            m_IedLine = new LinePos[6][][];

            for (int i = 0; i < 6; i++)
            {
                m_IedBmp[i] = new Bitmap[100];
                m_IEDPoint[i] = new DevRect[100];
                m_SwitchPoint[i] = new DevRect[100];
                m_ODFPoint[i] = new DevRect[100];
                m_SubLine[i] = new LinePos[100];

                m_IedPortRec[i] = new DevRect[100][];
                m_IedLine[i] = new LinePos[100][];

                for (int j = 0; j < 100; j++)
                {
                    m_IedPortRec[i][j] = new DevRect[10];
                    m_IedLine[i][j] = new LinePos[20];
                }
            }

        }

        /// <summary>
        /// This method sets the controller interface, which should be used for communication.
        /// </summary>
        /// <param name="controller">the controller object</param>
        void IConfigView.SetControllerInterface(IConfigController controller)
        {
            m_configControl = controller;

            ConnectControllers(m_configControl.GetCommData(), m_configControl.GetIedData());

        }

        void IConfigView.SaveCfgFile(string fileName, string oldFilePath)
        {
            if (SaveCfgFileDialog.InitialDirectory == null)
            {
                SaveCfgFileDialog.InitialDirectory = CommonViewRoutines.GetFilesDirectory();
            }
            SaveCfgFileDialog.RestoreDirectory = true;
            SaveCfgFileDialog.FileName = fileName;
            SaveCfgFileDialog.Filter = "|*.scd";

            fileName += ".scd";
            DialogResult dg = SaveCfgFileDialog.ShowDialog();
            if (dg == DialogResult.OK)
            {
                try
                {
                    File.Copy(oldFilePath, fileName);
                    
                }
                catch { }
                string newFilePath = oldFilePath.Substring(0, oldFilePath.LastIndexOf('\\') + 1) + fileName;

                m_configControl.SaveCfgFile(fileName, newFilePath);
            }

        }
        /// <summary>
		/// Connect data to GUI controls.
		/// </summary>
        /// <param name="commData"></param>
        private void ConnectControllers(CommValues commData, IedValues iedData)
        {
            m_SubNet = commData.GetSubNetValues();
            m_AccData = iedData.GetAccValues();
            IEDTreeView.Nodes.Clear();
            try
            {
                for (int i = 0; i < 6; i++)
                {

                    if (m_SubNet.SubNetData[i].SubNetName != "")//m_SubNet.SubNetName[i]
                    {
                        AddSubNetGroup(i);
                        m_IEDCount = 0;
                        m_SwitchCount = 0;
                        m_ODFCount = 0;
                        m_LineCount = 0;
                        for (int j = 0; j < 100; j++)
                        {
                            if (m_SubNet.SubNetData[i].IEDName[j] != "")//m_SubNet.IedName[i, j]
                            {
                                FillIedName(i, j);
                                for (int k = 0; k < 10; k++)
                                {
                                    if (m_SubNet.SubNetData[i].Phys[j, k].Count != 0)//m_SubNet.PhysNodes[i, j, k]
                                    {
                                        FillSwitchName(i, j, k);
                                        FillLinesList(i, j, k);
                                    }
                                }
                            }
                        }
                        SetDevGraphData(i);
                        DrawDevImage(i);
                        DrawIedPortImage(i);

                    }
                }
            }
            catch { }
        }

        private void ConfigView_Load(object sender, EventArgs e)
        {
            SubNetDevTable.Rows.Add(100);
            SubNetLineTable.Rows.Add(100);
        }

        private void SubNetBtn_Click(object sender, EventArgs e)
        {
            int str = Convert.ToInt16(((Button)sender).Name.Substring(9));
            SubNetDevTable.Name = "SubNetDevTable" + str;
            SubNetLineTable.Name = "SubNetLineTable" + str;
            SubNetPictureBox.Name = "SubNetPictureBox" + str;
            SubNetDevTable.Rows.Clear();
            SubNetLineTable.Rows.Clear();
            SubNetDevTable.Rows.Add(100);
            SubNetLineTable.Rows.Add(100);
            for (int i = 0; i < 100; i++)
            {
                if (!string.IsNullOrEmpty(m_IEDPoint[str][i].name))
                {
                    SubNetDevTable.Rows[i].Cells[0].Value = i;
                    SubNetDevTable.Rows[i].Cells[1].Value = m_IEDPoint[str][i].name;
                }

                if (m_SwitchPoint[str][i].name != null)
                    SubNetDevTable.Rows[i].Cells[2].Value = m_SwitchPoint[str][i].name;

                if (m_ODFPoint[str][i].name != null)
                    SubNetDevTable.Rows[i].Cells[3].Value = m_ODFPoint[str][i].name;

                if (m_SubLine[str][i].name != null)
                {
                    SubNetLineTable.Rows[i].Cells[0].Value = i;
                    SubNetLineTable.Rows[i].Cells[1].Value = m_SubLine[str][i].name;
                }
            }
           
            this.SubNetPictureBox.Image = m_DevBmp[str];

            dataGridView1.DataSource = m_SubNet.SubNetDevList;
            for (int i = 0; i < dataGridView1.RowCount; i++)
                dataGridView1.Rows[i].Cells[0].Value = i;
        }

        private void AddSubNetGroup(int subIdx)
        {
            SubNetBtn = new Button
            {
                Location = new Point(5 + 155 * subIdx, 6),
                Size = new Size(150, 30),
                Text = m_SubNet.SubNetData[subIdx].SubNetName,//m_SubNet.SubNetName[subIdx]
                Name = "SubNetBtn" + subIdx
            };
            MainPage.Controls.Add(SubNetBtn);
            SubNetBtn.Click += new EventHandler(this.SubNetBtn_Click);
        }

        //private void IEDTreeView_MouseClick(object sender, MouseEventArgs e)
        //{
        //    Point ClickPoint = new Point(e.X, e.Y);
        //    TreeNode CurrentNode = ((TreeView)sender).GetNodeAt(ClickPoint);
        //    for (int idx = 0; idx < 6; idx++)
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {
        //            if (CurrentNode != null && m_IEDPoint[idx][i].name != null
        //                && CurrentNode.Text == m_IEDPoint[idx][i].name)
        //            {
        //                PortPicture.Image = m_IedBmp[idx][i];

        //                ShowIEDVirLineTable(m_IEDPoint[idx][i].name);

        //                return;
        //            }
        //        }
        //    }
            
        //}

        private void IEDTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            IedCBAddrLbl.Text = "";

            TreeNode CurrentNode = IEDTreeView.SelectedNode;
            for (int idx = 0; idx < 6; idx++)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (CurrentNode != null && m_IEDPoint[idx][i].name != null
                        && CurrentNode.Text == m_IEDPoint[idx][i].name)
                    {
                        PortPicture.Image = m_IedBmp[idx][i];

                        ShowIEDVirLineTable(m_IEDPoint[idx][i].name);

                        ShowIEDDsRelayEnaTable(m_IEDPoint[idx][i].name);

                        return;
                    }
                }
            }
        }

        private void ShowIEDVirLineTable(string iedName)
        {
            List<VirLine> InLineList = m_AccData.Virlines.GetRange(0, m_AccData.Virlines.Count); // copy list
            List<VirLine> OutLineList = m_AccData.Virlines.GetRange(0, m_AccData.Virlines.Count);

            foreach (VirLine line in m_AccData.Virlines)
            {
                if (!line.InDesc.Contains(iedName))
                {
                    InLineList.Remove(line);
                }
                if (!line.ExtDesc.Contains(iedName))
                {
                    OutLineList.Remove(line);
                }
            }
            IEDInVirLineTab.DataSource = InLineList;
            for (int i = 0; i < InLineList.Count; i++)
            {
                IEDInVirLineTab.Rows[i].Cells[0].Value = i;
            }

            //IEDOutVirLineTab.DataSource = OutLineList;
            IEDOutVirLineTab.Rows.Clear();
            for (int i = 0; i < OutLineList.Count; i++)
            {
                IEDOutVirLineTab.Rows.Add();
                IEDOutVirLineTab.Rows[i].Cells[0].Value = i;
                IEDOutVirLineTab.Rows[i].Cells["IEDOutPort"].Value = OutLineList[i].ExtPort;
                IEDOutVirLineTab.Rows[i].Cells["IEDOutAddr"].Value = OutLineList[i].ExtAddr;
                IEDOutVirLineTab.Rows[i].Cells["IEDOutDesc"].Value = OutLineList[i].ExtDesc;
                IEDOutVirLineTab.Rows[i].Cells["IEDExtAddr"].Value = OutLineList[i].IntAddr;
                IEDOutVirLineTab.Rows[i].Cells["IEDExtDesc"].Value = OutLineList[i].InDesc;
                IEDOutVirLineTab.Rows[i].Cells["IEDCB"].Value = OutLineList[i].ExtCB;
            }
        }

        private void ShowIEDDsRelayEnaTable(string iedName)
        {
            List<DsRelayEna> dsRelayEnaList = m_AccData.DsRelayEnas.GetRange(0, m_AccData.DsRelayEnas.Count);

            foreach (DsRelayEna dsRelayEna in m_AccData.DsRelayEnas)
            {
                if (!dsRelayEna.DsName.Contains(iedName))
                {
                    dsRelayEnaList.Remove(dsRelayEna);
                }
            }
            IEDDsRelayEnaTab.DataSource = dsRelayEnaList;
            for (int i = 0; i < dsRelayEnaList.Count; i++)
            {
                IEDDsRelayEnaTab.Rows[i].Cells[0].Value = i;
            }
        }

        private void FillIedName(int subIdx, int connIdx)
        {
            for (int i = 0; i < m_IEDCount; i++)
            {
                if (m_IEDPoint[subIdx][i].name == m_SubNet.SubNetData[subIdx].IEDName[connIdx])//m_SubNet.IedName[subIdx, connIdx]
                {
                    return;
                }
            }
            m_IEDPoint[subIdx][m_IEDCount].name = m_SubNet.SubNetData[subIdx].IEDName[connIdx];//m_SubNet.IedName[subIdx, connIdx]
            m_IEDCount++;

            TreeNode IedNode = new TreeNode(m_SubNet.SubNetData[subIdx].IEDName[connIdx]);//m_SubNet.IedName[subIdx, connIdx]
            IEDTreeView.Nodes.Add(IedNode);
        }

        private void FillSwitchName(int subIdx, int connIdx, int physIdx)
        {
            foreach (KeyValuePair<string, string> physNode in m_SubNet.SubNetData[subIdx].Phys[connIdx,physIdx])//m_SubNet.PhysNodes[subIdx, connIdx, physIdx]
            {
                if (physNode.Key == "Cable")
                {
                    for (int i = 0; i < m_SwitchCount; i++)
                    {
                        if (m_SwitchPoint[subIdx][i].name == physNode.Value.Substring(0, physNode.Value.IndexOf(":")))
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < m_ODFCount; i++)
                    {
                        if (m_ODFPoint[subIdx][i].name == physNode.Value.Substring(0, physNode.Value.IndexOf(":")))
                        {
                            return;
                        }
                    }
                    if (string.Equals(physNode.Value.Substring(0, 6), V))
                    {
                        m_SwitchPoint[subIdx][m_SwitchCount].name = physNode.Value.Substring(0, physNode.Value.IndexOf(":"));
                        //SubNetDevTable.Rows[m_SwitchCount].Cells[2].Value = physNode.Value;
                        m_SwitchCount++;
                    }
                    else if (string.Equals(physNode.Value.Substring(0, 3), "ODF"))
                    {
                        m_ODFPoint[subIdx][m_ODFCount].name = physNode.Value.Substring(0, physNode.Value.IndexOf(":"));
                        //SubNetDevTable.Rows[m_ODFCount].Cells[3].Value = m_ODFPoint[subIdx][m_ODFCount].name;
                        m_ODFCount++;
                    }
                }
            }
        }

        private void FillLinesList(int subIdx, int connIdx, int physIdx)
        {
            string strIed = null;
            string strSwitch = null;
            foreach (KeyValuePair<string, string> physNode in m_SubNet.SubNetData[subIdx].Phys[connIdx, physIdx])//m_SubNet.PhysNodes[subIdx, connIdx, physIdx]
            {
                if (physNode.Key == "Port")
                {
                    strIed =  m_SubNet.SubNetData[subIdx].IEDName[connIdx] + ":" + physNode.Value;//m_SubNet.IedName[subIdx, connIdx]
                }
                if (physNode.Key == "Cable")
                {
                    strSwitch = physNode.Value;
                }
            }

            for (int i = 0; i < m_LineCount; i++)
            {
                if (strIed == m_SubLine[subIdx][i].name.Substring(m_SubLine[subIdx][i].name.LastIndexOf(" ") + 1))// strIed == strSwitch
                {
                    return;
                }
            }

            //m_LineCount = SubNetLineTable.Rows.Add();
            m_SubLine[subIdx][m_LineCount].name = strIed + " <-> " + strSwitch;
            //SubNetLineTable.Rows[m_LineCount].Cells[0].Value = m_LineCount;
            //SubNetLineTable.Rows[m_LineCount].Cells[1].Value = m_SubLine[subIdx][m_LineCount].name;
            m_LineCount++;
        }

        /// <summary>
        /// Draw SubNetwork image.
        /// </summary>
        private void DrawDevImage(int subIdx)
        {
            Graphics Graph;
            Pen BlackPen;

            SolidBrush DrawBrush = new SolidBrush(Color.Black);
            m_DevBmp[subIdx] = new Bitmap(1000, 1000);
            BlackPen = new Pen(Color.Black, 1.0f);
            BlackPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            Graph = Graphics.FromImage(m_DevBmp[subIdx]);

            StringFormat stringFormat = new StringFormat
            {
                FormatFlags = StringFormatFlags.NoWrap,
                Alignment = StringAlignment.Center
            };
            Font DrawFont = new Font("Times New Roman", 14);

            // draw title
            Graph.DrawString("SubNetwork" + subIdx, DrawFont, DrawBrush, 5, 5);
            DrawFont.Dispose();

            DrawFont = new Font("Times New Roman", 10);
            // Draw ied circles
            for (int i = 0; i < m_IEDCount; i++)
            {
                Graph.DrawEllipse(BlackPen, m_IEDPoint[subIdx][i].startP.X, m_IEDPoint[subIdx][i].startP.Y, IedW, IedH);
                Graph.DrawString("I" + i, DrawFont, DrawBrush, m_IEDPoint[subIdx][i].startP.X + 15, m_IEDPoint[subIdx][i].startP.Y + 8, stringFormat);
            }
            // Draw switch rectangles
            for (int i = 0; i < m_SwitchCount; i++)
            {
                Graph.DrawRectangle(BlackPen, m_SwitchPoint[subIdx][i].startP.X, m_SwitchPoint[subIdx][i].startP.Y, SwitchW, SwitchH);
                Graph.DrawString("S" + i, DrawFont, DrawBrush, m_SwitchPoint[subIdx][i].startP.X + 30, m_SwitchPoint[subIdx][i].startP.Y + 2, stringFormat);
            }
            // Draw ODF rectangles
            for (int i = 0; i < m_ODFCount; i++)
            {
                Graph.DrawRectangle(BlackPen, m_ODFPoint[subIdx][i].startP.X, m_ODFPoint[subIdx][i].startP.Y, OdfW, OdfH);
                Graph.DrawString("O" + i, DrawFont, DrawBrush, m_ODFPoint[subIdx][i].startP.X + 10, m_ODFPoint[subIdx][i].startP.Y + 22, stringFormat);
            }
            // Draw connect lines 
            for (int i = 0; i < m_LineCount; i++)
            {
                Graph.DrawLine(BlackPen, m_SubLine[subIdx][i].startP, m_SubLine[subIdx][i].endP);
                Graph.DrawString("L" + i, DrawFont, DrawBrush, (m_SubLine[subIdx][i].startP.X + m_SubLine[subIdx][i].endP.X)/2, 
                    (m_SubLine[subIdx][i].startP.Y + m_SubLine[subIdx][i].endP.Y) / 2 - 5, stringFormat);
            }

            Graph.Save();
            SubNetPictureBox.Image = m_DevBmp[0];
            BlackPen.Dispose();
            Graph.Dispose();
        }

        private void DrawIedPortImage(int subIdx)
        {
            Graphics Graph;
            Pen BlackPen;
            SolidBrush DrawBrush = new SolidBrush(Color.Black);
            BlackPen = new Pen(Color.Black, 1.0f)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Solid
            };
            StringFormat stringFormat = new StringFormat
            {
                FormatFlags = StringFormatFlags.NoWrap,
                Alignment = StringAlignment.Center
            };

            for (int idx = 0; idx < m_IEDCount; idx++)
            {
                m_IedBmp[subIdx][idx] = new Bitmap(800, 800);

                Graph = Graphics.FromImage(m_IedBmp[subIdx][idx]);

                Font DrawFont = new Font("Times New Roman", 14);

                // Draw ied rectangle
                Graph.DrawRectangle(BlackPen, 50, 50, 600, 30);
                Graph.DrawString(m_IEDPoint[subIdx][idx].name, DrawFont, DrawBrush, 350, 25, stringFormat);

                DrawFont.Dispose();

                // Draw connected rectangle
                DrawFont = new Font("Times New Roman", 10);
                for (int i = 0; i < 10; i++)
                {
                    if (m_IedPortRec[subIdx][idx][i].name != null)
                    {
                        Graph.DrawRectangle(BlackPen, m_IedPortRec[subIdx][idx][i].startP.X, m_IedPortRec[subIdx][idx][i].startP.Y, 60, 30);
                        Graph.DrawString(m_IedPortRec[subIdx][idx][i].name, DrawFont, DrawBrush, m_IedPortRec[subIdx][idx][i].startP.X + 30, m_IedPortRec[subIdx][idx][i].startP.Y + 35, stringFormat);
                    }
                }
                BlackPen.Dispose();
                DrawFont.Dispose();
                // Draw ied ports lines
                BlackPen = new Pen(Color.Black, 2.0f)
                {
                    DashStyle = DashStyle.Solid
                };
                AdjustableArrowCap aac = new AdjustableArrowCap(8, 12);
                BlackPen.CustomStartCap = aac;
                BlackPen.CustomEndCap = aac;
                DrawFont = new Font("Times New Roman", 8);

                for (int i = 0; i < 20; i++)
                {
                    if (m_IedLine[subIdx][idx][i].name != null)
                    {
                        int idx0 = m_IedLine[subIdx][idx][i].name.IndexOf(":");
                        string str0 = m_IedLine[subIdx][idx][i].name.Substring(idx0 + 1);
                        int idx1 = str0.IndexOf(':') + 1;
                        int idx2 = str0.IndexOf(" ");
                        int idx3 = str0.LastIndexOf(":");
                        // port num
                        string str1 = str0.Substring(idx1, idx2 - idx1); 
                        string str2 = str0.Substring(idx3 + 1);
                        Graph.DrawLine(BlackPen, m_IedLine[subIdx][idx][i].startP, m_IedLine[subIdx][idx][i].endP);
                        Graph.DrawString(str1, DrawFont, DrawBrush, m_IedLine[subIdx][idx][i].startP.X, m_IedLine[subIdx][idx][i].startP.Y - 25, stringFormat);
                        Graph.DrawString(str2, DrawFont, DrawBrush, m_IedLine[subIdx][idx][i].endP.X, m_IedLine[subIdx][idx][i].endP.Y + 5, stringFormat);
                    }
                }

                Graph.Save();
                Graph.Dispose();
            }
            PortPicture.Image = m_IedBmp[subIdx][0];
            BlackPen.Dispose();
        }

        private void DrawIedRelayImage(int subIdx)
        {
        }

        /// <summary>
        /// 
        /// 一个子网构建一个同心圆，交换机在内圆，IED在外圆，若有配线柜的话，再添加一个圆，并且它们均处在最外圆。
        /// 第一层主网络：IED设备、交换机、配线柜之间的连线；旁边设置一个表格，列出它们的设备名称；设置点击IED图形事件，可进入相应的IED设备界面。
        /// 第二层IED光纤链路图：以该IED设备为中心，绘制与其他相关IED设备、交换机、配线柜的链路图，包括端口号。
        /// 第二层IED虚回路图：以该IED设备为中心，绘制与其他相关IED设备的虚回路图，包括软压板；旁边设置一个表格，列出每条回路的数据信息。
        /// 将第二层图形都存储到Bitmap[]数组中，可利用listbox进行导航，实现IED试图的切换。
        /// </summary>
        /// <param name="IedCount"></param>

        private void SetDevGraphData(int subIdx)
        {
            // set IED points
            for (int i = 0; i < m_IEDCount; i++)
            {
                m_IEDPoint[subIdx][i].startP.X = 475 + (int)(470 * Math.Cos(i * 3.14 * 2 / m_IEDCount));// R = 350, r = 25, yuanxin = (500, 350)
                m_IEDPoint[subIdx][i].startP.Y = 325 - (int)(325 * Math.Sin(i * 3.14 * 2 / m_IEDCount));
                //m_IEDPoint[subIdx][i].width = 50;
                //m_IEDPoint[subIdx][i].height = 50;
            }
            // set switch points
            for (int i = 0; i < m_SwitchCount; i++)
            {
                m_SwitchPoint[subIdx][i].startP.X = 470 + (int)(100 * Math.Cos(i * 3.14 * 2 / m_SwitchCount));// R = 100, w = 60, h = 20, yuanxin = (500, 350)
                m_SwitchPoint[subIdx][i].startP.Y = 340 - (int)(100 * Math.Sin(i * 3.14 * 2 / m_SwitchCount));
                //m_SwitchPoint[subIdx][i].width = 60;
                //m_SwitchPoint[subIdx][i].height = 20;
            }
            // set ODF points
            for (int i = 0; i < m_ODFCount; i++)
            {
                m_ODFPoint[subIdx][i].startP.X = 490 + (int)(220 * Math.Cos(i * 3.14 * 2 / m_ODFCount));// R = 220, w = 20, h = 60, yuanxin = (500, 350)
                m_ODFPoint[subIdx][i].startP.Y = 320 - (int)(220 * Math.Sin(i * 3.14 * 2 / m_ODFCount));
                //m_ODFPoint[subIdx][i].width = 20;
                //m_ODFPoint[subIdx][i].height = 60;
            }
            // set Line points
            for (int i = 0; i < m_LineCount; i++)
            {
                int index0 = m_SubLine[subIdx][i].name.IndexOf(":");
                int index1 = m_SubLine[subIdx][i].name.LastIndexOf(" ") + 1;
                int index2 = m_SubLine[subIdx][i].name.LastIndexOf(":");
                for (int j = 0; j < m_IEDCount; j++)
                {
                    if (m_SubLine[subIdx][i].name.Substring(0, index0) == m_IEDPoint[subIdx][j].name)
                    {                   
                        m_SubLine[subIdx][i].startP.X = m_IEDPoint[subIdx][j].startP.X + IedW / 2;// yuanxin
                        m_SubLine[subIdx][i].startP.Y = m_IEDPoint[subIdx][j].startP.Y + IedH / 2;
                        break;
                    }
                }
                for (int j = 0; j < m_IEDCount; j++)
                {
                    if (m_SubLine[subIdx][i].name.Substring(index1, index2 - index1) == m_IEDPoint[subIdx][j].name)
                    {
                        m_SubLine[subIdx][i].endP.X = m_IEDPoint[subIdx][j].startP.X + IedW / 2;// yuanxin
                        m_SubLine[subIdx][i].endP.Y = m_IEDPoint[subIdx][j].startP.Y + IedH / 2;
                        break;
                    }
                }
                for (int j = 0; j < m_SwitchCount; j++)
                {
                    if (m_SubLine[subIdx][i].name.Substring(index1, index2 - index1) == m_SwitchPoint[subIdx][j].name)
                    {
                        m_SubLine[subIdx][i].endP.X = m_SwitchPoint[subIdx][j].startP.X + SwitchW / 2;// zhongxin
                        m_SubLine[subIdx][i].endP.Y = m_SwitchPoint[subIdx][j].startP.Y + SwitchH / 2;
                        break;
                    }
                }
                for (int j = 0; j < m_ODFCount; j++)
                {
                    if (m_SubLine[subIdx][i].name.Substring(index1, index2 - index1) == m_ODFPoint[subIdx][j].name)
                    {                   
                        m_SubLine[subIdx][i].endP.X = m_ODFPoint[subIdx][j].startP.X + OdfW / 2;// zhongxin
                        m_SubLine[subIdx][i].endP.Y = m_ODFPoint[subIdx][j].startP.Y + OdfH / 2;
                        break;
                    }
                }

            }

            // 1 set iedport rectangle
            //for (int idx = 0; idx < m_IEDCount; idx++)
            //{
            //    int count = 0;
            //    for (int i = 0; i < m_LineCount; i++)
            //    {
            //        if (m_SubLine[subIdx][i].name != null)
            //        {
            //            int idx0 = m_SubLine[subIdx][i].name.IndexOf(":");
            //            int idx1 = m_SubLine[subIdx][i].name.LastIndexOf(" ") + 1;
            //            int idx2 = m_SubLine[subIdx][i].name.LastIndexOf(":");
            //            if (m_SubLine[subIdx][i].name.Substring(0, idx0) == m_IEDPoint[subIdx][idx].name)
            //            {
            //                for (int j = 0; j < count; j++)
            //                {
            //                    if (m_IedPortRec[subIdx][idx][j].name == m_SubLine[subIdx][i].name.Substring(idx1, idx2 - idx1))
            //                    {
            //                        goto async;
            //                    }
                                
            //                }
            //                m_IedPortRec[subIdx][idx][count].startP.Y = 500;
            //                m_IedPortRec[subIdx][idx][count].name = m_SubLine[subIdx][i].name.Substring(idx1, idx2 - idx1);

            //                count++;
            //            }
            //            else if (m_SubLine[subIdx][i].name.Substring(idx1, idx2 - idx1) == m_IEDPoint[subIdx][idx].name)
            //            {
            //                for (int j = 0; j < count; j++)
            //                {
            //                    if (m_IedPortRec[subIdx][idx][j].name == m_SubLine[subIdx][i].name.Substring(0, idx0))
            //                    {
            //                        goto async;
            //                    }
            //                }
            //                m_IedPortRec[subIdx][idx][count].startP.Y = 500;
            //                m_IedPortRec[subIdx][idx][count].name = m_SubLine[subIdx][i].name.Substring(0, idx0);
            //                count++;
            //            }
            //        }
            //    async:
            //        {
            //        }
            //    }
            //    for (int i = 0; i < count; i++)
            //    {
            //        m_IedPortRec[subIdx][idx][i].startP.X = 20 + (650 * i / count);
            //    }
            //    int inum = 0;
            //    int cnum = 0;
            //    //int CRecNum = 0;
            //    for (int i = 0; i < m_LineCount; i++)
            //    {
            //        if (m_SubLine[subIdx][i].name != null)
            //        {
            //            int idx0 = m_SubLine[subIdx][i].name.IndexOf(":");
            //            int idx3 = m_SubLine[subIdx][i].name.IndexOf(" ");
            //            int idx1 = m_SubLine[subIdx][i].name.LastIndexOf(" ") + 1;
            //            int idx2 = m_SubLine[subIdx][i].name.LastIndexOf(":");
            //            for (int j = 0; j < count; j++)
            //            {
            //                if ((m_SubLine[subIdx][i].name.Substring(0, idx0) == m_IEDPoint[subIdx][idx].name
            //                    && m_SubLine[subIdx][i].name.Substring(idx1, idx2 - idx1) == m_IedPortRec[subIdx][idx][j].name)
            //                    || (m_SubLine[subIdx][i].name.Substring(0, idx0) == m_IedPortRec[subIdx][idx][j].name
            //                    && m_SubLine[subIdx][i].name.Substring(idx1, idx2 - idx1) == m_IEDPoint[subIdx][idx].name))
            //                {
            //                    // judge which connected rec
            //                    //if (CRecNum != j)
            //                    //{
            //                    //    CRecNum = j;
            //                    //    cnum = 0;
            //                    //}
            //                    m_IedLine[subIdx][idx][inum].startP = new Point(50 + (600 * (inum + 1) / 21), 80);// 20 ports in ied
            //                    m_IedLine[subIdx][idx][inum].endP = new Point(20 + (650 * j / count) + (60 * (cnum + 1) / 5), 500);// 4 ports in connected dev
            //                    m_IedLine[subIdx][idx][inum].name = m_SubLine[subIdx][i].name;
            //                    inum++;
            //                    cnum++;
            //                }
            //            }
            //        }
            //    }

            //}

            //2 set iedport rectangle
            for (int idx = 0; idx < m_IEDCount; idx++)
            {
                int count = 0;
                for (int i = 0; i < m_AccData.Virlines.Count; i++)
                {
                    try
                    {
                        if (m_AccData.Virlines[i].InDesc.Contains(m_IEDPoint[subIdx][idx].name))
                        {
                            int index0 = m_AccData.Virlines[i].ExtDesc.IndexOf(':') + 1;
                            int index1 = m_AccData.Virlines[i].ExtDesc.IndexOf('%');
                            for (int j = 0; j < count; j++)
                            {
                                if (m_AccData.Virlines[i].ExtDesc.Substring(index0, index1 - index0) == m_IedPortRec[subIdx][idx][j].name)
                                {
                                    goto i0;
                                }
                            }
                            // set connected ports point
                            m_IedPortRec[subIdx][idx][count].startP.Y = 500;
                            m_IedPortRec[subIdx][idx][count].name = m_AccData.Virlines[i].ExtDesc.Substring(index0, index1 - index0);
                            count++;
                        }
                        else if (m_AccData.Virlines[i].ExtDesc.Contains(m_IEDPoint[subIdx][idx].name))
                        {
                            int index0 = m_AccData.Virlines[i].InDesc.IndexOf(':') + 1;
                            int index1 = m_AccData.Virlines[i].InDesc.IndexOf('%');
                            for (int j = 0; j < count; j++)
                            {
                                if (m_AccData.Virlines[i].InDesc.Substring(index0, index1 - index0) == m_IedPortRec[subIdx][idx][j].name)
                                {
                                    goto i0;
                                }
                            }
                            m_IedPortRec[subIdx][idx][count].startP.Y = 500;
                            m_IedPortRec[subIdx][idx][count].name = m_AccData.Virlines[i].InDesc.Substring(index0, index1 - index0);
                            count++;
                        }
                    i0:
                        {
                        }
                    }
                    catch { }
                }
                for (int i = 0; i < count; i++)
                {
                    m_IedPortRec[subIdx][idx][i].startP.X = 20 + (650 * i / count);
                }
                int iNum = 0;// ied port seq
                int cNum = 0;// connected dev seq
                int lNum = 0;// line count
                for (int i = 0; i < m_AccData.Virlines.Count; i++)
                {
                    try
                    {
                        if (m_AccData.Virlines[i].ExtDesc != null)
                        {
                            int index0 = m_AccData.Virlines[i].InDesc.IndexOf(':') + 1;
                            int index1 = m_AccData.Virlines[i].InDesc.IndexOf('%');
                            int index2 = m_AccData.Virlines[i].ExtDesc.IndexOf(':') + 1;
                            int index3 = m_AccData.Virlines[i].ExtDesc.IndexOf('%');
                            for (int j = 0; j < count; j++)
                            {
                                // fan
                                if (m_AccData.Virlines[i].ExtDesc.Substring(index2, index3 - index2) == m_IEDPoint[subIdx][idx].name
                                    && m_AccData.Virlines[i].InDesc.Substring(index0, index1 - index0) == m_IedPortRec[subIdx][idx][j].name)
                                {
                                    // judge which connected rec
                                    string str0 = m_AccData.Virlines[i].ExtDesc.Substring(0, index3) + ":" + m_AccData.Virlines[i].ExtPort
                                    + " <-> " + m_AccData.Virlines[i].InDesc.Substring(0, index1) + ":" + m_AccData.Virlines[i].InPort;
                                    int idx00 = str0.IndexOf(" ");
                                    int idx01 = str0.LastIndexOf(":") + 1;
                                    bool rea = true;
                                    bool rea1 = true;
                                    for (int k = 0; k < lNum; k++)
                                    {
                                        if (str0 == m_IedLine[subIdx][idx][k].name)
                                        {
                                            goto a0;
                                        }
                                        string str = m_IedLine[subIdx][idx][k].name.Substring(m_IedLine[subIdx][idx][k].name.IndexOf(':') + 1);
                                        int idx0 = str.IndexOf(':') + 1;
                                        int idx1 = str.IndexOf(' ');
                                        if (m_AccData.Virlines[i].ExtPort == str.Substring(idx0, idx1 - idx0))
                                        {
                                            rea = false;
                                            if (m_AccData.Virlines[i].ExtDesc.Substring(0, index3) + ":" + m_AccData.Virlines[i].ExtPort
                                                == m_IedLine[subIdx][idx][k].name.Substring(0, m_IedLine[subIdx][idx][k].name.IndexOf(" <")))
                                            {
                                                m_IedLine[subIdx][idx][lNum].startP = m_IedLine[subIdx][idx][k].startP;
                                            }
                                        }

                                        int idx2 = str.LastIndexOf(':') + 1;
                                        if (m_AccData.Virlines[i].InPort == str.Substring(idx2))
                                        {
                                            rea1 = false;
                                            if (m_AccData.Virlines[i].InDesc.Substring(0, index1) + ":" + m_AccData.Virlines[i].InPort
                                                == m_IedLine[subIdx][idx][k].name.Substring(m_IedLine[subIdx][idx][k].name.LastIndexOf("> ") + 1))
                                            {
                                                m_IedLine[subIdx][idx][lNum].endP = m_IedLine[subIdx][idx][k].endP;
                                            }
                                            else
                                            {
                                                m_IedLine[subIdx][idx][lNum].endP = new Point(20 + (650 * j / count) + 20, 500);
                                            }
                                        }

                                    }
                                    if (rea)
                                    {
                                        iNum++;
                                        m_IedLine[subIdx][idx][lNum].startP = new Point(50 + (600 * iNum / 21), 80);// 20 ports in ied
                                    }
                                    if (rea1)
                                    {
                                        cNum++;
                                        m_IedLine[subIdx][idx][lNum].endP = new Point(20 + (650 * j / count) + (60 * cNum / 5), 500);
                                    }

                                    m_IedLine[subIdx][idx][lNum].name = str0;

                                    lNum++;
                                }
                                // zheng
                                else if (m_AccData.Virlines[i].InDesc.Substring(index0, index1 - index0) == m_IEDPoint[subIdx][idx].name
                                    && m_AccData.Virlines[i].ExtDesc.Substring(index2, index3 - index2) == m_IedPortRec[subIdx][idx][j].name)
                                {
                                    string str1 = m_AccData.Virlines[i].InDesc.Substring(0, index1) + ":" + m_AccData.Virlines[i].InPort
                                    + " <-> " + m_AccData.Virlines[i].ExtDesc.Substring(0, index3) + ":" + m_AccData.Virlines[i].ExtPort;
                                    bool rea = true;
                                    bool rea1 = true;
                                    for (int k = 0; k < lNum; k++)
                                    {
                                        if (str1 == m_IedLine[subIdx][idx][k].name)
                                        {
                                            goto a0;
                                        }
                                        string str = m_IedLine[subIdx][idx][k].name.Substring(m_IedLine[subIdx][idx][k].name.IndexOf(':') + 1);
                                        int idx0 = str.IndexOf(':') + 1;
                                        int idx1 = str.IndexOf(' ');
                                        if (m_AccData.Virlines[i].InPort == str.Substring(idx0, idx1 - idx0))
                                        {
                                            rea = false;
                                            if (m_AccData.Virlines[i].InDesc.Substring(0, index1) + ":" + m_AccData.Virlines[i].InPort
                                                == m_IedLine[subIdx][idx][k].name.Substring(0, m_IedLine[subIdx][idx][k].name.IndexOf(" <")))
                                            {
                                                m_IedLine[subIdx][idx][lNum].startP = m_IedLine[subIdx][idx][k].startP;
                                            }
                                        }
                                        int idx2 = str.LastIndexOf(':') + 1;
                                        if (m_AccData.Virlines[i].ExtPort == str.Substring(idx2))
                                        {
                                            rea1 = false;
                                            if (m_AccData.Virlines[i].ExtDesc.Substring(0, index3) + ":" + m_AccData.Virlines[i].ExtPort
                                                == m_IedLine[subIdx][idx][k].name.Substring(m_IedLine[subIdx][idx][k].name.LastIndexOf("> ") + 1))
                                            {
                                                m_IedLine[subIdx][idx][lNum].endP = m_IedLine[subIdx][idx][k].endP;
                                            }
                                            else
                                            {
                                                m_IedLine[subIdx][idx][lNum].endP = new Point(20 + (650 * j / count) + 20, 500);
                                            }
                                        }
                                    }
                                    if (rea)
                                    {
                                        iNum++;
                                        m_IedLine[subIdx][idx][lNum].startP = new Point(50 + (600 * iNum / 21), 80);// 20 ports in ied
                                    }
                                    if (rea1)
                                    {
                                        cNum++;
                                        m_IedLine[subIdx][idx][lNum].endP = new Point(20 + (650 * j / count) + (60 * cNum / 5), 500);
                                    }

                                    m_IedLine[subIdx][idx][lNum].name = str1;
                                    lNum++;
                                }
                            a0:
                                {
                                }
                            }
                        }
                    }
                    catch(Exception e) { }
                }
            }

        }

        private void SubNetLineTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Graphics gp = SubNetPictureBox.CreateGraphics();
            Pen bluePen = new Pen(Color.SkyBlue, 1.0f);
            Pen blackPen = new Pen(Color.Black, 1.0f);

            try
            {
                int idx = Convert.ToInt16(SubNetLineTable.Name.Substring(15));

                for (int i = 0; i < 100; i++)
                {
                    if (m_IEDPoint[idx][i].name != null)
                    {
                        gp.DrawEllipse(blackPen, m_IEDPoint[idx][i].startP.X, m_IEDPoint[idx][i].startP.Y, IedW, IedH);
                    }
                    if (m_SwitchPoint[idx][i].name != null)
                    {
                        gp.DrawRectangle(blackPen, m_SwitchPoint[idx][i].startP.X, m_SwitchPoint[idx][i].startP.Y, SwitchW, SwitchH);
                    }
                    if (m_ODFPoint[idx][i].name != null)
                    {
                        gp.DrawRectangle(blackPen, m_ODFPoint[idx][i].startP.X, m_ODFPoint[idx][i].startP.Y, OdfW, OdfH);
                    }
                    if (m_SubLine[idx][i].name != null)
                    {
                        gp.DrawLine(blackPen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                    }
                }
                for (int i = 0; i < 100; i++)
                {
                    if (i == e.RowIndex)
                    {
                        gp.DrawLine(bluePen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                    }
                }
            }
            catch { }

            gp.Dispose();
            blackPen.Dispose();
            bluePen.Dispose();
        }

        private void SubNetDevTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Graphics gp = SubNetPictureBox.CreateGraphics();
            Pen bluePen = new Pen(Color.SkyBlue, 1.0f);
            Pen blackPen = new Pen(Color.Black, 1.0f);
            int index0;
            int index1;
            int index2;

            try
            {
                int idx = Convert.ToInt16(SubNetLineTable.Name.Substring(15));

                for (int i = 0; i < 100; i++)
                {
                    if (m_IEDPoint[idx][i].name != null)
                    {
                        gp.DrawEllipse(blackPen, m_IEDPoint[idx][i].startP.X, m_IEDPoint[idx][i].startP.Y, IedW, IedH);
                    }
                    if (m_SwitchPoint[idx][i].name != null)
                    {
                        gp.DrawRectangle(blackPen, m_SwitchPoint[idx][i].startP.X, m_SwitchPoint[idx][i].startP.Y, SwitchW, SwitchH);
                    }
                    if (m_ODFPoint[idx][i].name != null)
                    {
                        gp.DrawRectangle(blackPen, m_ODFPoint[idx][i].startP.X, m_ODFPoint[idx][i].startP.Y, OdfW, OdfH);
                    }
                    if (m_SubLine[idx][i].name != null)
                    {
                        gp.DrawLine(blackPen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                    }
                }


                switch (e.ColumnIndex)
                {
                    case 1:
                        for (int i = 0; i < 100; i++)
                        {
                            if (m_IEDPoint[idx][i].name != null && i == e.RowIndex)
                            {
                                gp.DrawEllipse(bluePen, m_IEDPoint[idx][i].startP.X, m_IEDPoint[idx][i].startP.Y, IedW, IedH);
                            }
                            if (m_SubLine[idx][i].name != null)
                            {
                                index0 = m_SubLine[idx][i].name.IndexOf(":");
                                index1 = m_SubLine[idx][i].name.LastIndexOf(" ") + 1;
                                index2 = m_SubLine[idx][i].name.LastIndexOf(":");
                                if (m_SubLine[idx][i].name.Substring(0, index0) == m_IEDPoint[idx][e.RowIndex].name
                                  || m_SubLine[idx][i].name.Substring(index1, index2 - index1) == m_IEDPoint[idx][e.RowIndex].name)
                                {
                                    gp.DrawLine(bluePen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                                }
                            }
                        }
                        break;
                    case 2:
                        for (int i = 0; i < 100; i++)
                        {
                            if (m_SwitchPoint[idx][i].name != null && i == e.RowIndex)
                            {
                                gp.DrawRectangle(bluePen, m_SwitchPoint[idx][i].startP.X, m_SwitchPoint[idx][i].startP.Y, SwitchW, SwitchH);
                            }
                            if (m_SubLine[idx][i].name != null)
                            {
                                index0 = m_SubLine[idx][i].name.IndexOf(":");
                                index1 = m_SubLine[idx][i].name.LastIndexOf(" ") + 1;
                                index2 = m_SubLine[idx][i].name.LastIndexOf(":");
                                if (m_SubLine[idx][i].name.Substring(0, index0) == m_SwitchPoint[idx][e.RowIndex].name
                                  || m_SubLine[idx][i].name.Substring(index1, index2 - index1) == m_SwitchPoint[idx][e.RowIndex].name)
                                {
                                    gp.DrawLine(bluePen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int i = 0; i < 100; i++)
                        {
                            if (m_ODFPoint[idx][i].name != null && i == e.RowIndex)
                            {
                                gp.DrawRectangle(bluePen, m_ODFPoint[idx][i].startP.X, m_ODFPoint[idx][i].startP.Y, OdfW, OdfH);
                            }
                            if (m_SubLine[idx][i].name != null)
                            {
                                index0 = m_SubLine[idx][i].name.IndexOf(":");
                                index1 = m_SubLine[idx][i].name.LastIndexOf(" ") + 1;
                                index2 = m_SubLine[idx][i].name.LastIndexOf(":");
                                if (m_SubLine[idx][i].name.Substring(0, index0) == m_ODFPoint[idx][e.RowIndex].name
                                  || m_SubLine[idx][i].name.Substring(index1, index2 - index1) == m_ODFPoint[idx][e.RowIndex].name)
                                {
                                    gp.DrawLine(bluePen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
            
            gp.Dispose();
            blackPen.Dispose();
            bluePen.Dispose();
        }

        private void SubNetPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Rectangle[] IedRec = new Rectangle[100];
            Rectangle[] SwitchRec = new Rectangle[100];
            Rectangle[] OdfRec = new Rectangle[100];
            Rectangle[] LineRec = new Rectangle[100];
            Graphics gp = SubNetPictureBox.CreateGraphics();
            Pen bluePen = new Pen(Color.SkyBlue, 1.0f);
            Pen blackPen = new Pen(Color.Black, 1.0f);
            try
            {
                int idx = Convert.ToInt16(SubNetPictureBox.Name.Substring(16));

                for (int i = 0; i < 100; i++)
                {
                    if (m_IEDPoint[idx][i].startP != new Point(0, 0))
                    {
                        IedRec[i] = new Rectangle(m_IEDPoint[idx][i].startP, new Size(30, 30));
                    }

                    if (m_SwitchPoint[idx][i].startP != new Point(0, 0))
                    {
                        SwitchRec[i] = new Rectangle(m_SwitchPoint[idx][i].startP, new Size(60, 20));
                    }

                    if (m_ODFPoint[idx][i].startP != new Point(0, 0))
                    {
                        OdfRec[i] = new Rectangle(m_ODFPoint[idx][i].startP, new Size(20, 60));
                    }
                    if (m_SubLine[idx][i].startP != new Point(0, 0))
                    {
                        Point lineP = new Point((m_SubLine[idx][i].startP.X + m_SubLine[idx][i].endP.X) / 2 - 10,
                            (m_SubLine[idx][i].startP.Y + m_SubLine[idx][i].endP.Y) / 2 - 10);
                        LineRec[i] = new Rectangle(lineP, new Size(20, 20));
                    }
                }

                for (int i = 0; i < 100; i++)
                {
                    if (IedRec[i] != null)
                    {
                        gp.DrawEllipse(blackPen, IedRec[i]);
                    }

                    if (SwitchRec[i] != null)
                    {
                        gp.DrawRectangle(blackPen, SwitchRec[i]);
                    }

                    if (OdfRec[i] != null)
                    {
                        gp.DrawRectangle(blackPen, OdfRec[i]);
                    }

                    if (m_SubLine[idx][i].name != null)
                    {
                        gp.DrawLine(blackPen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                    }
                }
                for (int i = 0; i < 100; i++)
                {
                    if (IedRec[i] != null && IedRec[i].Contains(e.Location))
                    {
                        gp.DrawEllipse(bluePen, IedRec[i]);
                        for (int j = 0; j < 100; j++)
                        {
                            if (m_SubLine[idx][j].name != null && m_IEDPoint[idx][j].name != null
                                && m_SubLine[idx][j].name.Contains(m_IEDPoint[idx][i].name))
                            {
                                gp.DrawLine(bluePen, m_SubLine[idx][j].startP, m_SubLine[idx][j].endP);
                            }
                        }
                        break;
                    }
                    if (SwitchRec[i] != null && SwitchRec[i].Contains(e.Location))
                    {
                        gp.DrawRectangle(bluePen, SwitchRec[i]);
                        for (int j = 0; j < 100; j++)
                        {
                            if (m_SubLine[idx][j].name != null && m_IEDPoint[idx][j].name != null
                                && m_SubLine[idx][j].name.Contains(m_SwitchPoint[idx][i].name))
                            {
                                gp.DrawLine(bluePen, m_SubLine[idx][j].startP, m_SubLine[idx][j].endP);
                            }
                        }
                        break;
                    }
                    if (OdfRec[i] != null && OdfRec[i].Contains(e.Location))
                    {
                        gp.DrawRectangle(bluePen, OdfRec[i]);
                        for (int j = 0; j < 100; j++)
                        {
                            if (m_SubLine[idx][j].name != null && m_IEDPoint[idx][j].name != null
                                && m_SubLine[idx][j].name.Contains(m_ODFPoint[idx][i].name))
                            {
                                gp.DrawLine(bluePen, m_SubLine[idx][j].startP, m_SubLine[idx][j].endP);
                            }
                        }
                        break;
                    }
                    if (LineRec[i] != null && LineRec[i].Contains(e.Location))
                    {
                        gp.DrawLine(bluePen, m_SubLine[idx][i].startP, m_SubLine[idx][i].endP);
                        break;
                    }
                }
            }
            catch { }

            gp.Dispose();
            blackPen.Dispose();
            bluePen.Dispose();
        }

        private void SubNetPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Rectangle[] IedRec = new Rectangle[100];
            try
            {
                int idx = Convert.ToInt16(SubNetPictureBox.Name.Substring(16));

                for (int i = 0; i < 100; i++)
                {
                    if (m_IEDPoint[idx][i].startP != new Point(0, 0))
                    {
                        IedRec[i] = new Rectangle(m_IEDPoint[idx][i].startP, new Size(50, 50));
                    }
                }

                for (int i = 0; i < 100; i++)
                {
                    if (IedRec[i] != null && IedRec[i].Contains(e.Location))
                    {
                        NetCfgTab.SelectedIndex = idx + 1;
                        foreach (TreeNode node in IEDTreeView.Nodes)
                        {
                            if (node.Text == m_IEDPoint[idx][i].name)
                            {
                                IEDTreeView.SelectedNode = node;
                                return;
                            }
                        }
                    }

                }
            }
            catch { }
        }

        private void IEDInVirLineTab_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Graphics gp = PortPicture.CreateGraphics();
            Pen bluePen = new Pen(Color.SkyBlue, 2.0f);
            Pen blackPen = new Pen(Color.Black, 2.0f);
            AdjustableArrowCap aac = new AdjustableArrowCap(8, 12);
            bluePen.CustomStartCap = aac;
            blackPen.CustomStartCap = aac;
            blackPen.CustomEndCap = aac;

            StringFormat stringFormat = new StringFormat
            {
                FormatFlags = StringFormatFlags.NoWrap,
                Alignment = StringAlignment.Center
            };
            SolidBrush DrawBrush = new SolidBrush(Color.Black);
            Font DrawFont = new Font("Times New Roman", 10);

            int SNidx = 0;
            int IEDidx = 0;
            try
            {
                for (; SNidx < m_SubNet.SubNetData.Length; SNidx++)
                {
                    foreach (string iedName in m_SubNet.SubNetData[SNidx].IEDName)
                    {
                        if (IEDInVirLineTab.Rows[e.RowIndex].Cells[3].Value.ToString().Contains(iedName))
                        {
                            foreach (DevRect ied in m_IEDPoint[SNidx])
                            {
                                if (ied.name == iedName)
                                {
                                    goto inx;
                                }
                                IEDidx++;
                            }
                        }
                    }
                }

            //foreach (LinePos[][] ledLine in m_IedLine)
            //{

            //foreach (LinePos[] lLine in m_IedLine[SNidx])
            //{
            inx:
                foreach (LinePos line in m_IedLine[SNidx][IEDidx])
                {
                    if (line.name != null)
                    {
                        int idx0 = line.name.IndexOf(':');
                        int idx1 = line.name.IndexOf(' ');
                        int idx2 = line.name.LastIndexOf(' ');
                        int idx3 = line.name.LastIndexOf(':');

                        //IEDInVirLineTab.Rows[e.RowIndex].Cells[1].Value.ToString().Contains(line.name.Substring(idx0 + 1, idx1 - idx0))
                        //    IEDInVirLineTab.Rows[e.RowIndex].Cells[3].Value.ToString().Contains(m_IEDPoint[SNidx][j].name)
                        //    && (
                        if (IEDInVirLineTab.Rows[e.RowIndex].Cells[6].Value.ToString().Contains(line.name.Substring(idx2 + 1, idx3 - idx2 - 1))
                            || IEDInVirLineTab.Rows[e.RowIndex].Cells[6].Value.ToString().Contains(line.name.Substring(0, idx0)))
                        {
                            for (int k = 0; k < 20; k++)
                            {
                                if (m_IedLine[SNidx][IEDidx][k].name != null)
                                {
                                    gp.DrawLine(blackPen, m_IedLine[SNidx][IEDidx][k].startP, m_IedLine[SNidx][IEDidx][k].endP);
                                }
                            }
                            gp.DrawLine(bluePen, line.startP, line.endP);
                            goto aa;
                        }
                    }
                }
            //j++;
            //}
            //i++;
            //}

            aa:
                IedCBAddrLbl.Text = "";
                foreach (IedComm iedCommData in m_SubNet.IedCommDataList)
                {
                    if (IEDInVirLineTab.Rows[e.RowIndex].Cells[7].Value.ToString()
                        == iedCommData.IEDName + "/" + iedCommData.LdInst + "/" + iedCommData.CbName)
                    {
                        IedCBAddrLbl.Text = iedCommData.IEDName + "/" + iedCommData.LdInst + "/" + iedCommData.CbName 
                            + "\n\n" + iedCommData.CbAddr;
                        break;
                    }
                }
            }
            catch { }
            gp.Dispose();
        }

        private void IEDOutVirLineTab_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Graphics gp = PortPicture.CreateGraphics();
            Pen bluePen = new Pen(Color.SkyBlue, 2.0f);
            Pen blackPen = new Pen(Color.Black, 2.0f);
            AdjustableArrowCap aac = new AdjustableArrowCap(8, 12);
            bluePen.CustomEndCap = aac;
            blackPen.CustomStartCap = aac;
            blackPen.CustomEndCap = aac;

            StringFormat stringFormat = new StringFormat
            {
                FormatFlags = StringFormatFlags.NoWrap,
                Alignment = StringAlignment.Center
            };
            SolidBrush DrawBrush = new SolidBrush(Color.Black);
            Font DrawFont = new Font("Times New Roman", 10);

            //int subIdx = 
            //int i = 0;
            int SNidx = 0;
            int IEDidx = 0;
            try
            {
                for (; SNidx < m_SubNet.SubNetData.Length; SNidx++)
                {
                    foreach (string iedName in m_SubNet.SubNetData[SNidx].IEDName)
                    {
                        if (IEDOutVirLineTab.Rows[e.RowIndex].Cells[3].Value.ToString().Contains(iedName))
                        {
                            IEDidx = 0;
                            foreach (DevRect ied in m_IEDPoint[SNidx])
                            {
                                if (ied.name == iedName)
                                {
                                    goto outx;
                                }
                                IEDidx++;
                            }
                                
                        }
                    }
                }
                

                //foreach (LinePos[][] ledLine in m_IedLine)
                //{
            
                //foreach (LinePos[] lLine in m_IedLine[SNidx])
                //{
            outx:
                foreach (LinePos line in m_IedLine[SNidx][IEDidx])
                {
                    if (line.name != null)
                    {
                        int idx0 = line.name.IndexOf(':');
                        int idx1 = line.name.IndexOf(' ');
                        int idx2 = line.name.LastIndexOf(' ');
                        int idx3 = line.name.LastIndexOf(':');

                        //IEDInVirLineTab.Rows[e.RowIndex].Cells[1].Value.ToString().Contains(line.name.Substring(idx0 + 1, idx1 - idx0))
                        //IEDOutVirLineTab.Rows[e.RowIndex].Cells[3].Value.ToString().Contains(m_IEDPoint[SNidx][j].name)
                        //    &&
                        if (IEDOutVirLineTab.Rows[e.RowIndex].Cells[5].Value.ToString().Contains(line.name.Substring(0, idx0))
                            || IEDOutVirLineTab.Rows[e.RowIndex].Cells[5].Value.ToString().Contains(line.name.Substring(idx2 + 1, idx3 - idx2 - 1)))
                        {
                            for (int k = 0; k < 20; k++)
                            {
                                if (m_IedLine[SNidx][IEDidx][k].name != null)
                                {
                                    gp.DrawLine(blackPen, m_IedLine[SNidx][IEDidx][k].startP, m_IedLine[SNidx][IEDidx][k].endP);
                                }
                            }
                            gp.DrawLine(bluePen, line.startP, line.endP);
                            goto aa;
                        }
                    }
                }
            //j++;
            //}

            //i++;
            //}

            aa:
                IedCBAddrLbl.Text = "";
                foreach (IedComm iedCommData in m_SubNet.IedCommDataList)
                {
                    if (IEDOutVirLineTab.Rows[e.RowIndex].Cells[6].Value.ToString()
                        == iedCommData.IEDName + "/" + iedCommData.LdInst + "/" + iedCommData.CbName)
                    {
                        IedCBAddrLbl.Text = iedCommData.IEDName + "/" + iedCommData.LdInst + "/" + iedCommData.CbName
                            + "\n\n" + iedCommData.CbAddr;
                        break;
                    }
                }
            }
            catch { }
            gp.Dispose();
        }

        private void IEDInVirLineTab_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 3)
            {
                foreach (VirLine line in m_AccData.Virlines)
                {
                    if (line.IntAddr == IEDInVirLineTab.Rows[e.RowIndex].Cells[2].Value.ToString())
                    {
                        if (e.ColumnIndex == 1)
                        {
                            line.InPort = IEDInVirLineTab.Rows[e.RowIndex].Cells[1].Value.ToString();
                        }
                        else if (e.ColumnIndex == 3)
                        {
                            line.ExtPort = IEDInVirLineTab.Rows[e.RowIndex].Cells[3].Value.ToString();
                        }
                    }
                }
            }
        }
    }
}
//private void SetSwitchGraph(int subIdx, int connIdx, int physIdx)
        //{
        //    //int pictureW = SubNetPictureBox.Width;
        //    //int pictureH = SubNetPictureBox.Height;

        //    //int innerFullScaleW = 100;
        //    //int innerFullScaleH = 551;

        //    //int switchLeft = (pictureW * 48) / innerFullScaleW;
        //    //int switchTop = (pictureH * 16) / innerFullScaleH;
        //    //int switchHeight = (pictureH * 11) / innerFullScaleH;
        //    //int switchWidth = (pictureW * 4) / innerFullScaleW;
        //    //int switchGap = (pictureH * 31) / innerFullScaleH;

        //    foreach (KeyValuePair<string, string> physNode in m_SubNet.PhysNodes[subIdx, connIdx, physIdx])
        //    {

        //        if (physNode.Key == "Cable")
        //        {
        //            for (int i = 0; i < m_SwitchIdx; i++)
        //            {
        //                if (m_SwitchPoint[i].name == physNode.Value.ToString())
        //                {
        //                    return;
        //                }
        //            }
        //            //m_SwitchPoint[m_SwitchIdx].name = physNode.Value;
        //            //m_SwitchPoint[m_SwitchIdx].startP = new Point(switchLeft, switchTop + switchGap * m_SwitchIdx);
        //            //m_SwitchPoint[m_SwitchIdx].width = switchWidth;
        //            //m_SwitchPoint[m_SwitchIdx].height = switchHeight;
        //            //m_SwitchIdx++;

        //            // add switch / odf name to table
        //            if (string.Equals(physNode.Value.Substring(0, 6), "Switch"))
        //            {
        //                SubNetDevTable.Rows[m_SwitchCount].Cells[2].Value = physNode.Value;
        //                m_SwitchCount++;
        //            }
        //            else if(string.Equals(physNode.Value.Substring(0, 6), "ODF"))
        //            {
        //                SubNetDevTable.Rows[m_ODFCount].Cells[3].Value = m_SwitchPoint[m_SwitchIdx].name;
        //                m_ODFCount++;
        //            }
        //        }

        //    }
        //}

        //private void SetDevGraphData()
        //{

        //    int frontTop = 50;
        //    int frontMid = (frontTop + m_panelH) / 2;
        //    int frontBottom = m_panelH - 1;
        //    int midWidth = 33;
        //    int frontTopBottom = frontMid - midWidth;                          // top panel bottom
        //    int frontTopMid = (frontTop + frontTopBottom) / 2;                 // top panel 1/2
        //    int frontTopUpMid = (frontTop + frontTopMid) / 2;                  // top panel 1/4
        //    int frontTopDownMid = (frontTopBottom + frontTopMid) / 2;          // top panel 3/4     

        //    int frontBottomTop = frontMid + midWidth;                          // bottom panel top
        //    int frontBottomMid = (frontBottom + frontBottomTop) / 2;           // bottom panel 1/2
        //    int frontBottomUpMid = (frontBottomTop + frontBottomMid) / 2;      // bottom panel 1/4
        //    int frontBottomDownMid = (frontBottom + frontBottomMid) / 2;       // bottom panel 3/4
        //    int frontBottomUp3 = (frontBottom - frontBottomTop) / 3;           // bottom panel 1/3
        //    int frontBottomDown3 = (frontBottom - frontBottomTop) * 2 / 3;     // bottom panel 2/3

        //    int frontHeight = m_panelH - frontTop;
        //    int frontRight = m_panelW - 1;
        //    int sideWidth = 50;                                                // Width of side plate
        //    int innerLeft = sideWidth;
        //    int innerRight = frontRight - sideWidth;
        //    int innerWidth = innerRight - innerLeft;

        //    int innerFullScale = 520;                                          // indicates proportions, not the actual lengths
        //    int pspaMid = (innerWidth * 100) / innerFullScale + innerLeft;
        //    int ifcMid = (innerWidth * 50) / innerFullScale + innerLeft;
        //    int swtLeft = (innerWidth * 100) / innerFullScale + innerLeft;
        //    int swtRight = (innerWidth * 150) / innerFullScale + innerLeft;
        //    int alrRight = (innerWidth * 200) / innerFullScale + innerLeft;
        //    int CspiRight = (innerWidth * 250) / innerFullScale + innerLeft;
        //    int VfmMid = (innerWidth * 300) / innerFullScale + innerLeft;
        //    int VmuxLeft = (innerWidth * 350) / innerFullScale + innerLeft;
        //    int AmfLeft = (innerWidth * 400) / innerFullScale + innerLeft;
        //    int AmfRight = (innerWidth * 440) / innerFullScale + innerLeft;
        //    int LtLeft = (innerWidth * 460) / innerFullScale + innerLeft;

        //    int RfRight = (innerWidth * 200) / innerFullScale + innerLeft;
        //    int SlRight = (innerWidth * 230) / innerFullScale + innerLeft;
        //    int X211Right = (innerWidth * 260) / innerFullScale + innerLeft;
        //    int Vfx1Right = (innerWidth * 290) / innerFullScale + innerLeft;
        //    int Vfx2Right = (innerWidth * 320) / innerFullScale + innerLeft;
        //    int Rs232_12Right = (innerWidth * 350) / innerFullScale + innerLeft;
        //    int Rs232_58Right = (innerWidth * 380) / innerFullScale + innerLeft;
        //    int PsLeft = (innerWidth * 450) / innerFullScale + innerLeft;

        //    m_fixLine = new LinePos[DEV_FRAME_FIXED_COUNT];
        //    // Main frame
        //    m_fixLine[0].StartP = new Point(0, frontTop);                       // Front frame - top
        //    m_fixLine[0].EndP = new Point(m_panelW, frontTop);
        //    m_fixLine[1].StartP = new Point(0, frontTop);                       // Front frame - left
        //    m_fixLine[1].EndP = new Point(0, frontBottom);
        //    m_fixLine[2].StartP = new Point(frontRight, frontTop);              // Front frame - right
        //    m_fixLine[2].EndP = new Point(frontRight, frontBottom);
        //    m_fixLine[3].StartP = new Point(0, frontBottom);                    // Front frame - bottom
        //    m_fixLine[3].EndP = new Point(frontRight, frontBottom);

        //    m_fixLine[4].StartP = new Point(innerLeft, frontTopBottom);         // Inner panel - Top panel bottom
        //    m_fixLine[4].EndP = new Point(innerRight, frontTopBottom);
        //    m_fixLine[5].StartP = new Point(innerLeft, frontBottomTop);         // Inner panel - Bottom panel Top
        //    m_fixLine[5].EndP = new Point(innerRight, frontBottomTop);
        //    m_fixLine[6].StartP = new Point(innerLeft, frontTop);               // Inner panel - left
        //    m_fixLine[6].EndP = new Point(innerLeft, frontBottom);
        //    m_fixLine[7].StartP = new Point(innerRight, frontTop);              // Inner panel - right
        //    m_fixLine[7].EndP = new Point(innerRight, frontBottom);

        //    m_fixLine[8].StartP = new Point(innerLeft, frontTop);               // Horizontal view - left
        //    m_fixLine[8].EndP = new Point(130, 1);
        //    m_fixLine[9].StartP = new Point(innerRight, frontTop);              // Horizontal view - right
        //    m_fixLine[9].EndP = new Point(m_panelW - 130, 1);
        //    m_fixLine[10].StartP = new Point(130, 1);                            // Horizontal view - back
        //    m_fixLine[10].EndP = new Point(m_panelW - 130, 1);

        //    // Fixed compartments
        //    m_fixLine[20].StartP = new Point(innerLeft, frontTopMid);           // Separation - PSPA-1(2) / iSWT
        //    m_fixLine[20].EndP = new Point(alrRight, frontTopMid);
        //    m_fixLine[21].StartP = new Point(swtRight, frontTopMid);            // Separation - iSWT / ALR
        //    m_fixLine[21].EndP = new Point(swtRight, frontTopBottom);
        //    m_fixLine[22].StartP = new Point(alrRight, frontTop);            // Separation - ALR / Cspi
        //    m_fixLine[22].EndP = new Point(alrRight, frontTopBottom);
        //    m_fixLine[23].StartP = new Point(CspiRight, frontTop);              // Separation - Cspi / VFM-1
        //    m_fixLine[23].EndP = new Point(CspiRight, frontTopBottom);
        //    m_fixLine[24].StartP = new Point(VfmMid, frontTop);                 // Separation - VFM-1 / VFM-2
        //    m_fixLine[24].EndP = new Point(VfmMid, frontTopBottom);
        //    m_fixLine[25].StartP = new Point(VmuxLeft, frontTop);               // Separation - Vmux / VFM-2
        //    m_fixLine[25].EndP = new Point(VmuxLeft, frontTopBottom);
        //    m_fixLine[26].StartP = new Point(AmfLeft, frontTop);                // Separation - AMF / Vmux
        //    m_fixLine[26].EndP = new Point(AmfLeft, frontTopBottom);
        //    m_fixLine[27].StartP = new Point(LtLeft, frontTop);                 // Separation - AMF(RXF) / LT(TXF)
        //    m_fixLine[27].EndP = new Point(LtLeft, frontTopBottom);
        //    m_fixLine[28].StartP = new Point(AmfLeft, frontTopMid);              // Separation - AMF / RXF
        //    m_fixLine[28].EndP = new Point(AmfRight, frontTopMid);
        //    m_fixLine[29].StartP = new Point(CspiRight, frontTopUpMid);         // Separation - VFM 1/4
        //    m_fixLine[29].EndP = new Point(VmuxLeft, frontTopUpMid);
        //    m_fixLine[30].StartP = new Point(CspiRight, frontTopMid);           // Separation - VFM 1/2
        //    m_fixLine[30].EndP = new Point(VmuxLeft, frontTopMid);
        //    m_fixLine[31].StartP = new Point(CspiRight, frontTopDownMid);       // Separation - VFM 3/4
        //    m_fixLine[31].EndP = new Point(VmuxLeft, frontTopDownMid);

        //    m_fixLine[32].StartP = new Point(innerLeft, frontBottomDownMid);    // Separation - RF
        //    m_fixLine[32].EndP = new Point(RfRight, frontBottomDownMid);
        //    m_fixLine[33].StartP = new Point(RfRight, frontBottomDownMid);      // Separation - RF
        //    m_fixLine[33].EndP = new Point(RfRight, frontBottom);

        //    m_fixLine[34].StartP = new Point(RfRight + 2, frontBottomTop + 5);                      // SL
        //    m_fixLine[34].EndP = new Point(SlRight - 2, frontBottomTop + 5);
        //    m_fixLine[35].StartP = new Point(RfRight + 2, frontBottomTop + frontBottomUp3 / 2 - 5); // SL
        //    m_fixLine[35].EndP = new Point(SlRight - 2, frontBottomTop + frontBottomUp3 / 2 - 5);
        //    m_fixLine[36].StartP = new Point(RfRight + 2, frontBottomTop + 5);                      // SL
        //    m_fixLine[36].EndP = new Point(RfRight + 2, frontBottomTop + frontBottomUp3 / 2 - 5);
        //    m_fixLine[37].StartP = new Point(SlRight - 2, frontBottomTop + 5);                      // SL
        //    m_fixLine[37].EndP = new Point(SlRight - 2, frontBottomTop + frontBottomUp3 / 2 - 5);

        //    m_fixLine[38].StartP = new Point(RfRight + 2, frontBottomTop + frontBottomUp3 / 2 + 5); // FE1
        //    m_fixLine[38].EndP = new Point(SlRight - 2, frontBottomTop + frontBottomUp3 / 2 + 5);
        //    m_fixLine[39].StartP = new Point(RfRight + 2, frontBottomTop + frontBottomUp3 - 5);     // FE1
        //    m_fixLine[39].EndP = new Point(SlRight - 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[40].StartP = new Point(RfRight + 2, frontBottomTop + frontBottomUp3 / 2 + 5); // FE1
        //    m_fixLine[40].EndP = new Point(RfRight + 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[41].StartP = new Point(SlRight - 2, frontBottomTop + frontBottomUp3 / 2 + 5); // FE1
        //    m_fixLine[41].EndP = new Point(SlRight - 2, frontBottomTop + frontBottomUp3 - 5);

        //    m_fixLine[42].StartP = new Point(SlRight + 2, frontBottomTop + 5);                      // X21 1
        //    m_fixLine[42].EndP = new Point(X211Right - 2, frontBottomTop + 5);
        //    m_fixLine[43].StartP = new Point(SlRight + 2, frontBottomUpMid - 5);                    // X21 1
        //    m_fixLine[43].EndP = new Point(X211Right - 2, frontBottomUpMid - 5);
        //    m_fixLine[44].StartP = new Point(SlRight + 2, frontBottomTop + 5);                      // X21 1
        //    m_fixLine[44].EndP = new Point(SlRight + 2, frontBottomUpMid - 5);
        //    m_fixLine[45].StartP = new Point(X211Right - 2, frontBottomTop + 5);                    // X21 1
        //    m_fixLine[45].EndP = new Point(X211Right - 2, frontBottomUpMid - 5);

        //    m_fixLine[46].StartP = new Point(SlRight + 2, frontBottomUpMid + 5);                    // X21 2
        //    m_fixLine[46].EndP = new Point(X211Right - 2, frontBottomUpMid + 5);
        //    m_fixLine[47].StartP = new Point(SlRight + 2, frontBottomMid - 5);                      // X21 2
        //    m_fixLine[47].EndP = new Point(X211Right - 2, frontBottomMid - 5);
        //    m_fixLine[48].StartP = new Point(SlRight + 2, frontBottomUpMid + 5);                    // X21 2
        //    m_fixLine[48].EndP = new Point(SlRight + 2, frontBottomMid - 5);
        //    m_fixLine[49].StartP = new Point(X211Right - 2, frontBottomUpMid + 5);                  // X21 2
        //    m_fixLine[49].EndP = new Point(X211Right - 2, frontBottomMid - 5);

        //    m_fixLine[50].StartP = new Point(SlRight + 2, frontBottomMid + 5);                      // X21 DP
        //    m_fixLine[50].EndP = new Point(X211Right - 2, frontBottomMid + 5);
        //    m_fixLine[51].StartP = new Point(SlRight + 2, frontBottomDownMid - 5);                  // X21 DP
        //    m_fixLine[51].EndP = new Point(X211Right - 2, frontBottomDownMid - 5);
        //    m_fixLine[52].StartP = new Point(SlRight + 2, frontBottomMid + 5);                      // X21 DP
        //    m_fixLine[52].EndP = new Point(SlRight + 2, frontBottomDownMid - 5);
        //    m_fixLine[53].StartP = new Point(X211Right - 2, frontBottomMid + 5);                    // X21 DP
        //    m_fixLine[53].EndP = new Point(X211Right - 2, frontBottomDownMid - 5);

        //    m_fixLine[54].StartP = new Point(SlRight + 2, frontBottomDownMid + 5);                  // RM 1
        //    m_fixLine[54].EndP = new Point(X211Right - 2, frontBottomDownMid + 5);
        //    m_fixLine[55].StartP = new Point(SlRight + 2, frontBottom - 5);                         // RM 1
        //    m_fixLine[55].EndP = new Point(X211Right - 2, frontBottom - 5);
        //    m_fixLine[56].StartP = new Point(SlRight + 2, frontBottomDownMid + 5);                  // RM 1
        //    m_fixLine[56].EndP = new Point(SlRight + 2, frontBottom - 5);
        //    m_fixLine[57].StartP = new Point(X211Right - 2, frontBottomDownMid + 5);                // RM 1
        //    m_fixLine[57].EndP = new Point(X211Right - 2, frontBottom - 5);

        //    m_fixLine[58].StartP = new Point(X211Right + 2, frontBottomTop + 5);                    // VFX1 P1-3
        //    m_fixLine[58].EndP = new Point(Vfx1Right - 2, frontBottomTop + 5);
        //    m_fixLine[59].StartP = new Point(X211Right + 2, frontBottomTop + frontBottomUp3 - 5);   // VFX1 P1-3
        //    m_fixLine[59].EndP = new Point(Vfx1Right - 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[60].StartP = new Point(X211Right + 2, frontBottomTop + 5);                    // VFX1 P1-3
        //    m_fixLine[60].EndP = new Point(X211Right + 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[61].StartP = new Point(Vfx1Right - 2, frontBottomTop + 5);                    // VFX1 P1-3
        //    m_fixLine[61].EndP = new Point(Vfx1Right - 2, frontBottomTop + frontBottomUp3 - 5);

        //    m_fixLine[62].StartP = new Point(X211Right + 2, frontBottomTop + frontBottomUp3 + 5);   // VFX1 P4
        //    m_fixLine[62].EndP = new Point(Vfx1Right - 2, frontBottomTop + frontBottomUp3 + 5);
        //    m_fixLine[63].StartP = new Point(X211Right + 2, frontBottomTop + frontBottomDown3 - 5); // VFX1 P4
        //    m_fixLine[63].EndP = new Point(Vfx1Right - 2, frontBottomTop + frontBottomDown3 - 5);
        //    m_fixLine[64].StartP = new Point(X211Right + 2, frontBottomTop + frontBottomUp3 + 5);   // VFX1 P4
        //    m_fixLine[64].EndP = new Point(X211Right + 2, frontBottomTop + frontBottomDown3 - 5);
        //    m_fixLine[65].StartP = new Point(Vfx1Right - 2, frontBottomTop + frontBottomUp3 + 5);   // VFX1 P4
        //    m_fixLine[65].EndP = new Point(Vfx1Right - 2, frontBottomTop + frontBottomDown3 - 5);

        //    m_fixLine[66].StartP = new Point(Vfx1Right + 2, frontBottomTop + 5);                    // VFX2
        //    m_fixLine[66].EndP = new Point(Vfx2Right - 2, frontBottomTop + 5);
        //    m_fixLine[67].StartP = new Point(Vfx1Right + 2, frontBottomTop + frontBottomUp3 - 5);   // VFX2
        //    m_fixLine[67].EndP = new Point(Vfx2Right - 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[68].StartP = new Point(Vfx1Right + 2, frontBottomTop + 5);                    // VFX2
        //    m_fixLine[68].EndP = new Point(Vfx1Right + 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[69].StartP = new Point(Vfx2Right - 2, frontBottomTop + 5);                    // VFX2
        //    m_fixLine[69].EndP = new Point(Vfx2Right - 2, frontBottomTop + frontBottomUp3 - 5);

        //    m_fixLine[70].StartP = new Point(Vfx1Right + 2, frontBottomDownMid + 5);                // IP 1
        //    m_fixLine[70].EndP = new Point(Vfx2Right - 2, frontBottomDownMid + 5);
        //    m_fixLine[71].StartP = new Point(Vfx1Right + 2, frontBottom - 5);                       // IP 1
        //    m_fixLine[71].EndP = new Point(Vfx2Right - 2, frontBottom - 5);
        //    m_fixLine[72].StartP = new Point(Vfx1Right + 2, frontBottomDownMid + 5);                // IP 1
        //    m_fixLine[72].EndP = new Point(Vfx1Right + 2, frontBottom - 5);
        //    m_fixLine[73].StartP = new Point(Vfx2Right - 2, frontBottomDownMid + 5);                // IP 1
        //    m_fixLine[73].EndP = new Point(Vfx2Right - 2, frontBottom - 5);

        //    m_fixLine[74].StartP = new Point(Vfx2Right + 2, frontBottomTop + 5);                    // RS232_1-2
        //    m_fixLine[74].EndP = new Point(Rs232_12Right - 2, frontBottomTop + 5);
        //    m_fixLine[75].StartP = new Point(Vfx2Right + 2, frontBottomTop + frontBottomUp3 - 5);    // RS232_1-2
        //    m_fixLine[75].EndP = new Point(Rs232_12Right - 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[76].StartP = new Point(Vfx2Right + 2, frontBottomTop + 5);                    // RS232_1-2
        //    m_fixLine[76].EndP = new Point(Vfx2Right + 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[77].StartP = new Point(Rs232_12Right - 2, frontBottomTop + 5);                // RS232_1-2
        //    m_fixLine[77].EndP = new Point(Rs232_12Right - 2, frontBottomTop + frontBottomUp3 - 5);

        //    m_fixLine[78].StartP = new Point(Vfx2Right + 2, frontBottomTop + frontBottomUp3 + 5);    // RS232_3-4
        //    m_fixLine[78].EndP = new Point(Rs232_12Right - 2, frontBottomTop + frontBottomUp3 + 5);
        //    m_fixLine[79].StartP = new Point(Vfx2Right + 2, frontBottomTop + frontBottomDown3 - 5);  // RS232_3-4
        //    m_fixLine[79].EndP = new Point(Rs232_12Right - 2, frontBottomTop + frontBottomDown3 - 5);
        //    m_fixLine[80].StartP = new Point(Vfx2Right + 2, frontBottomTop + frontBottomUp3 + 5);    // RS232_3-4
        //    m_fixLine[80].EndP = new Point(Vfx2Right + 2, frontBottomTop + frontBottomDown3 - 5);
        //    m_fixLine[81].StartP = new Point(Rs232_12Right - 2, frontBottomTop + frontBottomUp3 + 5);// RS232_3-4
        //    m_fixLine[81].EndP = new Point(Rs232_12Right - 2, frontBottomTop + frontBottomDown3 - 5);

        //    m_fixLine[82].StartP = new Point(Rs232_12Right + 2, frontBottomTop + 5);                 // RS232_5-8
        //    m_fixLine[82].EndP = new Point(Rs232_58Right - 2, frontBottomTop + 5);
        //    m_fixLine[83].StartP = new Point(Rs232_12Right + 2, frontBottomTop + frontBottomUp3 - 5);// RS232_5-8
        //    m_fixLine[83].EndP = new Point(Rs232_58Right - 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[84].StartP = new Point(Rs232_12Right + 2, frontBottomTop + 5);                // RS232_5-8
        //    m_fixLine[84].EndP = new Point(Rs232_12Right + 2, frontBottomTop + frontBottomUp3 - 5);
        //    m_fixLine[85].StartP = new Point(Rs232_58Right - 2, frontBottomTop + 5);                // RS232_5-8
        //    m_fixLine[85].EndP = new Point(Rs232_58Right - 2, frontBottomTop + frontBottomUp3 - 5);

        //    m_fixLine[86].StartP = new Point(Rs232_12Right + 2, frontBottomMid + 5);                // ALARM
        //    m_fixLine[86].EndP = new Point(Rs232_58Right - 2, frontBottomMid + 5);
        //    m_fixLine[87].StartP = new Point(Rs232_12Right + 2, frontBottom - 5);                   // ALARM
        //    m_fixLine[87].EndP = new Point(Rs232_58Right - 2, frontBottom - 5);
        //    m_fixLine[88].StartP = new Point(Rs232_12Right + 2, frontBottomMid + 5);                // ALARM
        //    m_fixLine[88].EndP = new Point(Rs232_12Right + 2, frontBottom - 5);
        //    m_fixLine[89].StartP = new Point(Rs232_58Right - 2, frontBottomMid + 5);                // ALARM
        //    m_fixLine[89].EndP = new Point(Rs232_58Right - 2, frontBottom - 5);

        //    m_fixLine[90].StartP = new Point(PsLeft + 5, frontBottomUpMid + 5);                     // PS
        //    m_fixLine[90].EndP = new Point(innerRight - 5, frontBottomUpMid + 5);
        //    m_fixLine[91].StartP = new Point(PsLeft + 5, frontBottomDownMid - 5);                   // PS
        //    m_fixLine[91].EndP = new Point(innerRight - 5, frontBottomDownMid - 5);
        //    m_fixLine[92].StartP = new Point(PsLeft + 5, frontBottomUpMid + 5);                     // PS
        //    m_fixLine[92].EndP = new Point(PsLeft + 5, frontBottomDownMid - 5);
        //    m_fixLine[93].StartP = new Point(innerRight - 5, frontBottomUpMid + 5);                 // PS
        //    m_fixLine[93].EndP = new Point(innerRight - 5, frontBottomDownMid - 5);

        //    m_fixLine[101].StartP = new Point(AmfRight, frontTop);               // Separation - AMF(RXF) / LT(TXF)
        //    m_fixLine[101].EndP = new Point(AmfRight, frontTopBottom);
        //    m_fixLine[102].StartP = new Point(LtLeft, frontTopMid);              // Separation - LT / TXF
        //    m_fixLine[102].EndP = new Point(innerRight, frontTopMid);
        //    // end of DEV_FRAME_FIXED_COUNT

        //    m_cfgLine = new LinePos[DEV_FRAME_CFG_COUNT];
        //    // Changeable compartments based on actual configurations
        //    m_cfgLine[(int)E_SELECT_LINE.LINE_PS1].StartP = new Point(pspaMid, frontTop);                 // Separation - PS1 / PS2
        //    m_cfgLine[(int)E_SELECT_LINE.LINE_PS1].EndP = new Point(pspaMid, frontTopMid);
        //    m_cfgLine[(int)E_SELECT_LINE.LINE_IFC1].StartP = new Point(ifcMid, frontTopMid);              // Separation - IFC-1 / IFC-2
        //    m_cfgLine[(int)E_SELECT_LINE.LINE_IFC1].EndP = new Point(ifcMid, frontTopBottom);
        //    m_cfgLine[(int)E_SELECT_LINE.LINE_ISWT1].StartP = new Point(swtLeft, frontTopMid);            // Separation - IFC-1 / iSWT
        //    m_cfgLine[(int)E_SELECT_LINE.LINE_ISWT1].EndP = new Point(swtLeft, frontTopBottom);

        //    m_devRect = new RectPos[WT_CIRCLE_COUNT];

        //    m_devRect[0].StartP = new Point(20, 70);
        //    m_devRect[0].Width = 15;
        //    m_devRect[0].Height = 15;

        //    m_devRect[1].StartP = new Point(20, frontTopBottom - 31);
        //    m_devRect[1].Width = 15;
        //    m_devRect[1].Height = 15;

        //    m_devRect[2].StartP = new Point(DevImagePanel.Width - 31, 70);
        //    m_devRect[2].Width = 15;
        //    m_devRect[2].Height = 15;

        //    m_devRect[3].StartP = new Point(DevImagePanel.Width - 31, frontTopBottom - 31);
        //    m_devRect[3].Width = 15;
        //    m_devRect[3].Height = 15;

        //    m_devRect[4].StartP = new Point(innerLeft + (innerWidth * 13) / innerFullScale, frontBottom - 32);
        //    m_devRect[4].Width = 10;
        //    m_devRect[4].Height = 10;

        //    m_devRect[5].StartP = new Point(innerLeft + (innerWidth * 111) / innerFullScale, frontBottom - 37);
        //    m_devRect[5].Width = 10;
        //    m_devRect[5].Height = 10;

        //    m_devRect[6].StartP = new Point(innerLeft + (innerWidth * 131) / innerFullScale, frontBottom - 27);
        //    m_devRect[6].Width = 10;
        //    m_devRect[6].Height = 10;

        //    m_devRect[7].StartP = new Point(innerLeft + (innerWidth * 151) / innerFullScale, frontBottom - 37);
        //    m_devRect[7].Width = 10;
        //    m_devRect[7].Height = 10;

        //    m_devRect[8].StartP = new Point(innerLeft + (innerWidth * 171) / innerFullScale, frontBottom - 27);
        //    m_devRect[8].Width = 10;
        //    m_devRect[8].Height = 10;

        //    // Name position
        //    m_namePos[(int)E_SELECT_LINE.LINE_PS1] = new Point((pspaMid + innerLeft) / 2, (frontTop + frontTopMid) / 2);                                 // PS-1
        //    m_namePos[(int)E_SELECT_LINE.LINE_PS2] = new Point((pspaMid + alrRight) / 2, (frontTop + frontTopMid) / 2);                                  // PSCFS
        //    m_namePos[(int)E_SELECT_LINE.LINE_ISWT1] = new Point((swtLeft + swtRight) / 2, (frontTopBottom + frontTopMid) / 2);                          // iSWT
        //    m_namePos[(int)E_SELECT_LINE.LINE_IFC1] = new Point((ifcMid + swtLeft) / 2, (frontTopBottom + frontTopMid) / 2);                             // IFC-1
        //    m_namePos[(int)E_SELECT_LINE.LINE_IFC2] = new Point((ifcMid + innerLeft) / 2, (frontTopBottom + frontTopMid) / 2);                           // IFC-2
        //    m_namePos[(int)E_SELECT_LINE.LINE_ALR] = new Point((swtRight + alrRight) / 2, (frontTopBottom + frontTopMid) / 2);                           // ALR

        //    m_namePos[(int)E_SELECT_LINE.LINE_CSPI] = new Point((alrRight + CspiRight) / 2, frontTopMid);                                                // Cspi
        //    m_namePos[(int)E_SELECT_LINE.LINE_VMUX] = new Point((VmuxLeft + AmfLeft) / 2, frontTopMid);                                                  // Vmux
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX1_1] = new Point((CspiRight + VfmMid) / 2, (frontTop + frontTopUpMid) / 2);                             // Vfx1_P1
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX1_2] = new Point((CspiRight + VfmMid) / 2, (frontTopUpMid + frontTopMid) / 2);                          // Vfx1_P2
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX1_3] = new Point((CspiRight + VfmMid) / 2, (frontTopMid + frontTopDownMid) / 2);                        // Vfx1_P3
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX1_4] = new Point((CspiRight + VfmMid) / 2, (frontTopDownMid + frontTopBottom) / 2);                     // Vfx1_P4
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX2_1] = new Point((VfmMid + VmuxLeft) / 2, (frontTop + frontTopUpMid) / 2);                              // Vfx2_P1
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX2_2] = new Point((VfmMid + VmuxLeft) / 2, (frontTopUpMid + frontTopMid) / 2);                           // Vfx2_P2
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX2_3] = new Point((VfmMid + VmuxLeft) / 2, (frontTopMid + frontTopDownMid) / 2);                         // Vfx2_P3
        //    m_namePos[(int)E_SELECT_LINE.LINE_AMF] = new Point((AmfLeft + AmfRight) / 2, (frontTop + frontTopMid) / 2);                                  // AMF
        //    m_namePos[(int)E_SELECT_LINE.LINE_LT] = new Point((LtLeft + innerRight) / 2, (frontTop + frontTopMid) / 2);                                  // LT
        //    m_namePos[(int)E_SELECT_LINE.LINE_RXF] = new Point((AmfLeft + AmfRight) / 2, (frontTopBottom + frontTopMid) / 2);                            // RXF
        //    m_namePos[(int)E_SELECT_LINE.LINE_TXF] = new Point((LtLeft + innerRight) / 2, (frontTopBottom + frontTopMid) / 2);                           // TXF
        //    m_namePos[(int)E_SELECT_LINE.LINE_RF] = new Point(innerLeft + (13 + 111) / 2, (frontBottomDownMid + frontBottom) / 2);                       // RF 
        //    m_namePos[(int)E_SELECT_LINE.LINE_SL] = new Point((RfRight + SlRight) / 2, frontBottomTop + frontBottomUp3 / 4);                             // SL
        //    m_namePos[(int)E_SELECT_LINE.LINE_FE1] = new Point((RfRight + SlRight) / 2, frontBottomTop + frontBottomUp3 * 3 / 4);                        // FE1
        //    m_namePos[(int)E_SELECT_LINE.LINE_X21_1] = new Point((SlRight + X211Right) / 2, (frontBottomTop + frontBottomUpMid) / 2);                    // X21-1
        //    m_namePos[(int)E_SELECT_LINE.LINE_X21_2] = new Point((SlRight + X211Right) / 2, (frontBottomUpMid + frontBottomMid) / 2);                    // X21-2
        //    m_namePos[(int)E_SELECT_LINE.LINE_X21_DP] = new Point((SlRight + X211Right) / 2, (frontBottomMid + frontBottomDownMid) / 2);                 // X21-DP
        //    m_namePos[(int)E_SELECT_LINE.LINE_RM1] = new Point((SlRight + X211Right) / 2, (frontBottomDownMid + frontBottom) / 2);                       // RM1
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX1_P13] = new Point((X211Right + Vfx1Right) / 2, frontBottomTop + frontBottomUp3 / 2);                   // VFX1_P13
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX1_P4] = new Point((X211Right + Vfx1Right) / 2, frontBottomMid);                                         // VFX1_P4
        //    m_namePos[(int)E_SELECT_LINE.LINE_VFX2] = new Point((Vfx1Right + Vfx2Right) / 2, frontBottomTop + frontBottomUp3 / 2);                       // VFX2
        //    m_namePos[(int)E_SELECT_LINE.LINE_RS232_12] = new Point((Vfx2Right + Rs232_12Right) / 2, frontBottomTop + frontBottomUp3 / 2);               // RS232_12
        //    m_namePos[(int)E_SELECT_LINE.LINE_RS232_34] = new Point((Vfx2Right + Rs232_12Right) / 2, frontBottomMid);                                    // RS232_34
        //    m_namePos[(int)E_SELECT_LINE.LINE_RS232_58] = new Point((Rs232_12Right + Rs232_58Right) / 2, frontBottomTop + frontBottomUp3 / 2);           // RS232_58
        //    m_namePos[(int)E_SELECT_LINE.LINE_ALARM] = new Point((Rs232_12Right + Rs232_58Right) / 2, (frontBottomMid + frontBottom) / 2);               // Alarm
        //    m_namePos[(int)E_SELECT_LINE.LINE_POWERSUPPLY] = new Point((PsLeft + innerRight) / 2, (frontBottomUpMid + frontBottomDownMid) / 2);          // PowerSupply

        //}

        //private void ShowDevGraphData()
        //{
        //    bool isAc = false;
        //    //bool isAmpFull = false;
        //    bool isIec = false;             // true = EN100 IEC61850 configured
        //    string str;
        //    m_intTp = 0;
        //    m_intData = 0;
        //    m_intDataPorts = 0;
        //    m_name = new string[(int)E_SELECT_LINE.LINE_MAX];

        //    m_isLineEna[(int)E_SELECT_LINE.LINE_CSPI] = true;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_PS2] = true;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_AMF] = true;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_RXF] = true;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_LT] = true;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_TXF] = true;
        //    m_name[(int)E_SELECT_LINE.LINE_CSPI] = ConfigText.Cspi;
        //    m_name[(int)E_SELECT_LINE.LINE_PS2] = ConfigText.Pscf;
        //    m_name[(int)E_SELECT_LINE.LINE_AMF] = ConfigText.Amf;
        //    m_name[(int)E_SELECT_LINE.LINE_RXF] = ConfigText.Rxf;
        //    m_name[(int)E_SELECT_LINE.LINE_LT] = ConfigText.Lt;
        //    m_name[(int)E_SELECT_LINE.LINE_TXF] = ConfigText.Txf;

        //    m_isLineEna[(int)E_SELECT_LINE.LINE_RF] = true;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_POWERSUPPLY] = true;

        //    m_name[(int)E_SELECT_LINE.LINE_RF] = ConfigText.Rf;
        //    m_name[(int)E_SELECT_LINE.LINE_POWERSUPPLY] = ConfigText.PowerSupply;

        //    // Power Supply
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_PS1] = true;
        //    isAc = m_basicCfg.PsTypeValue.GetValue.Contains(BasicChoices.POWER_SUPPLY_AC);
        //    //isAmpFull_Half = m_basicCfg.AmpPwrValue.GetValue.Equals(BasicChoices.FRAME_FULL_AMP_HALF);
        //    if (isAc)
        //    {
        //        m_name[(int)E_SELECT_LINE.LINE_PS1] = ConfigText.PsAc;
        //    }
        //    else
        //    {
        //        m_name[(int)E_SELECT_LINE.LINE_PS1] = ConfigText.PsDc;
        //    }

        //    // alarm 
        //    if (m_basicCfg.Alarm3Value.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_ALR] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_ALARM] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_ALR] = ConfigText.Alr;
        //        m_name[(int)E_SELECT_LINE.LINE_ALARM] = ConfigText.AlarmH;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_ALR] = false;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_ALARM] = false;
        //    }

        //    //vmux
        //    if (m_basicCfg.VmuxValue.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VMUX] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VMUX] = ConfigText.Vmux;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VMUX] = false;
        //    }

        //    // iSWT A
        //    if (m_basicCfg.IswtChoiceValue.GetValue && m_basicCfg.IswtAValue.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_ISWT1] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_ISWT1] = ConfigText.Iswt;
        //        if (m_basicCfg.FomAValue.GetValue)
        //        {
        //            if (m_basicCfg.FosAValue.GetValue == BasicChoices.SWT_FOM_SM)
        //            {
        //                m_name[(int)E_SELECT_LINE.LINE_ISWT1] = ConfigText.Fos1;
        //            }
        //            else
        //            {
        //                m_name[(int)E_SELECT_LINE.LINE_ISWT1] = ConfigText.Fos2;
        //            }
        //        }
        //        else
        //        {
        //            str = m_basicCfg.IswtAIec61850Value.GetValue;
        //            if (str.Equals(BasicChoices.SWT_EN100_E))
        //            {
        //                // EN100 occupies the original position of IFC-1
        //                isIec = true;
        //                m_isLineEna[(int)E_SELECT_LINE.LINE_IFC1] = true;
        //                m_name[(int)E_SELECT_LINE.LINE_IFC1] = ConfigText.En100E;
        //            }
        //            else if (str.Equals(BasicChoices.SWT_EN100_O))
        //            {
        //                isIec = true;
        //                m_isLineEna[(int)E_SELECT_LINE.LINE_IFC1] = true;
        //                m_name[(int)E_SELECT_LINE.LINE_IFC1] = ConfigText.En100O;
        //            }
        //            else
        //            {
        //                isIec = false;
        //            }

        //            if (isIec)
        //            {
        //                // IFC-1 now is at the position of IFC-2
        //                str = m_basicCfg.IswtAIfcSlot1Value.GetValue;
        //                if (str.Equals(BasicChoices.SWT_IFC_D))
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = true;
        //                    m_name[(int)E_SELECT_LINE.LINE_IFC2] = ConfigText.IFCD;
        //                }
        //                else if (str.Equals(BasicChoices.SWT_IFC_P))
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = true;
        //                    m_name[(int)E_SELECT_LINE.LINE_IFC2] = ConfigText.IFCP;
        //                }
        //                else
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = false;
        //                }
        //            }
        //            else
        //            {
        //                // IFC-1
        //                str = m_basicCfg.IswtAIfcSlot1Value.GetValue;
        //                if (str.Equals(BasicChoices.SWT_IFC_D))
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC1] = true;
        //                    m_name[(int)E_SELECT_LINE.LINE_IFC1] = ConfigText.IFCD;
        //                }
        //                else if (str.Equals(BasicChoices.SWT_IFC_P))
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC1] = true;
        //                    m_name[(int)E_SELECT_LINE.LINE_IFC1] = ConfigText.IFCP;
        //                }
        //                else
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC1] = false;
        //                }

        //                // IFC-2
        //                str = m_basicCfg.IswtAIfcSlot2Value.GetValue;
        //                if (str.Equals(BasicChoices.SWT_IFC_D))
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = true;
        //                    m_name[(int)E_SELECT_LINE.LINE_IFC2] = ConfigText.IFCD;
        //                }
        //                else if (str.Equals(BasicChoices.SWT_IFC_P))
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = true;
        //                    m_name[(int)E_SELECT_LINE.LINE_IFC2] = ConfigText.IFCP;
        //                }
        //                else if (str.Equals(BasicChoices.SWT_IFC_S))
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = true;
        //                    m_name[(int)E_SELECT_LINE.LINE_IFC2] = ConfigText.IFCS;
        //                }
        //                else
        //                {
        //                    m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = false;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_ISWT1] = false;
        //        m_name[(int)E_SELECT_LINE.LINE_ISWT1] = "";
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_IFC1] = false;
        //        m_name[(int)E_SELECT_LINE.LINE_IFC1] = "";
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_IFC2] = false;
        //        m_name[(int)E_SELECT_LINE.LINE_IFC2] = "";
        //    }


        //    // VFM
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = false;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = false;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_3] = false;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_4] = false;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = false;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = false;
        //    m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_3] = false;

        //    if (m_intEM == 1)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Em;
        //    }
        //    else if (m_intEM == 2)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Em;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //    }
        //    else if (m_intEM == 3)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Em;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Em;
        //    }
        //    else if (m_intEM == 4)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Em;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Em;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX2_2] = ConfigText.Em;
        //    }

        //    if (m_intFXS == 1)
        //    {
        //        if (m_intEM == 0)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxs;
        //        }
        //        else if (m_intEM == 1)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //        }
        //        else if (m_intEM == 2)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxs;
        //        }
        //        else if (m_intEM == 3)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_2] = ConfigText.Em;
        //        }
        //    }

        //    if (m_intFXS == 2)
        //    {
        //        if (m_intEM == 0)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxs;
        //        }
        //        else if (m_intEM == 1)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //        }
        //        else if (m_intEM == 2)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_2] = ConfigText.Em;
        //        }
        //    }

        //    if (m_intFXO == 1)
        //    {
        //        if (m_intEM == 0 && m_intFXS == 0)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxo;
        //        }
        //        else if (m_intEM == 1 && m_intFXS == 0)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //        }
        //        else if (m_intEM == 2 && m_intFXS == 0)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxo;
        //        }
        //        else if (m_intEM == 3 && m_intFXS == 0)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_2] = ConfigText.Em;
        //        }
        //        else if ((m_intEM == 0 || m_intEM == 1) && m_intFXS == 1)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxo;
        //        }
        //        else if (m_intEM == 2 && m_intFXS == 1)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxs;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_2] = ConfigText.Em;
        //        }
        //    }

        //    if (m_intFXO == 2)
        //    {
        //        if (m_intEM == 0)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxo;
        //        }
        //        else if (m_intEM == 1)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //        }
        //        else if (m_intEM == 2)
        //        {
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] = true;
        //            m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = true;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_1] = ConfigText.Fxo;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Em;
        //            m_name[(int)E_SELECT_LINE.LINE_VFX2_2] = ConfigText.Em;
        //        }
        //    }

        //    if (m_basicCfg.Telep1Value.GetValue)
        //    {
        //        m_intTp = m_intTp + 1;
        //    }
        //    if (m_basicCfg.Telep2Value.GetValue)
        //    {
        //        m_intTp = m_intTp + 1;
        //    }
        //    if (m_intTp == 1)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_3] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_3] = ConfigText.Tp;
        //    }
        //    else if (m_intTp == 2)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_3] = true;
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_4] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_3] = ConfigText.Tp;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_4] = ConfigText.Tp;
        //    }

        //    if (m_basicCfg.Data1Value.GetValue)
        //    {
        //        m_intData = m_intData + 1;
        //        if (m_basicCfg.Data1PortValue.GetValue == BasicChoices.VF_F3_PORT_1)
        //        {
        //            m_intDataPorts = m_intDataPorts + 1;
        //        }
        //        if (m_basicCfg.Data1PortValue.GetValue == BasicChoices.VF_F3_PORT_2)
        //        {
        //            m_intDataPorts = m_intDataPorts + 2;
        //        }
        //        if (m_basicCfg.Data1PortValue.GetValue == BasicChoices.VF_F3_PORT_3)
        //        {
        //            m_intDataPorts = m_intDataPorts + 3;
        //        }
        //    }
        //    if (IfskSum1TextBox.Text != "" || VmIfskSum1TextBox.Text != "")
        //    {
        //        m_intData = m_intData + 1;
        //    }
        //    if (m_basicCfg.Data2Value.GetValue)
        //    {
        //        m_intData = m_intData + 1;
        //        if (m_basicCfg.Data2PortValue.GetValue == BasicChoices.VF_F3_PORT_1)
        //        {
        //            m_intDataPorts = m_intDataPorts + 1;
        //        }
        //        if (m_basicCfg.Data2PortValue.GetValue == BasicChoices.VF_F3_PORT_2)
        //        {
        //            m_intDataPorts = m_intDataPorts + 2;
        //        }
        //    }
        //    if (IfskSum2TextBox.Text != "" || VmIfskSum2TextBox.Text != "")
        //    {
        //        m_intData = m_intData + 1;
        //    }

        //    if (m_intDataPorts == 0)
        //    {
        //        FuncPorts();
        //        goto BREAK;
        //    }

        //    if (!m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_2] = ConfigText.Data;
        //        m_intDataPorts = m_intDataPorts - 1;
        //        if (m_intDataPorts == 0)
        //        {
        //            FuncPorts();
        //            goto BREAK;
        //        }
        //    }
        //    if (!m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_3])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_3] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_3] = ConfigText.Data;
        //        m_intDataPorts = m_intDataPorts - 1;
        //        if (m_intDataPorts == 0)
        //        {
        //            FuncPorts();
        //            goto BREAK;
        //        }
        //    }
        //    if (!m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_4])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_4] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_4] = ConfigText.Data;
        //        m_intDataPorts = m_intDataPorts - 1;
        //        if (m_intDataPorts == 0)
        //        {
        //            FuncPorts();
        //            goto BREAK;
        //        }
        //    }
        //    if (!m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX2_2] = ConfigText.Data;
        //        m_intDataPorts = m_intDataPorts - 1;
        //        if (m_intDataPorts == 0)
        //        {
        //            FuncPorts();
        //            goto BREAK;
        //        }
        //    }
        //    if (!m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_3])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_3] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX2_3] = ConfigText.Data;
        //        m_intDataPorts = m_intDataPorts - 1;
        //        if (m_intDataPorts == 0)
        //        {
        //            FuncPorts();
        //            goto BREAK;
        //        }
        //    }

        //BREAK:

        //    if (m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_1] || m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_2]
        //        || m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_3])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_P13] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_P13] = ConfigText.Vfx1_P13;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_P13] = false;
        //    }

        //    if (m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_4])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_P4] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX1_P4] = ConfigText.Vfx1_P4;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX1_P4] = false;
        //    }

        //    if (m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_1] || m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_2]
        //        || m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2_3])
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_VFX2] = ConfigText.Vfx2;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_VFX2] = false;
        //    }

        //    // SL
        //    if ((m_basicCfg.Voice4Value.GetValue && m_basicCfg.Voice4InfaValue.GetValue == BasicChoices.VOICE_SL)
        //        || (m_basicCfg.Voice5Value.GetValue && m_basicCfg.Voice5InfaValue.GetValue == BasicChoices.VOICE_SL)
        //        || (m_basicCfg.Voice6Value.GetValue && m_basicCfg.Voice6InfaValue.GetValue == BasicChoices.VOICE_SL)
        //        || (m_basicCfg.Voice7Value.GetValue && m_basicCfg.Voice7InfaValue.GetValue == BasicChoices.VOICE_SL)
        //        || (m_basicCfg.Voice8Value.GetValue && m_basicCfg.Voice8InfaValue.GetValue == BasicChoices.VOICE_SL))
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_SL] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_SL] = ConfigText.Sl;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_SL] = false;
        //    }

        //    // FE1
        //    if ((m_basicCfg.Voice4Value.GetValue && m_basicCfg.Voice4InfaValue.GetValue == BasicChoices.VOICE_FE1)
        //       || (m_basicCfg.Voice5Value.GetValue && m_basicCfg.Voice5InfaValue.GetValue == BasicChoices.VOICE_FE1)
        //       || (m_basicCfg.Voice6Value.GetValue && m_basicCfg.Voice6InfaValue.GetValue == BasicChoices.VOICE_FE1)
        //       || (m_basicCfg.Voice7Value.GetValue && m_basicCfg.Voice7InfaValue.GetValue == BasicChoices.VOICE_FE1)
        //       || (m_basicCfg.Voice8Value.GetValue && m_basicCfg.Voice8InfaValue.GetValue == BasicChoices.VOICE_FE1))
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_FE1] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_FE1] = ConfigText.Fe1;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_FE1] = false;
        //    }

        //    // RM1
        //    if (m_basicCfg.RmValue.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RM1] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_RM1] = ConfigText.Rm1;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RM1] = false;
        //    }

        //    //X21-1\-2\-DP
        //    if (m_basicCfg.X21Value.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_X21_DP] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_X21_DP] = ConfigText.X21_DpH;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_X21_DP] = false;
        //    }

        //    if (m_basicCfg.X211Value.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_X21_1] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_X21_1] = ConfigText.X21_1H;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_X21_1] = false;
        //    }

        //    if (m_basicCfg.X212Value.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_X21_2] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_X21_2] = ConfigText.X21_2H;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_X21_2] = false;
        //    }

        //    //RS232
        //    if (m_basicCfg.Rs2321Value.GetValue || m_basicCfg.Rs2322Value.GetValue
        //        || m_basicCfg.VmRs2321Value.GetValue || m_basicCfg.VmRs2322Value.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RS232_12] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_RS232_12] = ConfigText.Rs232_12H;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RS232_12] = false;
        //    }

        //    if (m_basicCfg.Rs2323Value.GetValue || m_basicCfg.Rs2324Value.GetValue
        //        || m_basicCfg.VmRs2323Value.GetValue || m_basicCfg.VmRs2324Value.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RS232_34] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_RS232_34] = ConfigText.Rs232_34H;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RS232_34] = false;
        //    }

        //    if (m_basicCfg.Rs2325Value.GetValue || m_basicCfg.Rs2326Value.GetValue
        //        || m_basicCfg.Rs2327Value.GetValue || m_basicCfg.Rs2328Value.GetValue
        //        || m_basicCfg.VmRs2325Value.GetValue || m_basicCfg.VmRs2326Value.GetValue
        //        || m_basicCfg.VmRs2327Value.GetValue || m_basicCfg.VmRs2328Value.GetValue)
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RS232_58] = true;
        //        m_name[(int)E_SELECT_LINE.LINE_RS232_58] = ConfigText.Rs232_58H;
        //    }
        //    else
        //    {
        //        m_isLineEna[(int)E_SELECT_LINE.LINE_RS232_58] = false;
        //    }
        //}