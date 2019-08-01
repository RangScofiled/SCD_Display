using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace WindowsFormsApplication3.Model
{

    public class VirLine
    {
        public VirLine() { }
        public string InPort { get; set; }
        public string IntAddr { get; set; }
        public string InDesc { get; set; }
        public string ExtPort { get; set; }
        public string ExtAddr { get; set; }
        public string ExtDesc { get; set; }
        public string ExtCB { get; set; }
    }

    public class DsRelayEna
    {
        public DsRelayEna()
        {
        }
        public string DsName { get; set; }
        public string DsData { get; set; }
        public string DsDataDesc { get; set; }

    }

    class AccValues
    {
        private readonly string[] LdName = null;
        private readonly XmlNode[] LnNode = null;

        private XmlNodeList LDNodes = null;
        private XmlNodeList InputsNodes = null;
        private XmlNodeList LNNodes = null;
        private XmlNodeList ExtLDNodes = null;
        private XmlNodeList ExtLN0Nodes = null;
        private XmlNodeList ExtLNNodes = null;
        private XmlNodeList ExtFcdaNodes = null;
        public List<VirLine> Virlines { get; set; } = new List<VirLine>();
        public List<DsRelayEna> DsRelayEnas { get; set; } = new List<DsRelayEna>();

        public AccValues()
        {
            LdName = new string[10];
            LnNode = new XmlNode[10];
        }

        public void LoadValues(XmlNode node, string iedDesc, XmlNodeList iedNodes)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(XmlDoc.NameTable);
            nsMgr.AddNamespace("ns", "http://www.iec.ch/61850/2003/SCL");

            // select LN nodes.
            LNNodes = node.SelectNodes("./ns:Server/ns:LDevice/ns:LN", nsMgr);
            // select extref nodes of all ieds.
            InputsNodes = node.SelectNodes("./ns:Server/ns:LDevice/ns:LN0/ns:Inputs/ns:ExtRef", nsMgr);
            
            //Virlines = new List<VirLine>();

            for (int i = 0; i < InputsNodes.Count; i++)
            {
                VirLine virLine = new VirLine();
                XmlElement ele = (XmlElement)InputsNodes[i];
                if (ele.GetAttribute("intAddr").IndexOf(':') > 0)
                {
                    virLine.InPort = ele.GetAttribute("intAddr").Substring(0, ele.GetAttribute("intAddr").IndexOf(':'));
                }
                else
                {
                    virLine.InPort = "X-X";
                }
                virLine.ExtPort = "X-X";
                virLine.IntAddr = ele.GetAttribute("intAddr");
                virLine.ExtAddr = ele.GetAttribute("iedName") + ele.GetAttribute("ldInst") + "/" + ele.GetAttribute("prefix")
                    + ele.GetAttribute("lnClass") + ele.GetAttribute("lnInst") + "." + ele.GetAttribute("doName") + "." 
                    + ele.GetAttribute("daName");
                virLine.InDesc = iedDesc + "%";
                for (int j = 0; j < LNNodes.Count; j++)
                {
                    XmlElement lnEle = (XmlElement)LNNodes[j];
                    if (virLine.IntAddr.Contains(lnEle.GetAttribute("prefix") + lnEle.GetAttribute("lnClass") + lnEle.GetAttribute("inst")))
                    {
                        virLine.InDesc += lnEle.GetAttribute("desc");

                        XmlNodeList list = LNNodes[j].ChildNodes;
                        foreach (XmlNode nd in list)
                        {
                            string str = ((XmlElement)nd).GetAttribute("name");
                            if (virLine.IntAddr.Contains(str))
                            {
                                virLine.InDesc += "%" + ((XmlElement)nd).GetAttribute("desc");
                                break;
                            }
                        }
                        break;
                    }
                }

                // virLine.ExtDesc & virLine.ExtCB
                foreach (XmlNode iedNd in iedNodes)
                {
                    string iedName = ((XmlElement)iedNd).GetAttribute("name");
                    if (iedName == ele.GetAttribute("iedName"))
                    {
                        string ExtIedDesc = ((XmlElement)iedNd).GetAttribute("desc") + ":" + iedName + "%";
                        // select ext LD of ext ied.
                        ExtLDNodes = iedNd.SelectNodes("./ns:AccessPoint/ns:Server/ns:LDevice", nsMgr);
                        foreach (XmlNode extLdNode in ExtLDNodes)
                        {
                            if (ele.GetAttribute("ldInst") == extLdNode.Attributes["inst"].Value)
                            {
                                ExtLN0Nodes = extLdNode.SelectNodes("ns:LN0", nsMgr);
                                ExtLNNodes = extLdNode.SelectNodes("ns:LN", nsMgr);
                                break;
                            }
                        }
                        if (ele.GetAttribute("lnClass") == "LLN0")
                        {
                            foreach (XmlNode extLnNode in ExtLN0Nodes)
                            {
                                XmlElement extLnEle = (XmlElement)extLnNode;
                                if (virLine.ExtAddr.Contains(extLnEle.GetAttribute("prefix") + extLnEle.GetAttribute("lnClass") + extLnEle.GetAttribute("inst")))
                                {
                                    virLine.ExtDesc = ExtIedDesc + extLnEle.GetAttribute("desc");

                                    XmlNodeList cnds = extLnNode.ChildNodes;
                                    foreach (XmlNode cnd in cnds)
                                    {
                                        string str = ((XmlElement)cnd).GetAttribute("name");
                                        if (ele.GetAttribute("doName").Contains(str))
                                        {
                                            virLine.ExtDesc += "%" + ((XmlElement)cnd).GetAttribute("desc");
                                            break;
                                        }
                                    }
                                    // select ext fcda of ext ied.
                                    ExtFcdaNodes = extLnNode.ParentNode.SelectNodes("ns:LN0/ns:DataSet/ns:FCDA", nsMgr);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (XmlNode extLnNode in ExtLNNodes)
                            {
                                XmlElement extLnEle = (XmlElement)extLnNode;
                                if (virLine.ExtAddr.Contains(extLnEle.GetAttribute("prefix") + extLnEle.GetAttribute("lnClass") + extLnEle.GetAttribute("inst")))
                                {
                                    virLine.ExtDesc = ExtIedDesc + "%" + extLnEle.GetAttribute("desc");

                                    XmlNodeList cnds = extLnNode.ChildNodes;
                                    foreach (XmlNode cnd in cnds)
                                    {
                                        string str = ((XmlElement)cnd).GetAttribute("name");
                                        if (ele.GetAttribute("doName").Contains(str))
                                        {
                                            virLine.ExtDesc += "%" + ((XmlElement)cnd).GetAttribute("desc");
                                            break;
                                        }
                                    }
                                    // select ext fcda of ext ied.
                                    ExtFcdaNodes = extLnNode.ParentNode.SelectNodes("ns:LN0/ns:DataSet/ns:FCDA", nsMgr);
                                    break;
                                }
                            }
                        }
                        foreach(XmlNode extFcdaNode in ExtFcdaNodes)
                        {
                            XmlElement extFcdaEle = (XmlElement)extFcdaNode;
                            if (virLine.ExtAddr.Contains(extFcdaEle.GetAttribute("prefix") 
                                + extFcdaEle.GetAttribute("lnClass") + extFcdaEle.GetAttribute("lnInst")))
                            {
                                string dataset = extFcdaNode.ParentNode.Attributes["name"].Value;
                                XmlNode extLN0 = extFcdaNode.ParentNode.ParentNode;
                                XmlNodeList extGoCBs = extLN0.SelectNodes("ns:GSEControl", nsMgr);
                                XmlNodeList extSmvCBs = extLN0.SelectNodes("ns:SampledValueControl", nsMgr);
                                foreach (XmlNode gocb in extGoCBs)
                                {
                                    if (gocb.Attributes["datSet"].Value == dataset)
                                    {
                                        virLine.ExtCB = iedName + "/" + extLN0.ParentNode.Attributes["inst"].Value +"/" + gocb.Attributes["name"].Value;
                                        break;
                                    }
                                }
                                foreach (XmlNode smvcb in extSmvCBs)
                                {
                                    if (smvcb.Attributes["datSet"].Value == dataset)
                                    {
                                        virLine.ExtCB = iedName + "/" + extLN0.ParentNode.Attributes["inst"].Value + "/" + smvcb.Attributes["name"].Value;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                
                Virlines.Add(virLine);
            }

            // select LD nodes.
            LDNodes = node.SelectNodes("./ns:Server/ns:LDevice", nsMgr);
            foreach (XmlNode ldNode in LDNodes)
            {
                XmlNodeList dsNds = ldNode.SelectNodes("ns:LN0/ns:DataSet", nsMgr);
                foreach (XmlNode dsNd in dsNds)
                {
                    string strDesc = ((XmlElement)dsNd).GetAttribute("desc");
                    string strName = ((XmlElement)dsNd).GetAttribute("name");
                    if (strDesc == "压板数据集" || strName.Contains("dsEna") || strName.Contains("dsRelayEna"))
                    {
                        XmlNodeList fcdaNds = dsNd.ChildNodes;
                        foreach (XmlNode fcdaNd in fcdaNds)
                        {
                            DsRelayEna dsRelayEna = new DsRelayEna();
                            dsRelayEna.DsName = ldNode.ParentNode.ParentNode.ParentNode.Attributes["name"].Value // iedName
                                + "/" + strName + ":" + strDesc;

                            XmlElement fcdaEle = (XmlElement)fcdaNd;
                            dsRelayEna.DsData = fcdaEle.GetAttribute("ldInst") + "/" + fcdaEle.GetAttribute("prefix")
                                + fcdaEle.GetAttribute("lnClass") + fcdaEle.GetAttribute("lnInst") 
                                + "/" + fcdaEle.GetAttribute("doName");
                            if (fcdaEle.GetAttribute("lnClass") == "LLN0")
                            {
                                XmlNodeList doi0Nds = ldNode.SelectNodes("ns:LN0/ns:DOI", nsMgr);
                                foreach (XmlNode doi0Nd in doi0Nds)
                                {
                                    if (doi0Nd.Attributes["name"].Value == fcdaEle.GetAttribute("doName"))
                                    {
                                        dsRelayEna.DsDataDesc = doi0Nd.Attributes["desc"].Value;
                                        break;
                                    }
                                }
                            }
                            else 
                            {
                                XmlNodeList lnNds = ldNode.SelectNodes("ns:LN", nsMgr);
                                foreach (XmlNode lnNd in lnNds)
                                {
                                    XmlElement lnEle = (XmlElement)lnNd;
                                    string strLn = lnEle.GetAttribute("prefix") + lnEle.GetAttribute("lnClass") + lnEle.GetAttribute("inst");
                                    if (strLn == fcdaEle.GetAttribute("prefix") + fcdaEle.GetAttribute("lnClass") + fcdaEle.GetAttribute("lnInst"))
                                    {
                                        XmlNodeList doiNds = lnNd.SelectNodes("ns:DOI", nsMgr);
                                        foreach (XmlNode doiNd in doiNds)
                                        {

                                            if (doiNd.Attributes["name"].Value == fcdaEle.GetAttribute("doName"))
                                            {
                                                dsRelayEna.DsDataDesc = doiNd.Attributes["desc"].Value;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            DsRelayEnas.Add(dsRelayEna);
                        }
                    }
                }
            }
        }


    }
}
