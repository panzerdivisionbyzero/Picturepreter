using MoonSharp.Interpreter;

namespace BitmapsPxDiff
{
    public struct ScriptEnvironmentVariables // carries thread chunk info (script "work environment")
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
    public class LuaScriptCalc
	{
		public LuaScriptCalc()
		{
		}
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
function rshift(x, by)
    return math.floor(x / 2 ^ by)
end
function ChangeColor2 ()" + "\r\n";
        // (user script between scriptBegin and scriptEnd)
        const string scriptEnd = "\r\n" + @"return CastToByte(result[2]) + CastToByte(result[3])*256 + CastToByte(result[4])*65536 + CastToByte(result[1])*16777216
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
pxPos = 1
pxStep = imagesCount*4;
colors = {}
result = {0,0,0,0}
for i=1, imagesCount do
    table.insert(colors, {0,0,0,0})
end
for i=1, tablelength(pixelsOut) do
    for im=1, imagesCount do
        colorPxPos = pxPos+(im-1)*4
        colors[im] = {pixelsIn[colorPxPos],pixelsIn[colorPxPos+1],pixelsIn[colorPxPos+2],pixelsIn[colorPxPos+3]}
    end
    pixelsOut[i]=ChangeColor2()
    imageX=imageX+1
    if imageX>chunkLastX then
        imageX=chunkStartX
        imageY=imageY+1
    end
    pxPos = pxPos + pxStep
end
return pixelsOut";
    }
}
