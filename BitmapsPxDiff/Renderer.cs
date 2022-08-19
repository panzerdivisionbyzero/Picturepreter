using System.Diagnostics;

namespace BitmapsPxDiff
{
    public delegate void RenderFinishEvent(Bitmap newImage, string newStatus);
    public class Renderer
	{
        const int maxChunksThreads = 16;
        ManualResetEvent[] endOfWorkEvents = new ManualResetEvent[maxChunksThreads];
        private static readonly object src1Locker = new object();
        private static readonly object src2Locker = new object();
        private static readonly object resultImgLocker = new object();

        private Thread? renderThread;
        private ManualResetEvent rendererSignal = new ManualResetEvent(true); // blokuje utworzenie nowego watku, dopoki dziala poprzednie wywolanie
        private bool interruptRendering = false;
        private LuaScriptCalc luaScriptCalc = new LuaScriptCalc();

        private Bitmap? localSrc1;
        private Bitmap? localSrc2;
        private Bitmap? localResultImage;
        private string? localScript;
        RenderFinishEvent? localOnRenderFinish;

        Stopwatch stopwatch = new Stopwatch();

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

            stopwatch.Restart();

            localSrc1 = (Bitmap)src1.Clone();
            localSrc2 = (Bitmap)src2.Clone();
            localScript = script;
            localOnRenderFinish = onRenderFinish;
            renderThread = new Thread(() => RenderResult());
            rendererSignal.Reset();
            Debug.WriteLine("StartRenderer(); Start();");
            renderThread.Start();
        }
        private void ShowThreadResult(string scriptStatus)
        {
            if ((localOnRenderFinish is null) || (localResultImage is null)) return;
            TimeSpan ts = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);

            lock (resultImgLocker)
                localOnRenderFinish((Bitmap)localResultImage.Clone(), scriptStatus+"\r\nTime elapsed: "+ ts.ToString(@"hh\:mm\:ss\.fff"));
        }

        private struct ImageChunk { public int startX, endX, startY, endY, width, height; }
        private ImageChunk[] GenerateImageChunks(int imageWidth, int imageHeight)
        {
            int chunkSize = 32;
            int chunksX = (int)Math.Ceiling(Convert.ToDouble(imageWidth) / Convert.ToDouble(chunkSize));
            int chunksY = (int)Math.Ceiling(Convert.ToDouble(imageHeight) / Convert.ToDouble(chunkSize));
            ImageChunk[] chunks = new ImageChunk[chunksX * chunksY];
            for (int y = 0; y < chunksY; y++)
                for (int x = 0; x < chunksX; x++)
                {
                    //chunks[y * chunkSize + x] = new ImageChunk();
                    try
                    {
                        chunks[y * chunksX + x].startX = x * chunkSize;
                        chunks[y * chunksX + x].startY = y * chunkSize;
                        chunks[y * chunksX + x].endX = Math.Min(imageWidth - 1, (x + 1) * chunkSize - 1);
                        chunks[y * chunksX + x].endY = Math.Min(imageHeight - 1, (y + 1) * chunkSize - 1);
                        chunks[y * chunksX + x].width = chunks[y * chunksX + x].endX - chunks[y * chunksX + x].startX + 1;
                        chunks[y * chunksX + x].height = chunks[y * chunksX + x].endY - chunks[y * chunksX + x].startY + 1;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("\ny=" + y.ToString() + "\nx=" + x.ToString() + "\nchunks.Length=" + chunks.Length.ToString() + "\nchunksX=" + chunksX.ToString() + "\nchunksY=" + chunksY.ToString() + ";", e);
                    }
                }
            return chunks;
        }
        private struct ThreadParams
        {
            public ImageChunk chunk;
            public bool methodResult;
            public string errorMessage;
        }
        ThreadParams[] threadsParams = new ThreadParams[maxChunksThreads];

        private void ImageToPixelsArray(ref Bitmap bmp, ref ImageChunk chunk, ref uint[] pixels)
        {
            Color c;
            for (int ty = 0; ty < chunk.height; ty++)
            {
                for (int tx = 0; tx < chunk.width; tx++)
                {
                    
                    c = bmp.GetPixel(tx, ty);
                    pixels[ty * chunk.width + tx] = (uint)(c.R + (c.G << 8) + (c.B << 16) + (c.A << 24));
                }
            }
        }

        // https://stackoverflow.com/questions/7350679/convert-a-bitmap-into-a-byte-array
        /*public static uint[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (uint[])converter.ConvertTo(img, typeof(uint[]));
        }*/

        private void ThreadMethod(int threadIndex)
        {
            int index = Convert.ToInt32(threadIndex);
            ImageChunk chunk = threadsParams[index].chunk;
            Debug.WriteLine("thread(" + index.ToString() + "); threadIndex = " + threadIndex.ToString());
            Color c1, c2, cr;
            cr = Color.Black;
            string errorMessage = "";

            Bitmap thrSrc1; //= new Bitmap(chunk.endX - chunk.startX, chunk.endY - chunk.startY);
            Bitmap thrSrc2; //= new Bitmap(thrSrc1.Width, thrSrc1.Height);
            Bitmap thrResultImage = new Bitmap(chunk.width, chunk.height);

            Debug.WriteLine("thread(" + index.ToString() + "); before src1Locker");
            lock (src1Locker) { thrSrc1 = localSrc1.Clone(new Rectangle(chunk.startX, chunk.startY, chunk.width, chunk.height), localSrc1.PixelFormat); }
            Debug.WriteLine("thread(" + index.ToString() + "); after src1Locker");
            uint[] pixels1 = new uint[chunk.width * chunk.height];
            uint[] pixels2 = new uint[chunk.width * chunk.height];
            uint[] pixelsOut = new uint[chunk.width * chunk.height];

            if (interruptRendering)
            {
                Debug.WriteLine("thread(" + index.ToString() + "); Set1");
                endOfWorkEvents[index].Set();
                return;
            }
            ImageToPixelsArray(ref thrSrc1, ref chunk, ref pixels1);


            Debug.WriteLine("thread(" + index.ToString() + "); before src2Locker");
            lock (src2Locker) { thrSrc2 = localSrc2.Clone(new Rectangle(chunk.startX, chunk.startY, chunk.width, chunk.height), localSrc2.PixelFormat); }
            Debug.WriteLine("thread(" + index.ToString() + "); after src2Locker");

            ImageToPixelsArray(ref thrSrc2, ref chunk, ref pixels2);

            if (!luaScriptCalc.LuaChangeColor(localScript, ref pixels1, ref pixels2, ref pixelsOut, ref errorMessage))
            {
                threadsParams[index].errorMessage = errorMessage;
                threadsParams[index].methodResult = false;
                Debug.WriteLine("thread(" + index.ToString() + "); Set2");
                endOfWorkEvents[index].Set();
                return;
            }
            uint i;
            for (int ty = 0; ty < chunk.height; ty++)
            {
                for (int tx = 0; tx < chunk.width; tx++)
                {
                    if (interruptRendering)
                    {
                        Debug.WriteLine("thread(" + index.ToString() + "); Set3");
                        endOfWorkEvents[index].Set();
                        return;
                    }
                    i = pixelsOut[ty * chunk.width + tx];
                    cr = Color.FromArgb((byte)(i >> 24), (byte)i, (byte)(i >> 8), (byte)(i >> 16));
                    thrResultImage.SetPixel(tx, ty, cr);
                }
            }
            Debug.WriteLine("thread(" + index.ToString() + "); before resultImgLocker");
            lock (resultImgLocker)
            {
                Graphics g = Graphics.FromImage(localResultImage);
                g.DrawImage(thrResultImage, chunk.startX, chunk.startY);
            }
            /*onRenderFinish(resultImage, "Script processed successfully");
            Debug.WriteLine("RenderResult(); return5");
            rendererSignal.Set();*/
            threadsParams[index].methodResult = true;
            Debug.WriteLine("thread(" + index.ToString() + "); Set4");
            endOfWorkEvents[index].Set();
        }

        private bool RenderResult()
        {
            Debug.WriteLine("RenderResult()");

            if ((localSrc1 is null) || (localSrc2 is null))
            {
                Debug.WriteLine("RenderResult(); return1");
                rendererSignal.Set();
                return false;
            }
            int x = Math.Min(localSrc1.Width, localSrc2.Width);
            int y = Math.Min(localSrc1.Height, localSrc2.Height);
            lock (resultImgLocker)
                localResultImage = new Bitmap(x, y);

            for (int t = 0; t < maxChunksThreads; t++)
            {
                endOfWorkEvents[t] = new ManualResetEvent(true);
                threadsParams[t].methodResult = true; // initial value before thread start
                threadsParams[t].errorMessage = "";
            }
            ImageChunk[] chunks = GenerateImageChunks(x, y);

            Debug.WriteLine("RenderResult(); before for [t]");

            for (int t = 0; t < chunks.Length; t++)
            {
                if (interruptRendering)
                {
                    Debug.WriteLine("RenderResult; return2; before WaitAll()");
                    WaitHandle.WaitAll(endOfWorkEvents);
                    Debug.WriteLine("RenderResult; return2; after WaitAll()");
                    rendererSignal.Set();
                    return false;
                }
                Debug.WriteLine("RenderResult(); t="+t.ToString());
                int threadIndex = WaitHandle.WaitAny(endOfWorkEvents);
                if (!threadsParams[threadIndex].methodResult) break;
                ShowThreadResult("Processing...");
                Debug.WriteLine("RenderResult(); threadIndex="+threadIndex.ToString());
                threadsParams[threadIndex].chunk = chunks[t];
                endOfWorkEvents[threadIndex].Reset();

                new Thread(() => ThreadMethod(threadIndex)).Start();
            } // for [t]
            Debug.WriteLine("RenderResult(); before WaitAll()");
            WaitHandle.WaitAll(endOfWorkEvents);
            Debug.WriteLine("RenderResult(); after WaitAll()");
            foreach (var tp in threadsParams)
            {
                if (!tp.methodResult)
                {
                    stopwatch.Stop();
                    ShowThreadResult(tp.errorMessage);
                    //tbScriptOutput.Text = tp.errorMessage;
                    Debug.WriteLine("RenderResult; return3");
                    rendererSignal.Set();
                    return false;
                }
            }
            //lock (threadsResultImage) resultImage = (Bitmap)threadsResultImage.Clone();
            //tbScriptOutput.Text = "Script processed successfully";
            stopwatch.Stop();
            ShowThreadResult("Script processed successfully");
            Debug.WriteLine("RenderResult; return4");
            rendererSignal.Set();
            return true;
        }
    }
}