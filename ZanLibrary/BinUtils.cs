namespace ZanLibrary
{
    public enum Endianness
    {
        Little,
        Big,
    }
    public class BinUtils
    {
        public static uint CalcPadding(uint BlockSize, uint Length)
        {
	        return BlockSize - (Length % BlockSize);
        }
        public static int CalcPadding(int BlockSize, int Length)
        {
	        return BlockSize - (Length % BlockSize);
        }
    }
}
