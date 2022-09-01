using System.Diagnostics;
using System.Drawing.Imaging;

namespace BitmapsPxDiff
{
    public delegate void OnChunkFinishedEvent(Bitmap newImage, string newStatus);
    /// <summary>
    /// Asynchronously performs operations written in given LUA script on source images and returns result image;
    /// </summary>
    public class ScriptRenderer
	{
        // structs:
        /// <summary>
        /// ImageChunk structure stores coordinates and size of image chunk intended for single thread processing;
        /// </summary>
        private struct ImageChunk { public int startX, lastX, startY, lastY, width, height; }
        /// <summary>
        /// ThreadDataSet structure contains input parameters (chunk) and output variables, assigned to each single thread;
        /// </summary>
        private struct ThreadDataSet
        {
            public ImageChunk chunk;
            public bool errorOccurred;
            public string errorMessage;
            public List<string> scriptLogs;
        }

        // threads variables:
        const int chunkSize = 32;
        const int maxChunksThreads = 16;
        ManualResetEvent[] endOfWorkEvents = new ManualResetEvent[maxChunksThreads];
        ThreadDataSet[] threadsDataSets = new ThreadDataSet[maxChunksThreads];

        // worker threads shared resources lockers:
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
        private OnChunkFinishedEvent onChunkFinished; // an event allowing mainRenderThread to update progress and return results
        private Action onRenderingStarted;
        private Action onRenderingFinished;

        Stopwatch stopwatch = new Stopwatch();

        private bool _running = false; 
        public bool Running { get => _running; }

        /// <summary>
        /// Class constructor; Gets methods delegates and assigns it to events;
        /// </summary>
        /// <param name="onChunkFinished">Event called for update result image preview when one of chunks processing is finished</param>
        /// <param name="onRenderingStarted">Event called at mainRenderThread startup</param>
        /// <param name="onRenderingFinished">Event called at mainRenderThread finish (or interruption)</param>
        public ScriptRenderer(OnChunkFinishedEvent onChunkFinished, Action onRenderingStarted, Action onRenderingFinished)
        {
            this.onChunkFinished = onChunkFinished;
            this.onRenderingStarted = onRenderingStarted;
            this.onRenderingFinished = onRenderingFinished;
        }
        /// <summary>
        /// Interrupts ongoing processing (if running), clones given images to localSourceImages and initiates mainRenderThread;
        /// </summary>
        /// <param name="sourceImages">Source images to be processed by LUA script</param>
        /// <param name="script">LUA script written by user</param>
        public void StartRendering(List<Bitmap> sourceImages, string script)
        {
            StopRendering(); // interrupt running rendering
            interruptRendering = false;
            _running = true;

            stopwatch.Restart();

            // copying resources and event delegate:
            localSourceImages.Clear();
            foreach (Bitmap sourceImage in sourceImages)
            {
                localSourceImages.Add((Bitmap)sourceImage.Clone());
            }
            localScript = script;

            // ready, steady, go:
            mainRenderThread = new Thread(() => MainRenderThreadJob());
            mainRenderThreadSignal.Reset();
            mainRenderThread.Start();
        }
        /// <summary>
        /// Interrupts ongoing processing and waits for threads to terminate;
        /// </summary>
        public void StopRendering()
        {
            bool wasRunning = _running;
            interruptRendering = true; // interrupt running rendering
            mainRenderThreadSignal.WaitOne(); // wait for threads to terminate (mainRenderThread waits for all workers first)
            _running = false;
        }
        /// <summary>
        /// Manages entire rendering process:
        /// - prepares localResultImage canvas;
        /// - prepares chunks coordinates;
        /// - prepares and manages worker threads;
        /// - checks worker threads results for errors, collects their logs;
        /// </summary>
        /// <returns>"true" if processing completed successful; "false" if error occured;</returns>
        private bool MainRenderThreadJob()
        {
            if (onRenderingStarted != null) {
                onRenderingStarted();
            }
            if ((localSourceImages.Count==0) || (localSourceImages.Contains(null))) {
                mainRenderThreadSignal.Set();
                return false;
            }
            // defining source bitmaps intersection dimensions:
            localResultImageSize = Helpers.GetImagesSizeIntersection(localSourceImages);
            lock (resultImgLocker) {
                localResultImage = new Bitmap(localResultImageSize.X, localResultImageSize.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb); // initializing render result bitmap
            }

            sourceImagesLockers = new object[localSourceImages.Count];
            Array.Fill(sourceImagesLockers, new object());

            // preparing worker threads:
            for (int t = 0; t < maxChunksThreads; t++)
            {
                endOfWorkEvents[t] = new ManualResetEvent(true);
                threadsDataSets[t].errorOccurred = false;
                threadsDataSets[t].errorMessage = "";
                threadsDataSets[t].scriptLogs = new List<string>();
            }
            // preparing chunks coords/dimensions:
            ImageChunk[] chunks = GenerateImageChunks(localResultImageSize.X, localResultImageSize.Y);
            threadsLogs.Clear();

            // calculating chunks by worker threads:
            for (int t = 0; t < chunks.Length; t++)
            {
                if (interruptRendering) break;

                int threadIndex = WaitHandle.WaitAny(endOfWorkEvents); // get first available worker index

                if (threadsDataSets[threadIndex].errorOccurred) break; // if previous worker reported error, interrupt loop

                TryGetAndClearThreadLogs(threadsDataSets[threadIndex].scriptLogs);

                RefreshRenderingProgress("Processing... "+Math.Round((double)t/chunks.Length*100).ToString()+"%");

                threadsDataSets[threadIndex].chunk = chunks[t]; // assign chunk to worker
                endOfWorkEvents[threadIndex].Reset();

                new Thread(() => WorkerThreadJob(threadIndex)).Start(); // create and start thread
            }
            WaitHandle.WaitAll(endOfWorkEvents); // wait for all workers to terminate

            // scanning workers results for errors, extracting logs:
            bool errorOcurred = false;
            string scriptStatus = "Script processed successfully";
            foreach (var tp in threadsDataSets)
            {
                TryGetAndClearThreadLogs(tp.scriptLogs);

                if (tp.errorOccurred && !errorOcurred) // get first found thread error
                {
                    errorOcurred = true;
                    scriptStatus = tp.errorMessage;
                }
            }
            if (threadsLogs.Count>0) {
                scriptStatus += "\r\nThreads logs:\r\n" + string.Join("\r\n", threadsLogs.ToArray());
            }
            // render finish:
            stopwatch.Stop();
            RefreshRenderingProgress(scriptStatus);
            if (onRenderingFinished != null) {
                onRenderingFinished();
            }
            mainRenderThreadSignal.Set();
            _running = false;
            return !errorOcurred;
        }
        /// <summary>
        /// Gathers threadLogs[] param content into this.threadsLogs[] and clears given threadLogs[] list;
        /// </summary>
        /// <param name="threadLogs">Single thread logs list</param>
        private void TryGetAndClearThreadLogs(List<string> threadLogs)
        {
            if (threadLogs.Count > 0)
            {
                threadsLogs.AddRange(threadLogs);
                threadLogs.Clear();
            }
        }
        /// <summary>
        /// Calls onChunkFinished() event with use of resultImgLocker; Event passes scriptStatus, elapsed time and localResultImage clone;
        /// </summary>
        /// <param name="scriptStatus">Status tu display on form script output; "Time elapsed" counter is automatically added;</param>
        private void RefreshRenderingProgress(string scriptStatus)
        {
            if ((onChunkFinished is null) || (localResultImage is null)) return;

            TimeSpan ts = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            lock (resultImgLocker)
            {
                onChunkFinished((Bitmap)localResultImage.Clone(), scriptStatus + "\r\nTime elapsed: " + ts.ToString(@"hh\:mm\:ss\.fff"));
            }
        }
        /// <summary>
        /// Creates chunks array (dividing image surface to small fragments);
        /// </summary>
        /// <param name="imageWidth">Source images intersection width</param>
        /// <param name="imageHeight">Source images intersection height</param>
        /// <returns>Calculated chunks[]</returns>
        private ImageChunk[] GenerateImageChunks(int imageWidth, int imageHeight)
        {
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
        /// <summary>
        /// Performs single worker thread operations:
        /// - converts source images chunks into bytes array (pixelsImagesARGB[]);
        /// - initiates script processing (luaScriptCalc.LuaChangeColor());
        /// - converts result bytes array into Bitmap (thrResultImage);
        /// - pastes thrResultImage into localResultImage with use of resultImgLocker;
        /// </summary>
        /// <param name="threadIndex">Assigns thread to threadsDataSets[] and endOfWorkEvents[]</param>
        private void WorkerThreadJob(int threadIndex)
        {
            // setting up viariables and buffers:
            int index = Convert.ToInt32(threadIndex);
            ImageChunk chunk = threadsDataSets[index].chunk;
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
                BitmapToPixelsArray(ref thrSourceImages[i], ref chunk, ref pixelsImagesARGB, i, thrSourceImages.Length);
            }

            if (interruptRendering) // interrupt signal check
            { 
                endOfWorkEvents[index].Set(); 
                return; 
            }
            // LUA script operations:
            if (!luaScriptCalc.LuaChangeColor(localScript, ref pixelsImagesARGB, ref pixelsOut, threadsDataSets[index].scriptLogs, envVars, ref errorMessage))
            {
                threadsDataSets[index].errorMessage = errorMessage;
                threadsDataSets[index].errorOccurred = true;
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
        /// <summary>
        /// Adds Bitmap pixels of given chunk area to pixelsImagesARGB[] in following order:
        /// px0_img0_A, px0_img0_R, px0_img0_G, px0_img0_B, px0_img1_A, px0_img1_R, px0_img1_G, px0_img1_B, ..., px0_imgN_A, px0_imgN_R, px0_imgN_G, px0_imgN_B,
        /// px1_img0_A, px1_img0_R, px1_img0_G, px1_img0_B, px1_img1_A, px1_img1_R, px1_img1_G, px1_img1_B, ..., px1_imgN_A, px1_imgN_R, px1_imgN_G, px1_imgN_B,
        /// ...       , ...       , ...       , ...       , ...       , ...       , ...       , ...       , ..., ...       , ...       , ...       , ...       , 
        /// pxK_img0_A, pxK_img0_R, pxK_img0_G, pxK_img0_B, pxK_img1_A, pxK_img1_R, pxK_img1_G, pxK_img1_B, ..., pxK_imgN_A, pxK_imgN_R, pxK_imgN_G, pxK_imgN_B,
        /// where:
        /// - px is following pixels positions (K = Width * Height - 1),
        /// - img is source image (N source images),
        /// - A/R/G/B represents each pixel values,
        /// - pixelsImagesARGB[] is one-dimensional array;
        /// This is the fastest way to "unpack" it by LUA script;
        /// </summary>
        /// <param name="bmp">One of source images to be written into pixels array</param>
        /// <param name="chunk">Specifies area to be copied from source image (and later processed by thread)</param>
        /// <param name="pixelsImagesARGB">Result array gathering following Bitmaps pixels</param>
        /// <param name="sourceImageIndex">Currently converted image index; needed to calculate pixelsImagesARGB[] destination index</param>
        /// <param name="imagesCount">All source images number; needed to calculate pixelsImagesARGB[] destination index</param>
        private void BitmapToPixelsArray(ref Bitmap bmp, ref ImageChunk chunk, ref byte[] pixelsImagesARGB, int sourceImageIndex, int imagesCount)
        {
            Color c;
            int pxIndex, imageColorPos;
            for (int ty = 0; ty < chunk.height; ty++)
            {
                for (int tx = 0; tx < chunk.width; tx++)
                {
                    c = bmp.GetPixel(tx, ty);
                    pxIndex = (ty * chunk.width + tx) * imagesCount;
                    imageColorPos = (pxIndex + sourceImageIndex) * 4;
                    pixelsImagesARGB[imageColorPos + 0] = c.A;
                    pixelsImagesARGB[imageColorPos + 1] = c.R;
                    pixelsImagesARGB[imageColorPos + 2] = c.G;
                    pixelsImagesARGB[imageColorPos + 3] = c.B;
                }
            }
        }
        /// <summary>
        /// Gets pixels values from "pixels[]" array and pastes it into "bmp" Bitmap;
        /// </summary>
        /// <param name="pixels">Pixels values to be pasted into "bmp" Bitmap</param>
        /// <param name="chunk">Destination "bmp" area; informs how to address pixels[] values</param>
        /// <param name="bmp">Result bitmap gathering chunks pixels[]</param>
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