using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatsuEditor.Common
{
    public class FileFilterBuilder
    {
        string Filter = string.Empty;
        public FileFilterBuilder AddFilter(string Extension, string Description)
        {
            Filter += $"{Description}|*.{Extension}|";
            return this;
        }

        public FileFilterBuilder AddFilters(string[] Extensions, string Description)
        {
            string build = string.Empty;
            foreach (string Extension in Extensions)
            {
                build += "*." + Extension + ";";
            }
            Filter += $"{Description}|{build.Substring(0, build.Length - 1)}|";
            return this;
        }
        public string GetFilter()
        {
            return Filter.Substring(0, Filter.Length - 1);
        }
    }
}
