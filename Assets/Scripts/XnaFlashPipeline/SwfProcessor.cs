using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using XnaFlash.Swf;

namespace XnaFlash.Pipeline
{
    //[ContentProcessor(DisplayName = "SWF Processor - XnaFlash")]
    public class SwfProcessor : ContentProcessor<byte[], byte[]>
    {
        public bool IgnoreCorrupted { get; set; }

        public override byte[] Process(byte[] input, ContentProcessorContext context)
        {
            if (!IgnoreCorrupted)
            {
                // Check SWF validity
                using (var mem = new MemoryStream(input))
                {
                    var swatch = new Stopwatch();
                    swatch.Start();

                    SwfStream stream = new SwfStream(mem);
                    int count = 0;
                    foreach (var t in stream.ProcessFile()) count++;

                    swatch.Stop();
                    context.Logger.LogMessage("SWF - Version {0}, {1}, FPS: {2}, {3} frames, {4} bytes, {5}x{6} px.",
                        stream.Version,
                        stream.Compressed ? "Compressed" : "Plain",
                        decimal.Round(stream.FrameRate, 2),
                        stream.FrameCount,
                        stream.Length,
                        stream.Rectangle.Width / 20,
                        stream.Rectangle.Height / 20);
                    context.Logger.LogMessage("Loaded {0} tags in {1} ms.", count, swatch.ElapsedMilliseconds);

                    foreach (var t in stream.GetSkippedTags().OrderBy(s => s))
                        context.Logger.LogMessage("Skipped not implemented {0}!", t);
                }
            }
                     
            // Return original data
            return input;
        }
    }
}
