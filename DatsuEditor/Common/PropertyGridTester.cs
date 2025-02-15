using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatsuEditor.Common
{
    internal class PropertyGridTester
    {
        [EditableProperty("This is a map, learn to hate it")]
        public Dictionary<string, string> TestMap { get; set; } = new Dictionary<string, string>() {
            { "TestKey", "TestValue" },
            { "SecondK", "SecondV" },
        };

        [EditableProperty]
        public bool TestBool { get; set; }

        [EditableProperty]
        public string TestString { get; set; }
    }
}
