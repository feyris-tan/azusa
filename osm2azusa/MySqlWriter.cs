using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AzusaERP;
using osm2azusa.Model;
using MySql.Data.MySqlClient;

namespace osm2azusa
{
    class MySqlWriter : IAzusaWriter
    {
        public MySqlWriter()
        {
            Ini ini = new Ini("azusa.ini");
            if (!ini.ContainsKey("mysql"))
                throw new Exception("mysql not configured.");
            IniSection iniSection = ini["mysql"];

            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder();
            connectionStringBuilder.Server = iniSection["server"];
            connectionStringBuilder.Port = Convert.ToUInt32(iniSection["port"]);
            connectionStringBuilder.UserID = iniSection["username"];
            connectionStringBuilder.Password = iniSection["password"];
            connectionStringBuilder.Database = iniSection["database"];
            conn = new MySqlConnection(connectionStringBuilder.ToString());
            conn.Open();
            conn.Ping();

            Console.WriteLine("Connected to: " + conn.ServerVersion);

            writeNode = conn.CreateCommand();
            writeNode.CommandText = "INSERT INTO osm_nodes (id,lat,lon) VALUES (@id,@lat,@lon)";
            writeNode.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
            writeNode.Parameters.Add(new MySqlParameter("@lat", MySqlDbType.Float));
            writeNode.Parameters.Add(new MySqlParameter("@lon", MySqlDbType.Float));
            writeNode.Prepare();
            writeNodeTag = conn.CreateCommand();
            writeNodeTag.CommandText = "INSERT INTO osm_node_tags (nodeId,k,v) VALUES (@nodeId,@k,@v)";
            writeNodeTag.Parameters.Add(new MySqlParameter("@nodeId", MySqlDbType.Int64));
            writeNodeTag.Parameters.Add(new MySqlParameter("@k", MySqlDbType.String));
            writeNodeTag.Parameters.Add(new MySqlParameter("@v", MySqlDbType.String));
            writeNodeTag.Prepare();
            writeRelation = conn.CreateCommand();
            writeRelation.CommandText = "INSERT INTO osm_relations (id) VALUES (@id)";
            writeRelation.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
            writeRelation.Prepare();
            writeRelationMember = conn.CreateCommand();
            writeRelationMember.CommandText = "INSERT INTO osm_relation_member (relationId,type,reference,role) VALUES (@relationId,@type,@reference,@role)";
            writeRelationMember.Parameters.Add(new MySqlParameter("@relationId", MySqlDbType.Int64));
            writeRelationMember.Parameters.Add(new MySqlParameter("@type", MySqlDbType.String));
            writeRelationMember.Parameters.Add(new MySqlParameter("@reference", MySqlDbType.Int64));
            writeRelationMember.Parameters.Add(new MySqlParameter("@role", MySqlDbType.String));
            writeRelationMember.Prepare();
            writeRelationTags = conn.CreateCommand();
            writeRelationTags.CommandText = "INSERT INTO osm_relation_tags (relationId,k,v) VALUES (@relationId,@k,@v)";
            writeRelationTags.Parameters.Add(new MySqlParameter("@relationId", MySqlDbType.Int64));
            writeRelationTags.Parameters.Add(new MySqlParameter("@k", MySqlDbType.String));
            writeRelationTags.Parameters.Add(new MySqlParameter("@v", MySqlDbType.String));
            writeRelationTags.Prepare();
            writeWay = conn.CreateCommand();
            writeWay.CommandText = "INSERT INTO osm_ways (id) VALUES (@id)";
            writeWay.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
            writeWay.Prepare();
            writeWayNode = conn.CreateCommand();
            writeWayNode.CommandText = "INSERT INTO osm_way_nodes (way,node) VALUES (@way,@node)";
            writeWayNode.Parameters.Add(new MySqlParameter("@way", MySqlDbType.Int64));
            writeWayNode.Parameters.Add(new MySqlParameter("@node", MySqlDbType.Int64));
            writeWayNode.Prepare();
            writeWayTags = conn.CreateCommand();
            writeWayTags.CommandText = "INSERT INTO osm_way_tags (wayId,k,v) VALUES (@wayId,@k,@v)";
            writeWayTags.Parameters.Add(new MySqlParameter("@wayId", MySqlDbType.Int64));
            writeWayTags.Parameters.Add(new MySqlParameter("@k", MySqlDbType.String));
            writeWayTags.Parameters.Add(new MySqlParameter("@v", MySqlDbType.String));
            writeWayTags.Prepare();

            transaction = conn.BeginTransaction();
        }

        ushort waiter;

        MySqlCommand writeWayTags;
        MySqlCommand writeWayNode;
        MySqlCommand writeWay;
        MySqlCommand writeRelationTags;
        MySqlCommand writeRelationMember;
        MySqlCommand writeRelation;
        MySqlCommand writeNodeTag;
        MySqlCommand writeNode;
        MySqlConnection conn;
        MySqlTransaction transaction;

        public void Dispose()
        {
            transaction.Commit();
            writeNodeTag.Dispose();
            writeNode.Dispose();
            conn.Close();
            conn.Dispose();
        }

        public void WriteMember(Relation way, Member member)
        {
            writeRelationMember.Parameters["@relationId"].Value = way.id;
            writeRelationMember.Parameters["@type"].Value = member.type;
            writeRelationMember.Parameters["@reference"].Value = member.reference;
            writeRelationMember.Parameters["@role"].Value = member.role;
            writeRelationMember.ExecuteNonQuery();
        }

        public void WriteNode(Node node)
        {
            writeNode.Parameters["@id"].Value = node.id;
            writeNode.Parameters["@lat"].Value = node.lat;
            writeNode.Parameters["@lon"].Value = node.lon;
            writeNode.ExecuteNonQuery();
            foreach(KeyValuePair<string,string> tag in node.tags)
            {
                WriteTag(node, tag.Key, tag.Value);
            }
            FlushTransactionIfRequired();
        }

        public void WriteRelation(Relation relation)
        {
            writeRelation.Parameters["@id"].Value = relation.id;
            writeRelation.ExecuteNonQuery();
            foreach(Member member in relation.members)
            {
                WriteMember(relation, member);
            }
            foreach (KeyValuePair<string,string> tag in relation.tags)
            {
                WriteTag(relation, tag.Key, tag.Value);
            }
            FlushTransactionIfRequired();
        }

        public void WriteTag(Node node, string k, string v)
        {
            if (v.Length > 128)
                return;

            writeNodeTag.Parameters["@nodeId"].Value = node.id;
            writeNodeTag.Parameters["@k"].Value = k;
            writeNodeTag.Parameters["@v"].Value = v;
            writeNodeTag.ExecuteNonQuery();
        }

        public void WriteTag(Way way, string k, string v)
        {
            writeWayTags.Parameters["@wayId"].Value = way.id;
            writeWayTags.Parameters["@k"].Value = k;
            writeWayTags.Parameters["@v"].Value = v;
            writeWayTags.ExecuteNonQuery();
        }

        public void WriteTag(Relation relation, string k, string v)
        {
            writeRelationTags.Parameters["@relationId"].Value = relation.id;
            writeRelationTags.Parameters["@k"].Value = k;
            writeRelationTags.Parameters["@v"].Value = v;
            writeRelationTags.ExecuteNonQuery();
        }

        public void WriteWay(Way way)
        {
            writeWay.Parameters["@id"].Value = way.id;
            writeWay.ExecuteNonQuery();
            foreach(ulong node in way.nodes)
            {
                WriteWayNode(way, node);
            }
            foreach(KeyValuePair<string,string> tag in way.tags)
            {
                WriteTag(way, tag.Key, tag.Value);
            }
            FlushTransactionIfRequired();
        }

        public void WriteWayNode(Way way, ulong reference)
        {
            writeWayNode.Parameters["@way"].Value = way.id;
            writeWayNode.Parameters["@node"].Value = reference;
            writeWayNode.ExecuteNonQuery();
        }

        void FlushTransactionIfRequired()
        {
            waiter++;
            if (waiter == 9100)
            {
                transaction.Commit();
                transaction = conn.BeginTransaction();
            }
        }
    }
}
