using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Microsoft.Xna.Framework.Content.Pipeline.Processors
{
    public class ContentProcessorContext
    {
        public Log Logger;
        public class Log
        {
            internal void LogMessage(string v1, byte version, string v2, decimal v3, ushort frameCount, uint length, int v4, int v5)
            {
                Debug.Log(v1);
            }

            internal void LogMessage(string v, object  count=null, object elapsedMilliseconds=null)
            {
                Debug.Log(v);
            }
        }
    }
}
