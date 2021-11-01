using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.Xna.Framework.GamerServices
{
    public class GamerServicesDispatcher 
    {
        // Token: 0x04000175 RID: 373
       // private static UserPacketBuffer packetBuffer;
        public static bool IsInitialized
        {
            get
            {
                UnityEngine.Debug.Log(">");
                return true;
                //return GamerServicesDispatcher.packetBuffer != null;
            }
        }
    }
}
