using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapsPxDiff
{
    // https://stackoverflow.com/questions/1600962/displaying-the-build-date?answertab=modifieddesc#tab-top
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
