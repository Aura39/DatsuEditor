using System;

namespace ZanLibrary
{
    public enum MGRFileFormat : int
    {
        UNK = 0, // ICON: V
        DAT, // ICON: V
        BXM, // ICON: V
        WTB, // ICON: V
        WMB, // ICON: V
        MOT, // ICON: X
        EST, // ICON: V
        BNK, // ICON: V
        SCR, // ICON: X
        SYN, // ICON: X
        LY2, // ICON: X
        UID, // ICON: X
        SOP, // ICON: X
        EXP, // ICON: X
        CTX, // ICON: X
        UVD, // ICON: X
        SAE, // ICON: X
        SAS, // ICON: X
        HKX, // ICON: X
        CPK, // ICON: X
        WEM, // ICON: V
        VCD, // ICON: X
        BRD  // ICON: X
    }
    public class FormatUtils
    {
        public static MGRFileFormat DetectFileFormat(byte[] data)
        {
            if (data.Length < 4)
                return MGRFileFormat.UNK;
            uint magic = BitConverter.ToUInt32(data, 0);
            switch (magic)
            {
                case 0x00544144:
                    return MGRFileFormat.DAT;
                case 0x004C4D58:
                case 0x004D5842:
                    return MGRFileFormat.BXM;
                case 0x00425457:
                    return MGRFileFormat.WTB;
                case 0x34424D57:
                    return MGRFileFormat.WMB;
                case 0x00464645:
                    return MGRFileFormat.EST;
                case 0x00746F6D:
                    return MGRFileFormat.MOT;
                case 0x44484B42:
                    return MGRFileFormat.BNK;
                case 0x00524353:
                    return MGRFileFormat.SCR;
                    // return MGRFileFormat.SYN;
                    // return MGRFileFormat.LY2;
                    // return MGRFileFormat.UID;
                    // return MGRFileFormat.SOP;
                    // return MGRFileFormat.EXP;
                case 0x00325443:
                    return MGRFileFormat.CTX;
                    // return MGRFileFormat.UVD;
                    // return MGRFileFormat.SAE;
                    // return MGRFileFormat.SAS;
                    // return MGRFileFormat.HKX;
                    // return MGRFileFormat.CPK;
                case 0x46464952:
                    return MGRFileFormat.WEM;
                    // return MGRFileFormat.VCD;
                    // return MGRFileFormat.BRD;
                default:
                    return MGRFileFormat.UNK;

            }
        }
    }
}
