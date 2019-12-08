using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using System.IO;
using System.Xml;
using osm2azusa.Model;
using System.Globalization;

namespace osm2azusa
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("azusa.ini"))
            {
                Console.WriteLine("could not find azusa.ini!");
                return;
            }
            FileStream level1 = File.OpenRead(args[0]);
            BufferedStream level2 = new BufferedStream(level1);
            BZip2InputStream level3 = new BZip2InputStream(level2);
            XmlReader xmlReader = XmlReader.Create(level3);
            IFormatProvider culture = CultureInfo.InvariantCulture;
            IAzusaWriter azusaWriter = new ProxyWriter(new MySqlWriter(), level1);

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "node":
                                Node child = new Node();
                                child.id = Convert.ToInt64(xmlReader.GetAttribute("id"));
                                child.lat = Convert.ToDouble(xmlReader.GetAttribute("lat"),culture);
                                child.lon = Convert.ToDouble(xmlReader.GetAttribute("lon"),culture);
                                if (!xmlReader.IsEmptyElement)
                                {
                                    while (xmlReader.NodeType != XmlNodeType.EndElement)
                                    {
                                        xmlReader.Read();
                                        if (xmlReader.NodeType == XmlNodeType.Element)
                                        {
                                            if (xmlReader.Name == "tag")
                                            {
                                                string k = xmlReader.GetAttribute(0);
                                                if (k.ToLower().Equals("fixme"))
                                                    continue;
                                                string v = xmlReader.GetAttribute(1);
                                                child.tags[k] = v;
                                            }
                                            else
                                            {
                                                throw new NotImplementedException(xmlReader.Name);
                                            }
                                        }
                                    }
                                }
                                azusaWriter.WriteNode(child);
                                break;
                            case "way":
                                Way way = new Way();
                                way.id = Convert.ToInt64(xmlReader.GetAttribute("id"));
                                if (!xmlReader.IsEmptyElement)
                                {
                                    while (xmlReader.NodeType != XmlNodeType.EndElement)
                                    {
                                        xmlReader.Read();
                                        if (xmlReader.NodeType == XmlNodeType.Element)
                                        {
                                            if (xmlReader.Name == "nd")
                                            {
                                                long nodeRef = Convert.ToInt64(xmlReader.GetAttribute("ref"));
                                                if (!way.nodes.Contains(nodeRef))
                                                {
                                                    way.nodes.Add(nodeRef);
                                                }
                                                else
                                                {
                                                    nodeRef.GetHashCode();
                                                }
                                            }
                                            else if (xmlReader.Name == "tag")
                                            {
                                                string k = xmlReader.GetAttribute(0).ToLower();
                                                if (k.Equals("fixme"))
                                                    continue;
                                                if (way.tags.ContainsKey(k))
                                                    continue;
                                                string v = xmlReader.GetAttribute(1);
                                                way.tags[k] = v;
                                            }
                                            else
                                            {
                                                throw new NotImplementedException(xmlReader.Name);
                                            }
                                        }
                                    }
                                }
                                azusaWriter.WriteWay(way);
                                break;
                            case "relation":
                                Relation relation = new Relation();
                                relation.id = Convert.ToInt64(xmlReader.GetAttribute("id"));
                                if (!xmlReader.IsEmptyElement)
                                {
                                    while (xmlReader.NodeType != XmlNodeType.EndElement)
                                    {
                                        xmlReader.Read();
                                        if (xmlReader.NodeType == XmlNodeType.Element)
                                        {
                                            if (xmlReader.Name == "member")
                                            {
                                                Member member = new Member();
                                                member.type = xmlReader.GetAttribute("type");
                                                member.reference = Convert.ToInt64(xmlReader.GetAttribute("ref"));
                                                member.role = xmlReader.GetAttribute("role");
                                                if (!relation.members.Contains(member))
                                                {
                                                    relation.members.Add(member);
                                                }
                                                else
                                                {
                                                    relation.GetHashCode();
                                                }
                                            }
                                            else if (xmlReader.Name == "tag")
                                            {
                                                string k = xmlReader.GetAttribute(0).ToLower();
                                                if (k.Length > 128)
                                                    continue;
                                                if (relation.tags.ContainsKey(k))
                                                    continue;
                                                if (k.ToLower().Equals("fixme"))
                                                    continue;
                                                string v = xmlReader.GetAttribute(1);
                                                relation.tags[k] = v;
                                            }
                                            else
                                            {
                                                throw new NotImplementedException(xmlReader.Name);
                                            }
                                        }
                                    }
                                }
                                azusaWriter.WriteRelation(relation);
                                break;
                            case "osm":
                                DateTime version = Convert.ToDateTime(xmlReader.GetAttribute("timestamp"));
                                Console.WriteLine("OSM Stream Timestamp: " + version);
                                break;
                            case "bounds":
                                break;
                            default:
                                throw new NotImplementedException(xmlReader.Name);
                        }
                        break;
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.XmlDeclaration:
                    case XmlNodeType.EndElement:
                        break;
                    default:
                        throw new NotImplementedException(xmlReader.NodeType.ToString());
                }
            }
            azusaWriter.Dispose();
            xmlReader.Close();
            level2.Close();
            level1.Close();
            level1.Dispose();
        }
    }
}
