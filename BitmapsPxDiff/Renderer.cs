using System.Diagnostics;

namespace BitmapsPxDiff
{
    public delegate void RenderFinishEvent(Bitmap newImage, string newStatus);
    public class Renderer
	{
        private Thread? renderThread;
        private ManualResetEvent rendererSignal = new ManualResetEvent(true); // blokuje utworzenie nowego watku, dopoki dziala poprzednie wywolanie
        private bool interruptRendering = false;
        private LuaScriptCalc luaScriptCalc = new LuaScriptCalc();
        public Renderer()
		{
		}
        public void StartRenderer(Bitmap src1, Bitmap src2, string script, RenderFinishEvent onRenderFinish)
        {
            Debug.WriteLine("StartRenderer()");
            interruptRendering = true;
            rendererSignal.WaitOne();
            interruptRendering = false;
            Debug.WriteLine("StartRenderer(); new thread;");
            renderThread = new Thread(() => RenderResult(src1, src2, script, onRenderFinish));
            rendererSignal.Reset();
            Debug.WriteLine("StartRenderer(); Start();");
            renderThread.Start();
        }

        private bool RenderResult(Bitmap src1, Bitmap src2, string script, RenderFinishEvent onRenderFinish)
        {
            Debug.WriteLine("RenderResult()");

            if ((src1 is null) || (src2 is null))
            {
                Debug.WriteLine("RenderResult(); return1");
                rendererSignal.Set();
                return false;
            }
            int x = Math.Min(src1.Width, src2.Width);
            int y = Math.Min(src1.Height, src2.Height);
            Bitmap resultImage = new Bitmap(x, y);

            Color c1, c2, cr;
            cr = Color.Black;
            string errorMessage = "";
            uint[] pixels1 = new uint[x * y];
            uint[] pixels2 = new uint[x * y];
            uint[] pixelsOut = new uint[x * y];
            for (y = 0; y < resultImage.Height; y++)
            {
                for (x = 0; x < resultImage.Width; x++)
                {
                    if (interruptRendering)
                    {
                        Debug.WriteLine("RenderResult(); return2");
                        rendererSignal.Set();
                        return false;
                    }
                    c1 = src1.GetPixel(x, y);
                    c2 = src2.GetPixel(x, y);
                    pixels1[y * resultImage.Width + x] = (uint)(c1.R + (c1.G << 8) + (c1.B << 16) + (c1.A << 24));
                    pixels2[y * resultImage.Width + x] = (uint)(c2.R + (c2.G << 8) + (c2.B << 16) + (c2.A << 24));
                }
            }
            if (!luaScriptCalc.LuaChangeColor(script, ref pixels1, ref pixels2, ref pixelsOut, ref errorMessage))
            {
                onRenderFinish(resultImage, errorMessage);
                Debug.WriteLine("RenderResult(); return3");
                rendererSignal.Set();
                return false;
            }
            uint i;
            for (y = 0; y < resultImage.Height; y++)
            {
                for (x = 0; x < resultImage.Width; x++)
                {
                    if (interruptRendering)
                    {
                        Debug.WriteLine("RenderResult(); return4");
                        rendererSignal.Set();
                        return false;
                    }
                    i = pixelsOut[y * resultImage.Width + x];
                    cr = Color.FromArgb((byte)(i >> 24), (byte)i, (byte)(i >> 8), (byte)(i >> 16));
                    resultImage.SetPixel(x, y, cr);
                }
            }
            onRenderFinish(resultImage, "Script processed successfully");
            Debug.WriteLine("RenderResult(); return5");
            rendererSignal.Set();
            return true;
        }
    }
}