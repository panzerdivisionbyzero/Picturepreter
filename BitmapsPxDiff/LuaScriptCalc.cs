using MoonSharp.Interpreter;

namespace BitmapsPxDiff
{
    public struct ScriptEnvironmentVariables // carries thread chunk info (script "work environment")
    {
        public int chunkX, chunkY, chunkLastX, chunkLastY, imageW, imageH;
        public ScriptEnvironmentVariables(int chunkStartX, int chunkStartY, int chunkWidth, int chunkHeight, int imageW, int imageH)
        {
            this.chunkX = chunkStartX; 
            this.chunkY = chunkStartY;
            this.chunkLastX = chunkWidth;
            this.chunkLastY = chunkHeight;
            this.imageW = imageW;
            this.imageH = imageH;
        }
        public override string ToString() => $"chunkStartX={chunkX}\r\nchunkStartY={chunkY}\r\nchunkLastX={chunkLastX}\r\nchunkLastY={chunkLastY}\r\nimageW={imageW}\r\nimageH={imageH}\r\n";
    }
    public class LuaScriptCalc
	{
		public LuaScriptCalc()
		{
		}
        public bool LuaChangeColor(string dynamicCode, ref uint[][] pixelsIn, ref uint[] pixelsOut, List<string> logsOut, ScriptEnvironmentVariables envVars, ref string errorMessage)
        {
            bool result = false;
            string scriptText = envVars.ToString() + scriptBegin + dynamicCode + scriptEnd;
            Script script = new Script();
            try
            {
                script.Globals["pixelsIn"] = pixelsIn;
                /*for (int i = 0; i < pixelsIn.Length; i++)
                {
                    script.Globals["pixels"+i.ToString()] = pixelsIn[i];
                }*/
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
function rshift(x, by)
    return math.floor(x / 2 ^ by)
end
function ChangeColor2 (images)
    c1 = images[1]
    c2 = images[2]
    image1R = CastToByte(c1)
    image1G = CastToByte(rshift(c1,8))
    image1B = CastToByte(rshift(c1,16))
    image1A = CastToByte(rshift(c1,24))
    image2R = CastToByte(c2)
    image2G = CastToByte(rshift(c2,8))
    image2B = CastToByte(rshift(c2,16))
    image2A = CastToByte(rshift(c2,24))" + "\r\n";
        // (user script between scriptBegin and scriptEnd)
        const string scriptEnd = "\r\n" + @"return CastToByte(resultR) + CastToByte(resultG)*256 + CastToByte(resultB)*65536 + CastToByte(resultA)*16777216
end
function tablelength(T)
    local count = 0
    for _ in pairs(T) do count = count + 1 end
    return count
end
imageX=chunkStartX
imageY=chunkStartY
debug={}
function DebugEachPx(s)
    table.insert(debug,tablelength(debug)+1,s)    
end
function DebugForPx(x,y,s)
    if imageX==x and imageY==y then
        table.insert(debug,tablelength(debug)+1,s)
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
for i=1, tablelength(pixelsOut) do
    pixelsOut[i]=ChangeColor2(pixelsIn[i])
    imageX=imageX+1
    if imageX>chunkLastX then
        imageX=chunkStartX
        imageY=imageY+1
    end
end
return pixelsOut";
    }
}
