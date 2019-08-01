using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;

namespace WindowsFormsApplication3.Model
{

    public class SubNetwork
    {
        public SubNetwork()
        {
            
            for (int i = 0; i < 100; i++)
            {
                IEDName[i] = "";
                SwitchName[i] = "";
                ODFName[i] = "";
                for (int j = 0; j < 10; j++)
                {
                    Phys[i, j] = new Dictionary<string, string>();
                }
            }
        }

        public string SubNetName { get; set; } = "";
        public string[] IEDName { get; set; } = new string[100];
        public Dictionary <string, string>[,] Phys{get; set; } = new Dictionary<string, string>[100, 10];
        public string[] SwitchName { get; set; }= new string[100];
        public string[] ODFName{ get; set; }= new string[100];
    }

    public class IedComm
    {
        public IedComm()
        {
        }
        public string IEDName { get; set; }
        public string CbName { get; set; }
        public string LdInst{ get; set; }
        public string CbAddr{ get; set; }
    }

    public class IedPhysComm
    {
        public IedPhysComm()
        {
            PhysAddr = new Dictionary<string, string>();
        }
        public string IEDName{ get; set; }
        public Dictionary<string, string> PhysAddr{ get; set; }
    }

        class SubNetValues
    {

        #region Constants and Enums
        /// <summary>
        /// XML nodes name 
        /// </summary>
        private const string XML_TEXT_NODE = "Text";
        private const string XML_BIT_NODE = "BitRate";
        private const string XML_CONN_NODE = "ConnectedAP";
        private const string XML_ADDR_NODE = "Address";
        private const string XML_PHYS_NODE = "PhysConn";
        private const string XML_GSE_NODE = "GSE";
        private const string XML_SMV_NODE = "SMV";
        private const string XML_P_NODE = "P";

        private const string XML_NAME_NODE = "name";
        private const string XML_TYPE_NODE = "type";
        private const string XML_UNIT_NODE = "unit";
        private const string XML_IED_NODE = "iedName";
        private const string XML_AP_NODE = "apName";
        private const string XML_LDINST_NODE = "ldInst";
        private const string XML_CB_NODE = "cbName";


        private const string NODE_IP_TYPE = "IP";
        private const string NODE_IPSUB_TYPE = "IP-SUBNET";
        private const string NODE_IPGAT_TYPE = "IP-GATEWAY";
        private const string NODE_TSEL_TYPE = "OSI-TSEL";
        private const string NODE_PSEL_TYPE = "OSI-PSEL";
        private const string NODE_SSEL_TYPE = "OSI-SSEL";
        private const string NODE_APT_TYPE = "OSI-AP-Title";
        private const string NODE_AEQ_TYPE = "OSI-AE-Qualifier";
        private const string NODE_CONN_TYPE = "Connection";
        private const string NODE_REDCONN_TYPE = "RedConn";
        private const string NODE_TYPE_TYPE = "type";
        private const string NODE_PLUG_TYPE = "Plug";
        private const string NODE_PORT_TYPE = "port";
        private const string NODE_CABLE_TYPE = "cable";
        /// <summary>
        /// This is a list of all optical settings.
        /// </summary>
        private readonly string[] SUBNET_NODE_NAMES = new string[]
        {
            XML_TEXT_NODE, XML_BIT_NODE, XML_CONN_NODE,
            XML_ADDR_NODE, XML_PHYS_NODE, XML_GSE_NODE, XML_SMV_NODE
        };
        private readonly string[] CONN_NODE_NAMES = new string[]
        {
            XML_ADDR_NODE, XML_PHYS_NODE, XML_GSE_NODE, XML_SMV_NODE
        };

        #endregion Constants and Enums
        #region Fields

        private readonly string m_Text = null;
        private readonly string m_Bit = null;
        private readonly string m_Conn = null;
        private readonly string m_Addr = null;
        private readonly string m_Phys = null;
        private readonly string m_Gse = null;
        private readonly string m_Smv = null;

        // shall match ConnectedAP_GROUP_COUNT
        public enum ConnType { Address, GSE, SMV }
        public const int CONN_GROUP_COUNT = (int)(ConnType.SMV);

        /// <summary>
        /// This dictionary contains all ChoiceNodes.
        /// </summary>
        private Dictionary<string, string> m_Nodes = new Dictionary<string, string>();
        private Dictionary<string, string> m_OldNodes = new Dictionary<string, string>();

        private Dictionary<string, string> m_ConnNodes = new Dictionary<string, string>();
        private readonly Dictionary<string, string>[,] m_AddrNodes = new Dictionary<string, string>[6,100];

        public List<SubNetwork> SubNetDevList = new List<SubNetwork>();

        #endregion Constants and Enums

        #region Construction/Destruction/Initialisation
        /// <summary>
        /// no parameters
        /// Constructuction function without parameter
        /// </summary>
        public SubNetValues()
        {
            for (int i = 0; i < 6; i++) // max 6 subNetwork
            {
                for (int j = 0; j < 100; j++) // max 100 connectedAP in a subNetwork
                {
                    m_AddrNodes[i, j] = new Dictionary<string, string>();
                    for (int k = 0; k < 10; k++) // max 10 physconn in a connectedAP
                        PhysNodes[i, j, k] = new Dictionary<string, string>();
                }

            }
            foreach (string name in SUBNET_NODE_NAMES)
            {
                switch (name)
                {
                    case XML_TEXT_NODE:
                        m_Text = "0";
                        m_Nodes.Add(name, m_Text);
                        break;
                    case XML_BIT_NODE:
                        m_Bit = "0";
                        m_Nodes.Add(name, m_Bit);
                        break;
                    case XML_CONN_NODE:
                        m_Conn = "0";
                        m_Nodes.Add(name, m_Conn);
                        //ConnValues();
                        break;
                    case XML_ADDR_NODE:
                        m_Addr = "0";
                        m_Nodes.Add(name, m_Addr);
                        m_ConnNodes.Add(name, m_Addr);
                        //ConnValues();
                        break;
                    case XML_PHYS_NODE:
                        m_Phys = "0";
                        m_Nodes.Add(name, m_Phys);
                        m_ConnNodes.Add(name, m_Phys);
                        //ConnValues();
                        break;
                    case XML_GSE_NODE:
                        m_Gse = "0";
                        m_Nodes.Add(name, m_Gse);
                        m_ConnNodes.Add(name, m_Gse);
                        //ConnValues();
                        break;
                    case XML_SMV_NODE:
                        m_Smv = "0";
                        m_Nodes.Add(name, m_Smv);
                        m_ConnNodes.Add(name, m_Smv);
                        //ConnValues();
                        break;
                }
            }

            foreach (KeyValuePair<string, string> NodeElement in m_Nodes)
            {
                string tempNode = NodeElement.Value;
                m_OldNodes.Add(NodeElement.Key, tempNode);
            }
        }

        //private void ConnValues()
        //{
        //    foreach (string name in CONN_NODE_NAMES)
        //    {
        //        switch (name)
        //        {
        //            case XML_ADDR_NODE:
        //                InitAddrData();
        //                break;
        //        }
        //    }
        //}

        //private void InitAddrData()
        //{
        //    //int idx = (int)(ConnType.Address);

        //    //foreach ()
        //}

        #endregion Construction/Destruction/Initialisation

        #region Public Methods

        public void LoadValues(XmlNode node, int idx)
        {
            int connNum = 0;
            Dictionary<string, string> TempNodes = m_Nodes;
            SubNetData[idx] = new SubNetwork();
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                CreateSubNetName(node, idx, SubNetData[idx]);
                foreach (KeyValuePair<string, string> nodeElement in TempNodes)
                {
                    string strTemp = node.ChildNodes[i].InnerText;
                    if (nodeElement.Key.Equals(node.ChildNodes[i].Name))
                    {
                        string tempNode = nodeElement.Value;
                        tempNode = strTemp;
                        if (nodeElement.Key.Equals(XML_CONN_NODE))
                        {
                            LoadConnData(node.ChildNodes[i], idx, connNum, SubNetData[idx]);
                            connNum++;
                        }
                        SubNetDevList.Add(SubNetData[idx]);
                        break;
                    }
                }
            }
            

            m_OldNodes.Clear();
            foreach (KeyValuePair<string, string> NodeElement in TempNodes)
            {
                string tempNode = NodeElement.Value;
                m_OldNodes.Add(NodeElement.Key, tempNode);
            }
        }

        private void CreateSubNetName(XmlNode node, int idx, SubNetwork subNet)
        {
            string name = node.Attributes[XML_NAME_NODE].Value;
            string typeName = node.Attributes[XML_TYPE_NODE].Value;

            SubNetName[idx] = name + ": " + typeName;
            subNet.SubNetName = name + ": " + typeName;
        }

        private void LoadConnData(XmlNode node, int subIdx, int idx, SubNetwork subNet)
        {
            int physIdx = 0;
            XmlDocument XmlDoc = new XmlDocument();
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(XmlDoc.NameTable);
            nsMgr.AddNamespace("ns", "http://www.iec.ch/61850/2003/SCL");
            foreach (XmlNode cbNode in node.ChildNodes) // GSE / SMV
            {
                IedComm iedCommData = new IedComm
                {
                    IEDName = node.Attributes[XML_IED_NODE].Value,
                    CbName = ((XmlElement)cbNode).GetAttribute("cbName"),
                    LdInst = ((XmlElement)cbNode).GetAttribute("ldInst")
                };

                if (iedCommData.CbName != "" && iedCommData.LdInst != "")
                {
                    XmlNode cbAddrNode = cbNode.SelectSingleNode("ns:Address", nsMgr);
                    iedCommData.CbAddr = cbNode.Name + "\n";
                    foreach (XmlNode pNode in cbAddrNode)
                    {
                        iedCommData.CbAddr += ((XmlElement)pNode).GetAttribute("type") 
                            + ":" + pNode.InnerText + ";" + "\n";
                    }
                    IedCommDataList.Add(iedCommData);
                }
            }

            foreach (XmlNode physNode in node.ChildNodes) // physConn
            {
                if (((XmlElement)physNode).GetAttribute("type") == "Connection" || ((XmlElement)physNode).GetAttribute("type") == "RedConn")
                {
                    IedPhysComm iedPhysComm = new IedPhysComm();
                    iedPhysComm.IEDName = node.Attributes[XML_IED_NODE].Value;
                    foreach (XmlNode p in physNode.ChildNodes)
                    {
                        string ty = p.Attributes["type"].Value.ToString();
                        iedPhysComm.PhysAddr[ty] = p.InnerText;
                    }
                    IedPhysCommDataList.Add(iedPhysComm);
                }
            }

            subNet.IEDName[idx] = node.Attributes[XML_IED_NODE].Value;

            IedName[subIdx, idx] = node.Attributes[XML_IED_NODE].Value;
            ApName[subIdx, idx] = node.Attributes[XML_AP_NODE].Value;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                foreach (KeyValuePair<string, string> nodeElement in m_ConnNodes)
                {
                    string strTemp = node.ChildNodes[i].InnerText;
                    if (nodeElement.Key.Equals(node.ChildNodes[i].Name))
                    {
                        string tempNode = nodeElement.Value;
                        tempNode = strTemp;
                        if (nodeElement.Key.Equals(XML_ADDR_NODE))
                        {
                            LoadAddrData(node.ChildNodes[i], subIdx, idx);
                        }
                        else if (nodeElement.Key.Equals(XML_PHYS_NODE))
                        {
                            LoadPhysData(node.ChildNodes[i], subIdx, idx, physIdx, subNet);
                            physIdx++;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// load address data.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="subIdx"></param>
        /// <param name="connIdx"></param>
        private void LoadAddrData(XmlNode node, int subIdx, int connIdx)
        {
            string strAttr = "";
            string strVal = "";     

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "P")
                {
                    strAttr = node.ChildNodes[i].Attributes["type"].Value;
                    if (!String.IsNullOrEmpty(strAttr))
                    {
                        strVal = node.ChildNodes[i].InnerText;
                        m_AddrNodes[subIdx, connIdx][strAttr] = strVal;
                    }
                }
                
            }
        }
        private void LoadPhysData(XmlNode node, int subIdx, int connIdx, int physIdx, SubNetwork subNet)
        {
            string strAttr = "";
            string strVal = "";

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "P")
                {
                    strAttr = node.ChildNodes[i].Attributes["type"].Value;
                    if (!String.IsNullOrEmpty(strAttr))
                    {
                        strVal = node.ChildNodes[i].InnerText;
                        int[] idx = new int[] { subIdx, connIdx };
                        PhysNodes[subIdx, connIdx, physIdx][strAttr] = strVal;
                        subNet.Phys[connIdx, physIdx][strAttr] = strVal;

                        if (strAttr == "Cable" && string.Equals(strVal.Substring(0, 6), "Switch"))
                        {
                            subNet.SwitchName[physIdx] = strVal.Substring(0, strVal.IndexOf(':'));
                        }
                        else if (strAttr == "Cable" && string.Equals(strVal.Substring(0, 3), "ODF"))
                        {
                            subNet.ODFName[physIdx] = strVal.Substring(0, strVal.IndexOf(':'));
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Properties

        public string[] SubNetName { get; set; } = new string[6];
        public string[,] IedName { get; set; } = new string[6, 100];
        public string[,] ApName { get; set; } = new string[6, 100];
        public Dictionary<string, string>[,,] PhysNodes { get; set; } = new Dictionary<string, string>[6, 100, 10];

        public List<IedComm> IedCommDataList { get; set; } = new List<IedComm>();
        public List<IedPhysComm> IedPhysCommDataList { get; set; } = new List<IedPhysComm>();

        public SubNetwork[] SubNetData { get; set; } = new SubNetwork[6];

        #endregion Properties
    }
}
