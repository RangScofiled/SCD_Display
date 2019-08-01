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
    class ProcessingConfig : IProcessingConfig
    {
        protected CommValues m_commData = new CommValues();
        protected IedValues m_IedData = new IedValues();

        #region XML for offer / order


        #endregion XML for offer / order

        void IProcessingConfig.SaveCfgFile(string fileName, string filePath)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(filePath);
            var root = XmlDoc.DocumentElement;

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(XmlDoc.NameTable);
            nsMgr.AddNamespace("ns", "http://www.iec.ch/61850/2003/SCL");
            string nsUrl = "http://www.iec.ch/61850/2003/SCL";

            XmlNode CommNode = root.SelectSingleNode("ns:Communication", nsMgr);
            XmlNodeList IedNodes = root.SelectNodes("ns:IED", nsMgr);

            foreach (VirLine virLine in m_IedData.GetAccValues().Virlines)
            {
                string[] str = virLine.ExtCB.Split('/');
                XmlNodeList ConnNodes = CommNode.SelectNodes("./ns:SubNetwork/ns:ConnectedAP", nsMgr);
                foreach (XmlNode connNode in ConnNodes)
                {
                    if (((XmlElement)connNode).GetAttribute("iedName") == str[0])
                    {
                        XmlNodeList GseNodes = connNode.SelectNodes("./ns:GSE", nsMgr);
                        XmlNodeList SmvNodes = connNode.SelectNodes("./ns:SMV", nsMgr);
                        bool rel = false;
                        foreach (XmlNode gseNode in GseNodes)
                        {
                            if (((XmlElement)gseNode).GetAttribute("cbName") == str[2] && ((XmlElement)gseNode).GetAttribute("ldInst") == str[1])
                            {
                                XmlNodeList PhyNodes = connNode.SelectNodes("./ns:PhysConn", nsMgr);
                                foreach (XmlNode phyNode in PhyNodes)
                                {
                                    foreach (XmlNode pNode in phyNode.ChildNodes)
                                    {
                                        if (((XmlElement)pNode).GetAttribute("type") == "Cable")
                                        {
                                            if (pNode.InnerText == virLine.ExtCB.Substring(0, virLine.ExtCB.IndexOf('/')) + ":" + virLine.ExtPort)
                                            {
                                                rel = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        foreach (XmlNode smvNode in SmvNodes)
                        {
                            if (((XmlElement)smvNode).GetAttribute("cbName") == str[2] && ((XmlElement)smvNode).GetAttribute("ldInst") == str[1])
                            {
                                XmlNodeList PhyNodes = connNode.SelectNodes("./ns:PhysConn", nsMgr);
                                foreach (XmlNode phyNode in PhyNodes)
                                {
                                    foreach (XmlNode pNode in phyNode.ChildNodes)
                                    {
                                        if (((XmlElement)pNode).GetAttribute("type") == "Cable")
                                        {
                                            if (pNode.InnerText == virLine.ExtCB.Substring(0, virLine.ExtCB.IndexOf('/')) + ":" + virLine.ExtPort)
                                            {
                                                rel = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!rel)
                        {
                            XmlElement newPhys = XmlDoc.CreateElement("PhysConn", nsUrl);
                            //XmlAttribute physAtt = XmlDoc.CreateAttribute("type");
                            //physAtt.InnerText = "RedConn";
                            newPhys.SetAttribute("type", "RedConn");
                            XmlElement physChild1 = XmlDoc.CreateElement("P", nsUrl);
                            physChild1.SetAttribute("type", "Plug");
                            physChild1.InnerText = "ST";
                            XmlElement physChild2 = XmlDoc.CreateElement("P", nsUrl);
                            physChild2.SetAttribute("type", "Port");
                            physChild2.InnerText = virLine.InPort;
                            XmlElement physChild3 = XmlDoc.CreateElement("P", nsUrl);
                            physChild3.SetAttribute("type", "Type");
                            physChild3.InnerText = "FOC";
                            XmlElement physChild4 = XmlDoc.CreateElement("P", nsUrl);
                            physChild4.SetAttribute("type", "Cable");
                            physChild4.InnerText = virLine.InDesc.Substring(virLine.InDesc.IndexOf(':') + 1, virLine.InDesc.IndexOf('/') - virLine.InDesc.IndexOf(':') - 1) + ":" + virLine.InPort;
                            newPhys.AppendChild(physChild1);
                            newPhys.AppendChild(physChild2);
                            newPhys.AppendChild(physChild3);
                            newPhys.AppendChild(physChild4);
                            connNode.AppendChild(newPhys);
                            XmlNodeList phys = connNode.SelectNodes("./ns:PhysConn", nsMgr);
                            for (int i = 0; i < phys.Count; i++)
                            {
                                for (int j = i + 1; j < phys.Count; j++)
                                {
                                    if (phys[j].InnerText == phys[i].InnerText)
                                    {
                                        ((XmlElement)connNode).RemoveChild(phys[j]);
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            XmlDoc.Save(fileName);
            
        }

        #region IProcessingGeneInfo Members

        /// <summary>
        /// Read Configuration data from XML file and store them into memory
        /// </summary>
        /// <param name="fileName"></param>
        void IProcessingConfig.LoadCfgFile(string fileName)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(fileName);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(XmlDoc.NameTable);
            nsMgr.AddNamespace("ns", "http://www.iec.ch/61850/2003/SCL");
            XmlNode RootNode = XmlDoc.DocumentElement;

            XmlNode CommNode = RootNode.SelectSingleNode("ns:Communication",nsMgr);
            XmlNodeList IedNodes = RootNode.SelectNodes("ns:IED", nsMgr);

            try
            {
                int num = CommNode.ChildNodes.Count;
                for (int i = 0; i < num; i++)
                {
                    XmlNode node = CommNode.ChildNodes[i];// level 2
                    LoadSubNetData(node, i);
                }
                foreach (XmlElement IedElement in IedNodes)
                {
                    string str = IedElement.GetAttribute("desc") + ":" + IedElement.GetAttribute("name");
                    XmlNodeList nodes = IedElement.SelectNodes("ns:AccessPoint", nsMgr);

                    foreach (XmlElement e in nodes)
                    {
                        LoadAccData(e, str, IedNodes);
                    }
                }
            }
            catch
            {
            }
        }

        private void LoadSubNetData(XmlNode Node, int idx)
        {
            SubNetValues SubNetInfo = m_commData.GetSubNetValues();
            SubNetInfo.LoadValues(Node, idx);
        }

        private void LoadAccData(XmlNode Node, string IedDesc, XmlNodeList IedNodes)
        {
            AccValues AccInfo = m_IedData.GetAccValues();
            AccInfo.LoadValues(Node, IedDesc, IedNodes);
        }

        /// <summary>
        /// Return CommValues object in this object
        /// </summary>
        /// <returns></returns>
        CommValues IProcessingConfig.CommCfgSet()
        {
            return m_commData;
        }

        IedValues IProcessingConfig.IedCfgSet()
        {
            return m_IedData;
        }

        #endregion IProcessingGeneInfo Members
    }
}
