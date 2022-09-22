/*
 * This unit is part of Picturepreter.
 * 
 * Licensed under the terms of the GNU GPL 2.0 license,
 * excluding used libraries:
 * - MoonSharp, licensed under MIT license;
 * and used code snippets marked with link to original source.
 * 
 * Copyright(c) 2022 by Paweł Witkowski
 * 
 * pawel.vitek.witkowski@gmail.com 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picturepreter
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
