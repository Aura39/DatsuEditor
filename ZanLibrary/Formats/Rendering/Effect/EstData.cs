namespace ZanLibrary.Formats.Rendering.Effect
{
    public class EstData
    {
        public struct EffectHeader
        {
            public string Magic; // Assuming fixed size 4
            public uint RecordCount;
            public uint RecordOffsetsOffset;
            public uint TypeOffset;
            public uint TypeEndOffset;
            public uint TypeSize;
            public uint TypeNumber;
        }

        public class TypeGroup
        {
            public uint Padding;
            public string Id;
            public uint Size;
            public uint Offset;
        }

        public class TypeGroups
        {
            public TypeGroup[] Types;
        }

        public struct FileLayout
        {
            public EffectHeader Header;
            public uint[] Offsets;
            public TypeGroups[] TypeGroups;
        }
    }
}
