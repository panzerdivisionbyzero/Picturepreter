using System.Diagnostics;
using System.Drawing.Imaging;

namespace BitmapsPxDiff
{
    public delegate void OnRefreshRenderingProgressEvent(Bitmap newImage, string newStatus);
    public class Renderer
	{
        // structs:
        private struct ImageChunk { public int startX, lastX, startY, lastY, width, height; }
        private struct ThreadParams
        {
            public ImageChunk chunk;
            public bool errorOccurred;
            public string errorMessage;
            public List<string> scriptLogs;
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
        private int localResultImageWidth, localResultImageHeight = 1;
        private string localScript = "";
        private List<string> threadsLogs = new List<string>();

        // rendering events:
        private OnRefreshRenderingProgressEvent onRefreshRenderingProgress; // an event allowing mainRenderThread to update progress and return results
        private Action onRenderingStarted;
        private Action onRenderingFinished;

        Stopwatch stopwatch = new Stopwatch();

        private bool _running = false; 
        public bool Running { get => _running; }

        public Renderer(OnRefreshRenderingProgressEvent onRefreshRenderingProgress, Action onRenderingStarted, Action onRenderingFinished)
        {
            _running = false;
            this.onRefreshRenderingProgress = onRefreshRenderingProgress;
            this.onRenderingStarted = onRenderingStarted;
            this.onRenderingFinished = onRenderingFinished;
        }
        public void StartRendering(Bitmap src1, Bitmap src2, string script)
        {
            StopRendering(); // interrupt running rendering
            interruptRendering = false;
            _running = true;

            stopwatch.Restart();

            // copying resources and event delegate:
            localSrc1 = (Bitmap)src1.Clone();
            localSrc2 = (Bitmap)src2.Clone();
            localScript = script;

            // ready, steady, go:
            mainRenderThread = new Thread(() => MainRenderThreadJob());
            mainRenderThreadSignal.Reset();
            mainRenderThread.Start();
        }
        public void StopRendering()
        {
            bool wasRunning = _running;
            interruptRendering = true; // interrupt running rendering
            mainRenderThreadSignal.WaitOne(); // wait for threads to terminate (mainRenderThread waits for all workers first)
            _running = false;
        }
        private bool MainRenderThreadJob()
        {
            if (onRenderingStarted != null)
            {
                onRenderingStarted();
            }

            if ((localSrc1 is null) || (localSrc2 is null))
            {
                mainRenderThreadSignal.Set();
                return false;
            }
            // defining source bitmaps intersection dimensions:
            localResultImageWidth = Math.Min(localSrc1.Width, localSrc2.Width);
            localResultImageHeight = Math.Min(localSrc1.Height, localSrc2.Height);
            lock (resultImgLocker)
            {
                localResultImage = new Bitmap(localResultImageWidth, localResultImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb); // initializing render result bitmap
            }

            // preparing worker threads:
            for (int t = 0; t < maxChunksThreads; t++)
            {
                endOfWorkEvents[t] = new ManualResetEvent(true);
                threadsParams[t].errorOccurred = false;
                threadsParams[t].errorMessage = "";
                threadsParams[t].scriptLogs = new List<string>();
            }
            // preparing chunks coords/dimensions:
            ImageChunk[] chunks = GenerateImageChunks(localResultImageWidth, localResultImageHeight);
            threadsLogs.Clear();

            // calculating chunks by worker threads:
            for (int t = 0; t < chunks.Length; t++)
            {
                if (interruptRendering)
                {
                    break;
                }
                int threadIndex = WaitHandle.WaitAny(endOfWorkEvents); // get first available worker index

                if (threadsParams[threadIndex].errorOccurred)
                {
                    break; // if previous worker reported error, interrupt loop
                }
                TryGetAndClearThreadLogs(threadsParams[threadIndex].scriptLogs);

                RefreshRenderingProgress("Processing... "+Math.Round((double)t/chunks.Length*100).ToString()+"%");

                threadsParams[threadIndex].chunk = chunks[t]; // assign chunk to worker
                endOfWorkEvents[threadIndex].Reset();

                new Thread(() => WorkerThreadJob(threadIndex)).Start(); // create and start thread
            }
            WaitHandle.WaitAll(endOfWorkEvents); // wait for all workers to terminate

            // scanning workers results for errors, extracting logs:
            bool errorOcurred = false;
            string scriptStatus = "Script processed successfully";
            foreach (var tp in threadsParams)
            {
                TryGetAndClearThreadLogs(tp.scriptLogs);

                if (tp.errorOccurred && !errorOcurred) // get first found thread error
                {
                    errorOcurred = true;
                    scriptStatus = tp.errorMessage;
                }
            }
            if (threadsLogs.Count>0)
            {
                scriptStatus += "\r\nThreads logs:\r\n" + string.Join("\r\n", threadsLogs.ToArray());
            }
            // render finish:
            stopwatch.Stop();
            RefreshRenderingProgress(scriptStatus);
            if (onRenderingFinished != null)
            {
                onRenderingFinished();
            }
            mainRenderThreadSignal.Set();
            _running = false;
            return !errorOcurred;
        }
        private void TryGetAndClearThreadLogs(List<string> threadLogs)
        {
            if (threadLogs.Count > 0)
            {
                threadsLogs.AddRange(threadLogs);
                threadLogs.Clear();
            }
        }
        private void RefreshRenderingProgress(string scriptStatus)
        {
            if ((onRefreshRenderingProgress is null) || (localResultImage is null))
            {
                return;
            }
            TimeSpan ts = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            lock (resultImgLocker)
            {
                onRefreshRenderingProgress((Bitmap)localResultImage.Clone(), scriptStatus + "\r\nTime elapsed: " + ts.ToString(@"hh\:mm\:ss\.fff"));
            }
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
                    chunks[c].lastX = Math.Min(imageWidth - 1, (x + 1) * chunkSize - 1);
                    chunks[c].lastY = Math.Min(imageHeight - 1, (y + 1) * chunkSize - 1);
                    chunks[c].width = chunks[c].lastX - chunks[c].startX + 1;
                    chunks[c].height = chunks[c].lastY - chunks[c].startY + 1;
                }
            }
            return chunks;
        }
        private void WorkerThreadJob(int threadIndex)
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
            ScriptEnvironmentVariables envVars = new ScriptEnvironmentVariables(chunk.startX, chunk.startY, chunk.lastX, chunk.lastY, localResultImageWidth, localResultImageHeight);

            // getting bitmaps chunks and converting them to pixels arrays:
            lock (src1Locker)
            {
                thrSrc1 = localSrc1.Clone(new Rectangle(chunk.startX, chunk.startY, chunk.width, chunk.height), localSrc1.PixelFormat);
            }
            BitmapToPixelsArray(ref thrSrc1, ref chunk, ref pixels1);

            lock (src2Locker)
            {
                thrSrc2 = localSrc2.Clone(new Rectangle(chunk.startX, chunk.startY, chunk.width, chunk.height), localSrc2.PixelFormat);
            }
            BitmapToPixelsArray(ref thrSrc2, ref chunk, ref pixels2);
            
            if (interruptRendering) // interrupt signal check
            { 
                endOfWorkEvents[index].Set(); 
                return; 
            }

            // LUA script operations:
            if (!luaScriptCalc.LuaChangeColor(localScript, ref pixels1, ref pixels2, ref pixelsOut, threadsParams[index].scriptLogs, envVars, ref errorMessage))
            {
                threadsParams[index].errorMessage = errorMessage;
                threadsParams[index].errorOccurred = true;
                endOfWorkEvents[index].Set();
                return;
            }
            PixelArrayToBitmap(ref pixelsOut, ref chunk, ref thrResultImage); // converting script result to bitmap

            if (interruptRendering) // interrupt signal check
            {
                endOfWorkEvents[index].Set();
                return;
            }

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