using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml;


public class OsmWay : BaseOsm
{
    public ulong ID { get; private set; }
    public bool Visible { get; private set; }
    public List<ulong> NodeIDs { get; private set; }

    public bool isBoundary;
    public OsmWay(XmlNode node)
    {
        NodeIDs = new List<ulong>();
        ID = GetAttribute<ulong>("id", node.Attributes);
        Visible = GetAttribute<bool>("visible", node.Attributes);

        XmlNodeList nds = node.SelectNodes("nd");
        foreach(XmlNode n in nds)
        {
            ulong refno = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refno);
        }
        if(NodeIDs.Count>1)
        {
            isBoundary = NodeIDs[0] == NodeIDs[NodeIDs.Count - 1];
        }
    }
}
