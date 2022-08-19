using System.Diagnostics;
using System.Drawing.Imaging;

namespace BitmapsPxDiff
{
    public delegate void RenderFinishEvent(Bitmap newImage, string newStatus);
    public class Renderer
	{
        // structs:
        private struct ImageChunk { public int startX, endX, startY, endY, width, height; }
        private struct ThreadParams
        {
            public ImageChunk chunk;
            public bool errorOccurred;
            public string errorMessage;
        }

        // threads variables:
        const int maxChunksThreads = 16;
        ManualResetEvent[] endOfWorkEvents = new ManualResetEvent[maxChunksThreads];
        ThreadParams[] threadsParams = new ThreadParams[maxChunksThreads];

        // worker threads shared resources lockers:
        private static readonly object src1Locker = new object();
        private static readonly object src2Locker = new object();
        private static readonly object resultImgLocker = new object();

        // mainRenderThread variables:
        private Thread? mainRenderThread;
        private ManualResetEvent mainRenderThreadSignal = new ManualResetEvent(true); // blocks creating new thread until terminating previous calculations
        private bool interruptRendering = false; // signal for worker threads to interrupt

        private LuaScriptCalc luaScriptCalc = new LuaScriptCalc();

        // local copy of given resources, independent from the form thread
        private Bitmap localSrc1 = new Bitmap(1, 1);
        private Bitmap localSrc2 = new Bitmap(1, 1);
        private Bitmap localResultImage = new Bitmap(1,1);
        private string localScript = "";
        RenderFinishEvent? localOnRenderFinish; // an event allowing mainRenderThread to update progress and return results

        Stopwatch stopwatch = new Stopwatch();

        private bool _running = false; 
        public bool Running { get => _running; }

        public Renderer()
		{
            _running = false;
		}
        public void StartRendering(Bitmap src1, Bitmap src2, string script, RenderFinishEvent onRenderFinish)
        {
            StopRendering(); // interrupt running rendering
            interruptRendering = false;
            _running = true;

            stopwatch.Restart();

            // copying resources and event delegate:
            localSrc1 = (Bitmap)src1.Clone();
            localSrc2 = (Bitmap)src2.Clone();
            localScript = script;
            localOnRenderFinish = onRenderFinish;

            // ready, steady, go:
            mainRenderThread = new Thread(() => RenderResult());
            mainRenderThreadSignal.Reset();
            mainRenderThread.Start();
        }
        public void StopRendering()
        {
            interruptRendering = true; // interrupt running rendering
            mainRenderThreadSignal.WaitOne(); // wait for threads to terminate (mainRenderThread waits for all workers first)
            _running = false;
        }
        private bool RenderResult()
        {
            if ((localSrc1 is null) || (localSrc2 is null))
            {
                mainRenderThreadSignal.Set();
                return false;
            }
            // defining source bitmaps intersection dimensions:
            int x = Math.Min(localSrc1.Width, localSrc2.Width);
            int y = Math.Min(localSrc1.Height, localSrc2.Height);
            lock (resultImgLocker)
                localResultImage = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb); // initializing render result bitmap

            // preparing worker threads:
            for (int t = 0; t < maxChunksThreads; t++)
            {
                endOfWorkEvents[t] = new ManualResetEvent(true);
                threadsParams[t].errorOccurred = false;
                threadsParams[t].errorMessage = "";
            }
            // preparing chunks coords/dimensions:
            ImageChunk[] chunks = GenerateImageChunks(x, y);

            // calculating chunks by worker threads:
            for (int t = 0; t < chunks.Length; t++)
            {
                if (interruptRendering)
                {
                    WaitHandle.WaitAll(endOfWorkEvents);
                    mainRenderThreadSignal.Set();
                    return false;
                }
                int threadIndex = WaitHandle.WaitAny(endOfWorkEvents); // get first available worker index

                if (threadsParams[threadIndex].errorOccurred) break; // if previous worker reported error, interrupt loop

                RefreshRenderingProgress("Processing...");

                threadsParams[threadIndex].chunk = chunks[t]; // assign chunk to worker
                endOfWorkEvents[threadIndex].Reset();

                new Thread(() => ThreadMethod(threadIndex)).Start(); // create and start thread
            }
            WaitHandle.WaitAll(endOfWorkEvents); // wait for all workers to terminate
            // scanning workers results for errors:
            foreach (var tp in threadsParams)
            {
                if (tp.errorOccurred)
                {
                    stopwatch.Stop();
                    RefreshRenderingProgress(tp.errorMessage);
                    mainRenderThreadSignal.Set();
                    return false;
                }
            }
            // render finish:
            stopwatch.Stop();
            RefreshRenderingProgress("Script processed successfully");
            mainRenderThreadSignal.Set();
            return true;
        }
        private void RefreshRenderingProgress(string scriptStatus)
        {
            if ((localOnRenderFinish is null) || (localResultImage is null)) return;
            TimeSpan ts = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            lock (resultImgLocker)
                localOnRenderFinish((Bitmap)localResultImage.Clone(), scriptStatus + "\r\nTime elapsed: " + ts.ToString(@"hh\:mm\:ss\.fff"));
        }
        private ImageChunk[] GenerateImageChunks(int imageWidth, int imageHeight)
        {
            int chunkSize = 32;
            int chunksX = (int)Math.Ceiling(Convert.ToDouble(imageWidth) / Convert.ToDouble(chunkSize));
            int chunksY = (int)Math.Ceiling(Convert.ToDouble(imageHeight) / Convert.ToDouble(chunkSize));
            ImageChunk[] chunks = new ImageChunk[chunksX * chunksY];
            for (int y = 0; y < chunksY; y++)
            {
                for (int x = 0; x < chunksX; x++)
                {
                    int c = y * chunksX + x;
                    chunks[c].startX = x * chunkSize;
                    chunks[c].startY = y * chunkSize;
                    chunks[c].endX = Math.Min(imageWidth - 1, (x + 1) * chunkSize - 1);
                    chunks[c].endY = Math.Min(imageHeight - 1, (y + 1) * chunkSize - 1);
                    chunks[c].width = chunks[c].endX - chunks[c].startX + 1;
                    chunks[c].height = chunks[c].endY - chunks[c].startY + 1;
                }
            }
            return chunks;
        }
        private void ThreadMethod(int threadIndex)
        {
            // setting up viariables and buffers:
            int index = Convert.ToInt32(threadIndex);
            ImageChunk chunk = threadsParams[index].chunk;
            string errorMessage = "";
            Bitmap thrSrc1;
            Bitmap thrSrc2;
            Bitmap thrResultImage = new Bitmap(chunk.width, chunk.height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            uint[] pixels1 = new uint[chunk.width * chunk.height];
            uint[] pixels2 = new uint[chunk.width * chunk.height];
            uint[] pixelsOut = new uint[chunk.width * chunk.height];

            // getting bitmaps chunks and converting them to pixels arrays:
            lock (src1Locker) { thrSrc1 = localSrc1.Clone(new Rectangle(chunk.startX, chunk.startY, chunk.width, chunk.height), localSrc1.PixelFormat); }
            BitmapToPixelsArray(ref thrSrc1, ref chunk, ref pixels1);
            lock (src2Locker) { thrSrc2 = localSrc2.Clone(new Rectangle(chunk.startX, chunk.startY, chunk.width, chunk.height), localSrc2.PixelFormat); }
            BitmapToPixelsArray(ref thrSrc2, ref chunk, ref pixels2);
            
            if (interruptRendering) { endOfWorkEvents[index].Set(); return; } // interrupt signal check

            // LUA script operations:
            if (!luaScriptCalc.LuaChangeColor(localScript, ref pixels1, ref pixels2, ref pixelsOut, ref errorMessage))
            {
                threadsParams[index].errorMessage = errorMessage;
                threadsParams[index].errorOccurred = true;
                endOfWorkEvents[index].Set();
                return;
            }
            PixelArrayToBitmap(ref pixelsOut, ref chunk, ref thrResultImage); // converting script result to bitmap

            if (interruptRendering) { endOfWorkEvents[index].Set(); return; } // interrupt signal check

            // pasting thread result into the aggregate bitmap
            lock (resultImgLocker)
            {
                Graphics g = Graphics.FromImage(localResultImage);
                g.DrawImage(thrResultImage, chunk.startX, chunk.startY);
                g.Dispose();
            }
            endOfWorkEvents[index].Set();
        }
        private void BitmapToPixelsArray(ref Bitmap bmp, ref ImageChunk chunk, ref uint[] pixels)
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
        private void PixelArrayToBitmap(ref uint[] pixels, ref ImageChunk chunk, ref Bitmap bmp)
        {
            uint i;
            Color c;
            for (int ty = 0; ty < chunk.height; ty++)
            {
                for (int tx = 0; tx < chunk.width; tx++)
                {
                    i = pixels[ty * chunk.width + tx];
                    c = Color.FromArgb((byte)(i >> 24), (byte)i, (byte)(i >> 8), (byte)(i >> 16));
                    bmp.SetPixel(tx, ty, c);
                }
            }
        }
    }
}