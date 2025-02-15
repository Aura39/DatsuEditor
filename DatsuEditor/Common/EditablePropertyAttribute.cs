using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatsuEditor.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    sealed class EditablePropertyAttribute : Attribute
    {
        public string DisplayedName;
        public EditablePropertyAttribute(string dispName = "")
        {
            DisplayedName = dispName;
        }
    }
}
