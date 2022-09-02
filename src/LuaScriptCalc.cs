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
using MoonSharp.Interpreter;

namespace Picturepreter
{
    /// <summary>
    /// Structure contains thread chunk info (script "work environment")
    /// </summary>
    public struct ScriptEnvironmentVariables
    {
        public int chunkX, chunkY, chunkLastX, chunkLastY, imageW, imageH, imagesCount;
        public ScriptEnvironmentVariables(int chunkStartX, int chunkStartY, int chunkWidth, int chunkHeight, int imageW, int imageH, int imagesCount)
        {
            this.chunkX = chunkStartX; 
            this.chunkY = chunkStartY;
            this.chunkLastX = chunkWidth;
            this.chunkLastY = chunkHeight;
            this.imageW = imageW;
            this.imageH = imageH;
            this.imagesCount = imagesCount;
        }
        public override string ToString() => $"chunkStartX={chunkX}\r\nchunkStartY={chunkY}\r\nchunkLastX={chunkLastX}\r\nchunkLastY={chunkLastY}\r\nimageW={imageW}\r\nimageH={imageH}\r\nimagesCount={imagesCount}\r\n";
    }
    /// <summary>
    /// LuaScriptCalc class processes given source pixels arrays by given script and returns result pixels array;
    /// The script template provides global variables:
    /// - chunkStartX
    /// - chunkStartY
    /// - chunkLastX
    /// - chunkLastY
    /// - imageW
    /// - imageH
    /// - currentX
    /// - currentY
    /// - imagesCount
    /// - imagesPixels[imageIndex] = {A,R,G,B}
    /// - result = {A,R,G,B}
    /// and functions:
    /// - CastToByte()
    /// - Tablelength()
    /// - DebugEachPx()
    /// - DebugForPx()
    /// - DebugChunkBegin()
    /// - DebugChunkEnd()
    /// - DebugImageBegin()
    /// - DebugImageEnd()
    /// (see scriptBegin and scriptEnd fields)
    /// </summary>
    public class LuaScriptCalc
	{
        /// <summary>
        /// Class constructor;
        /// </summary>
		public LuaScriptCalc()
		{
		}
        /// <summary>
        /// Responsible for entire script processing:
        /// - prepares final script content;
        /// - passes input data (as global variables) to MoonSharp (LUA script interpreter);
        /// - initiates MoonSharp interpreter;
        /// - takes out results (pixelsOut[], debug strings)
        /// </summary>
        /// <param name="dynamicCode">User-dependent script part (placed between scriptBegin and scriptEnd)</param>
        /// <param name="pixelsImagesARGB">Source images pixels values in order: Pixels/Images/ARGB</param>
        /// <param name="pixelsOut">Result image pixels values</param>
        /// <param name="logsOut">LUA script debug logs</param>
        /// <param name="envVars">Chunk and image params (script "work environment")</param>
        /// <param name="errorMessage">Message returned in case of exception</param>
        /// <returns></returns>
        public bool LuaChangeColor(string dynamicCode, ref byte[] pixelsImagesARGB, ref uint[] pixelsOut, List<string> logsOut, ScriptEnvironmentVariables envVars, ref string errorMessage)
        {
            bool result = false;
            string scriptText = envVars.ToString() + scriptBegin + dynamicCode + scriptEnd;
            Script script = new Script();
            try
            {
                script.Globals["pixelsIn"] = pixelsImagesARGB;
                script.Globals["pixelsOut"] = pixelsOut;

                DynValue res = script.DoString(scriptText);
                for (int p = 1; p <= pixelsOut.Length; p++)
                    pixelsOut[p - 1] = Convert.ToUInt32(res.Table[p]);

                result = true;
            }
            catch (Exception e)
            {
                errorMessage = "Script error:\r\n" + e.Message + "\r\nGenerated script:\r\n" + scriptText;
            }
            if (script.Globals["debug"] != null)
            {
                try
                {
                    MoonSharp.Interpreter.Table tab = (MoonSharp.Interpreter.Table)script.Globals["debug"];
                    for (int t = 1; t <= tab.Length; t++)
                        logsOut.Add(tab[t].ToString());
                }
                catch
                {
                    errorMessage += "\r\nUnable to extract thread logs.";
                }
            }
            return result;
        }
        /* LUA functions sources:
         * https://stackoverflow.com/questions/5977654/how-do-i-use-the-bitwise-operator-xor-in-lua
         * https://stackoverflow.com/questions/2705793/how-to-get-number-of-entries-in-a-lua-table
         */
        const string scriptBegin = @"  
function CastToByte(i)
    if i<0 then i = i % 256 + 255 end
    if i>255 then i = i % 256 end
    return math.floor(i)
end
function ChangeColor()" + "\r\n";
        // (user script between scriptBegin and scriptEnd)
        const string scriptEnd = "\r\n" + @"return CastToByte(result[2]) + CastToByte(result[3])*256 + CastToByte(result[4])*65536 + CastToByte(result[1])*16777216
end
function Tablelength(T)
    local count = 0
    for _ in pairs(T) do count = count + 1 end
    return count
end
currentX=chunkStartX
currentY=chunkStartY
debug={}
function DebugEachPx(s)
    table.insert(debug,Tablelength(debug)+1,s)    
end
function DebugForPx(x,y,s)
    if currentX==x and currentY==y then
        table.insert(debug,Tablelength(debug)+1,s)
    end
end
function DebugChunkBegin(s)
    DebugForPx(chunkStartX,chunkStartY,s)
end
function DebugChunkEnd(s)
    DebugForPx(chunkLastX,chunkLastY,s)
end
function DebugImageBegin(s)
    DebugForPx(0,0,s)
end
function DebugImageEnd(s)
    DebugForPx(imageW-1,imageH-1,s)  
end
pxPos = 1
pxStep = imagesCount*4;
imagesPixels = {}
result = {0,0,0,0}
for i=1, imagesCount do
    table.insert(imagesPixels, {0,0,0,0})
end
for i=1, Tablelength(pixelsOut) do
    for im=1, imagesCount do
        colorPxPos = pxPos+(im-1)*4
        imagesPixels[im] = {pixelsIn[colorPxPos],pixelsIn[colorPxPos+1],pixelsIn[colorPxPos+2],pixelsIn[colorPxPos+3]}
    end
    pixelsOut[i]=ChangeColor()
    currentX=currentX+1
    if currentX>chunkLastX then
        currentX=chunkStartX
        currentY=currentY+1
    end
    pxPos = pxPos + pxStep
end
return pixelsOut";
    }
}
