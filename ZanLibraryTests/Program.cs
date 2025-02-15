using System.Diagnostics;
using ZanLibrary;
using ZanLibrary.Formats.Common;
using ZanLibrary.Formats.Container;
using ZanLibraryTests;

namespace ZanLibraryTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var data = File.ReadAllBytes(@"E:\p118_sub.bxm");
            File.WriteAllBytes(@"E:\bxm.bxm", BxmFile.Save(BxmFile.Load(data)[0]));
        }
    }
}