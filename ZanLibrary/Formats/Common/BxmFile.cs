using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace ZanLibrary.Formats.Common
{
    public struct BxmHeader
    {
        public string Magic;
        public uint Flags;
        public short NodeCount;
        public short DataCount;
        public uint DataSize;
    }
    public struct BxmDataOffset
    {
        public ushort NameOffset;
        public ushort DataOffset;

        public bool Equals(BxmDataOffset other)
        {
            return (NameOffset == other.NameOffset) && (DataOffset == other.DataOffset);
        }
    }
    public struct BxmNodeInfo
    {
        public short ChildCount;
        public short FirstChildIndex;
        public short AttributeCount;
        public short DataIndex;
    }
    public struct BxmAttribute
    {
        public string Name;
        public string Value;
    }
    public class BxmDataNode
    {
        public string Name;
        public string Value;
        public Dictionary<string, string> Attributes;
        public List<BxmDataNode> Children;
        public BxmDataNode Parent;

        public int Index;
        public int ChildCount;
        public int FirstChildIndex;
    }
    public class BxmFile
    {
        private static List<BxmDataNode> GetNodeChildren(BxmDataNode[] Nodes, BxmDataNode RequestedNode)
        {
            var FirstChild = RequestedNode.FirstChildIndex;
            var Children = Nodes.Skip(FirstChild).Take(RequestedNode.ChildCount);
            foreach (var node in Children)
            {
                node.Parent = RequestedNode;
                node.Children = GetNodeChildren(Nodes, node);
            }
            return Children.ToList();
        }
        public static BxmDataNode[] Load(byte[] data)
        {

            BinReader reader = new BinReader(data, Endianness.Big);
            BxmHeader header = new BxmHeader();

            header.Magic = reader.ReadString(4);
            header.Flags = reader.ReadUInt32();
            header.NodeCount = reader.ReadInt16();
            header.DataCount = reader.ReadInt16();
            header.DataSize = reader.ReadUInt32();

            List<BxmNodeInfo> infos = new List<BxmNodeInfo>();
            for (int i = 0; i < header.NodeCount; i++)
            {
                BxmNodeInfo NodeInfo = new BxmNodeInfo();
                NodeInfo.ChildCount = reader.ReadInt16();
                NodeInfo.FirstChildIndex = reader.ReadInt16();
                NodeInfo.AttributeCount = reader.ReadInt16();
                NodeInfo.DataIndex = reader.ReadInt16();
                infos.Add(NodeInfo);
            }

            List<BxmDataOffset> dataOffsets = new List<BxmDataOffset>();
            for (int i = 0; i < header.DataCount; i++)
            {
                BxmDataOffset DataOffset = new BxmDataOffset();
                DataOffset.NameOffset = reader.ReadUInt16();
                DataOffset.DataOffset = reader.ReadUInt16();
                dataOffsets.Add(DataOffset);
            }

            uint stringsOffset = (uint)(0x10 + 8 * header.NodeCount + header.DataCount * 4);

            List<BxmDataNode> nodes = new List<BxmDataNode>();
            for (int i = 0; i < infos.Count; i++)
            {
                BxmNodeInfo info = infos[i];
                BxmDataNode node = new BxmDataNode();

                node.Index = i;
                node.FirstChildIndex = info.FirstChildIndex;
                node.ChildCount = info.ChildCount;

                var nameOffset = dataOffsets[info.DataIndex].NameOffset;
                if (nameOffset != 0xFFFF)
                {
                    reader.Seek(stringsOffset + nameOffset);
                    node.Name = reader.ReadString();
                }
                var dataOffset = dataOffsets[info.DataIndex].DataOffset;
                if (dataOffset != 0xFFFF)
                {
                    reader.Seek(stringsOffset + dataOffset);
                    node.Value = reader.ReadString();
                }

                node.Attributes = new Dictionary<string, string>();
                for (int j = 0; j < info.AttributeCount; j++)
                {
                    string name = string.Empty;
                    string value = string.Empty;
                    var attrNameOffset = dataOffsets[info.DataIndex + 1 + j].NameOffset;
                    if (attrNameOffset != 0xFFFF)
                    {
                        reader.Seek(stringsOffset + attrNameOffset);
                        name = reader.ReadString();
                    }
                    var attrDataOffset = dataOffsets[info.DataIndex + 1 + j].DataOffset;
                    if (attrDataOffset != 0xFFFF)
                    {
                        reader.Seek(stringsOffset + attrDataOffset);
                        value = reader.ReadString();
                    }
                    node.Attributes[name] = value;
                }
                nodes.Add(node);
            }

            foreach (var node in nodes)
            {
                node.Children = GetNodeChildren(nodes.ToArray(), node);
                foreach (var child in node.Children)
                    child.Parent = node;
            }

            return nodes.ToArray();
        }

        public static byte[] Save(BxmDataNode root)
        {
            /*
             * Generating part
             */

            List<BxmDataNode> nodes = new List<BxmDataNode>();

            void FlattenChildren(BxmDataNode node)
            {
                nodes.Add(node);
                foreach (var child in node.Children) { 
                    FlattenChildren(child);
                }
            }

            FlattenChildren(root);

            foreach (var node in nodes)
            {
                Console.WriteLine($"{node.Name.PadRight(16)} : {node.Index.ToString().PadRight(4)} : {node.FirstChildIndex}");
            }

            List<string> UniqueStrings = new List<string>();
            void AddUniqueString(string str)
            {
                if (str != null && !UniqueStrings.Contains(str))
                    UniqueStrings.Add(str);
            }
            foreach (var node in nodes)
            {
                AddUniqueString(node.Name);
                foreach (var attr in node.Attributes)
                {
                    AddUniqueString(attr.Key);
                    AddUniqueString(attr.Value);
                }
                AddUniqueString(node.Value);
            }

            Dictionary<string, ushort> StringOffsets = new Dictionary<string, ushort>();
            int StringOffset = 0;
            foreach (var UniqueString in UniqueStrings)
            {
                StringOffsets.Add(UniqueString, (ushort)StringOffset);
                StringOffset += UniqueString.Length + 1;
            }

            List<BxmDataOffset> dataOffsets = new List<BxmDataOffset>();
            Dictionary<BxmDataNode, int> NodeDataIndices = new Dictionary<BxmDataNode, int>();
            foreach (var node in nodes)
            {
                BxmDataOffset offset = new BxmDataOffset();
                offset.NameOffset = node.Name != null && StringOffsets.ContainsKey(node.Name) ? StringOffsets[node.Name] : (ushort)0xFFFF;
                offset.DataOffset = node.Value != null && StringOffsets.ContainsKey(node.Value) ? StringOffsets[node.Value] : (ushort)0xFFFF;
                NodeDataIndices.Add(node, dataOffsets.Count);
                dataOffsets.Add(offset);
                foreach (var attr in node.Attributes)
                {
                    BxmDataOffset attrOffset = new BxmDataOffset();
                    attrOffset.NameOffset = attr.Key != null && StringOffsets.ContainsKey(attr.Key) ? StringOffsets[attr.Key] : (ushort)0xFFFF;
                    attrOffset.DataOffset = attr.Value != null && StringOffsets.ContainsKey(attr.Value) ? StringOffsets[attr.Value] : (ushort)0xFFFF;
                    dataOffsets.Add(attrOffset);
                }
            }

            List<BxmNodeInfo> infos = new List<BxmNodeInfo>();
            BxmNodeInfo NodeToInfo(BxmDataNode Node, int FCI)
            {
                BxmNodeInfo info = new();
                info.ChildCount = (short)Node.Children.Count;
                info.AttributeCount = (short)Node.Attributes.Count;
                info.DataIndex = (short)NodeDataIndices[Node];
                info.FirstChildIndex = (short)FCI;
                return info;
            }
            int PreCounter = 1;
            int Counter = 1;
            void AddChildrenInfo(BxmDataNode Node)
            {
                foreach (var child in Node.Children)
                {
                    if (child.Children.Count > 0)
                        infos.Add(NodeToInfo(child, Counter));
                    else
                        infos.Add(NodeToInfo(child, PreCounter));

                    Counter += child.Children.Count;
                }
                foreach (var child in Node.Children)
                {
                    PreCounter += child.Children.Count;
                    AddChildrenInfo(child);
                }
            }
            infos.Add(NodeToInfo(root, Counter));
            PreCounter += root.Children.Count;
            Counter = PreCounter;
            AddChildrenInfo(root);

            /*
             * Binary writing part
             */

            BinWriter writer = new BinWriter(Endianness.Big);

            writer.WriteString("BXM\0");
            writer.WriteUInt32(0);
            writer.WriteInt16((short)nodes.Count);
            writer.WriteInt16((short)dataOffsets.Count);
            uint collectiveSum = (uint)UniqueStrings.Sum(x => x.Length);
            writer.WriteUInt32(collectiveSum);

            foreach (var info in infos)
            {
                writer.WriteInt16(info.ChildCount);
                writer.WriteInt16(info.FirstChildIndex);
                writer.WriteInt16(info.AttributeCount);
                writer.WriteInt16(info.DataIndex);
            }

            foreach (var data in dataOffsets)
            {
                writer.WriteUInt16(data.NameOffset);
                writer.WriteUInt16(data.DataOffset);
            }

            foreach (var str in UniqueStrings)
            {
                writer.WriteString(str);
                writer.WriteByte(0);
            }

            return writer.GetArray();
        }
    }
}
