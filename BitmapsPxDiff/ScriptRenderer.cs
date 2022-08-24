using System.Diagnostics;
using System.Drawing.Imaging;

namespace BitmapsPxDiff
{
    public delegate void OnRefreshRenderingProgressEvent(Bitmap newImage, string newStatus);
    public class ScriptRenderer
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
        //private List<object> sourceImagesLockers = new List<object>;
        private object[] sourceImagesLockers = new object[0];
        private static readonly object resultImgLocker = new object();

        // mainRenderThread variables:
        private Thread? mainRenderThread;
        private ManualResetEvent mainRenderThreadSignal = new ManualResetEvent(true); // blocks creating new thread until terminating previous calculations
        private bool interruptRendering = false; // signal for worker threads to interrupt

        private LuaScriptCalc luaScriptCalc = new LuaScriptCalc(); // LUA script interpreter

        // local copy of given resources, independent from the form thread
        private List<Bitmap> localSourceImages = new List<Bitmap> ();
        private Bitmap localResultImage = new Bitmap(1, 1);
        private Point localResultImageSize = new Point(1, 1);
        private string localScript = "";
        private List<string> threadsLogs = new List<string>();

        // rendering events:
        private OnRefreshRenderingProgressEvent onRefreshRenderingProgress; // an event allowing mainRenderThread to update progress and return results
        private Action onRenderingStarted;
        private Action onRenderingFinished;

        Stopwatch stopwatch = new Stopwatch();

        private bool _running = false; 
        public bool Running { get => _running; }

        public ScriptRenderer(OnRefreshRenderingProgressEvent onRefreshRenderingProgress, Action onRenderingStarted, Action onRenderingFinished)
        {
            _running = false;
            this.onRefreshRenderingProgress = onRefreshRenderingProgress;
            this.onRenderingStarted = onRenderingStarted;
            this.onRenderingFinished = onRenderingFinished;
        }
        public void StartRendering(List<Bitmap> sourceImages, string script)
        {
            StopRendering(); // interrupt running rendering
            interruptRendering = false;
            _running = true;

            stopwatch.Restart();

            // copying resources and event delegate:
            localSourceImages = new List<Bitmap>(sourceImages);
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

            if ((localSourceImages.Count==0) || (localSourceImages.Contains(null)))
            {
                mainRenderThreadSignal.Set();
                return false;
            }
            // defining source bitmaps intersection dimensions:
            localResultImageSize = Helpers.GetImagesSizeIntersection(localSourceImages);
            lock (resultImgLocker)
            {
                localResultImage = new Bitmap(localResultImageSize.X, localResultImageSize.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb); // initializing render result bitmap
            }

            sourceImagesLockers = new object[localSourceImages.Count];
            Array.Fill(sourceImagesLockers, new object());
            //sourceImagesLockers = (List<object>)Enumerable.Repeat(new object(), localSourceImages.Count);

            // preparing worker threads:
            for (int t = 0; t < maxChunksThreads; t++)
            {
                endOfWorkEvents[t] = new ManualResetEvent(true);
                threadsParams[t].errorOccurred = false;
                threadsParams[t].errorMessage = "";
                threadsParams[t].scriptLogs = new List<string>();
            }
            // preparing chunks coords/dimensions:
            ImageChunk[] chunks = GenerateImageChunks(localResultImageSize.X, localResultImageSize.Y);
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
            Bitmap[] thrSourceImages = new Bitmap[localSourceImages.Count];
            Bitmap thrResultImage = new Bitmap(chunk.width, chunk.height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] pixelsImagesARGB = new byte[chunk.width * chunk.height * localSourceImages.Count * 4]; // px / images / ARGB
            uint[] pixelsOut = new uint[chunk.width * chunk.height];
            ScriptEnvironmentVariables envVars = new ScriptEnvironmentVariables(chunk.startX, chunk.startY, chunk.lastX, chunk.lastY, localResultImageSize.X, localResultImageSize.Y, localSourceImages.Count);

            // getting bitmaps chunks and converting them to pixels arrays:
            for (int i = 0; i < thrSourceImages.Length; i++)
            {
                lock (sourceImagesLockers[i])
                {
                    thrSourceImages[i] = localSourceImages[i].Clone(new Rectangle(chunk.startX, chunk.startY, chunk.width, chunk.height), localSourceImages[i].PixelFormat);
                }
                BitmapToPixelsArray(ref thrSourceImages[i], ref chunk, ref pixelsImagesARGB, i);
            }
            
            if (interruptRendering) // interrupt signal check
            { 
                endOfWorkEvents[index].Set(); 
                return; 
            }

            // LUA script operations:
            if (!luaScriptCalc.LuaChangeColor(localScript, ref pixelsImagesARGB, ref pixelsOut, threadsParams[index].scriptLogs, envVars, ref errorMessage))
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
        private void BitmapToPixelsArray(ref Bitmap bmp, ref ImageChunk chunk, ref byte[] pixelsImagesARGB, int pixelsImageIndex)
        {
            Color c;
            int pxIndex, imageColorPos;
            for (int ty = 0; ty < chunk.height; ty++)
            {
                for (int tx = 0; tx < chunk.width; tx++)
                {
                    c = bmp.GetPixel(tx, ty);
                    pxIndex = ty * chunk.width + tx;
                    imageColorPos = pxIndex * pixelsImageIndex * 4 + pxIndex * 4;
                    pixelsImagesARGB[imageColorPos + 0] = c.A;
                    pixelsImagesARGB[imageColorPos + 1] = c.R;
                    pixelsImagesARGB[imageColorPos + 2] = c.G;
                    pixelsImagesARGB[imageColorPos + 3] = c.B;
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