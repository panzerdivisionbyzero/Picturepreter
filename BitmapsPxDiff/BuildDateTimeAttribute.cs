using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapsPxDiff
{
    /// <summary>
    /// Reads project build date (it's kind of automatic version number);
    /// Taken from: https://stackoverflow.com/questions/1600962/displaying-the-build-date?answertab=modifieddesc#tab-top
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildDateTimeAttribute : Attribute
    {
        public string Date { get; set; }
        public BuildDateTimeAttribute(string date)
        {
            Date = date;
        }
    }
}
