using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MapFileParser : MonoBehaviour
{
    public GameObject roadVisualizer;
    public GameObject buildingVisualizer;
    float time;
    Dictionary<ulong, OsmNode> nodes;
    List<OsmWay> ways;
    OsmBounds bounds;
    public string resourceFile;
    LineRenderer ray;
    public bool draw = true;
    // Start is called before the first frame update
    void Start()
    {
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();
        var txt = Resources.Load<TextAsset>(resourceFile);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(txt.text);
        SetBounds(xmlDoc.SelectSingleNode("/osm/bounds"));
        GetNodes(xmlDoc.SelectNodes("/osm/node"));
        GetWays(xmlDoc.SelectNodes("/osm/way"));
        Debug.Log(nodes);
        ray = GetComponent<LineRenderer>();
        makeRoad();


    }


    void makeRoad()
    {
        foreach (OsmWay w in ways)
        {
            if (w.Visible)
            {
                Color c = Color.cyan;
                GameObject toInstantiate=buildingVisualizer;
                if (!w.isBoundary)
                {
                    toInstantiate = roadVisualizer;
                }

                for (int i = 1; i < w.NodeIDs.Count; i++)
                {
                        OsmNode p1 = nodes[w.NodeIDs[i - 1]];
                        OsmNode p2 = nodes[w.NodeIDs[i]];

                        Vector3 v1 = p1 - bounds.Centre;
                        Vector3 v2 = p2 - bounds.Centre;
                        Debug.DrawLine(v1, v2, c);
                        GameObject road = Instantiate(toInstantiate, v1, Quaternion.identity);
                        RaycastHit hit;
                        road.GetComponent<LineRenderer>().SetPosition(0, v1);
                        road.GetComponent<LineRenderer>().SetPosition(1, v2);
                        road.GetComponent<LineRenderer>().startWidth = 2f;
                        road.GetComponent<LineRenderer>().endWidth = 2f;

                }
                

            }


        }

    }

    private void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
    }

    private void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;
        }
    }

    private void SetBounds(XmlNode xmlNode)
    {
        bounds = new OsmBounds(xmlNode);
    }


}
