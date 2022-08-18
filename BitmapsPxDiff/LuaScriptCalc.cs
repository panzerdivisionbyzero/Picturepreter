using MoonSharp.Interpreter;

namespace BitmapsPxDiff
{
	public class LuaScriptCalc
	{
		public LuaScriptCalc()
		{
		}
        /* LUA functions sources:
         * https://stackoverflow.com/questions/5977654/how-do-i-use-the-bitwise-operator-xor-in-lua
         * https://stackoverflow.com/questions/2705793/how-to-get-number-of-entries-in-a-lua-table
         */
        public bool LuaChangeColor(string dynamicCode, ref uint[] pixels1, ref uint[] pixels2, ref uint[] pixelsOut, ref string errorMessage)
        {
            string script = "";
            try
            {
                script = @"  
function CastToByte(i)
    if i<0 then i = i % 256 + 255 end
    if i>255 then i = i % 256 end
    return i
end
function rshift(x, by)
  return math.floor(x / 2 ^ by)
end
function ChangeColor2 (c1,c2)
image1R = CastToByte(c1)
image1G = CastToByte(rshift(c1,8))
image1B = CastToByte(rshift(c1,16))
image1A = CastToByte(rshift(c1,24))
image2R = CastToByte(c2)
image2G = CastToByte(rshift(c2,8))
image2B = CastToByte(rshift(c2,16))
image2A = CastToByte(rshift(c2,24))"
    + "\r\n" + dynamicCode + "\r\n"
    + @"return CastToByte(resultR) + CastToByte(resultG)*256 + CastToByte(resultB)*65536 + CastToByte(resultA)*16777216
end
function tablelength(T)
  local count = 0
  for _ in pairs(T) do count = count + 1 end
  return count
end

for i=1,tablelength(pixelsOut) do 
    pixelsOut[i]=ChangeColor2(pixels1[i],pixels2[i])
end
return pixelsOut";

                Script s = new Script();
                s.Globals["pixels1"] = pixels1;
                s.Globals["pixels2"] = pixels2;
                s.Globals["pixelsOut"] = pixelsOut;

                DynValue res = s.DoString(script);
                for (int p = 1; p <= res.Table.Length; p++)
                    pixelsOut[p - 1] = Convert.ToUInt32(res.Table[p]);
            }
            catch (Exception e)
            {
                errorMessage = "Script error:\r\n" + e.Message + "\r\nGenerated script:\r\n" + script;
                return false;
            }

            return true;
        }
    }
}
