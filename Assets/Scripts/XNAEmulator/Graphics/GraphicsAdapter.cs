using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{
    // Token: 0x02000088 RID: 136
    public sealed class GraphicsAdapter
    {
        // Token: 0x06000295 RID: 661 RVA: 0x0000A0E8 File Offset: 0x000094E8
        // Note: this type is marked as 'beforefieldinit'.
        static GraphicsAdapter()
        {
            GraphicsAdapter.InitalizeGraphics();
            GraphicsAdapter.InitializeAdapterList();
        }

        private static void InitializeAdapterList()
        {
            //throw new NotImplementedException();
            UnityEngine.Debug.Log("InitializeAdapterList");
        }

        private static void InitalizeGraphics()
        {
            //throw new NotImplementedException();
            UnityEngine.Debug.Log("InitalizeGraphics");
        }

        private DisplayMode _currentDisplayMode;

        public static GraphicsAdapter DefaultAdapter
        {
            get
            {
                return GraphicsAdapter.Adapters[0];
            }
        }

        private static ReadOnlyCollection<GraphicsAdapter> adapters;
        public static ReadOnlyCollection<GraphicsAdapter> Adapters
        {
            get
            {
                if (GraphicsAdapter.adapters == null)
                {
                    List<GraphicsAdapter> ga = new List<GraphicsAdapter>();
                    ga.Add(new GraphicsAdapter());
                    GraphicsAdapter.adapters = new ReadOnlyCollection<GraphicsAdapter>(ga);
                }
                return GraphicsAdapter.adapters;
            }
        }


        public DisplayMode CurrentDisplayMode
        {
            get
            {
                if (this._currentDisplayMode == null)
                {
                    UnityEngine.Debug.Log("getCurrentDisplayMode");
                    DisplayMode displayMode = new DisplayMode(UnityEngine.Screen.width, UnityEngine.Screen.height,SurfaceFormat.Rgb32 );
                    this._currentDisplayMode = displayMode;
                    if (displayMode._format < SurfaceFormat.Color)
                    {
                        displayMode._format = SurfaceFormat.Color;
                    }
                }
                return this._currentDisplayMode;
            }
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool IsProfileSupported(GraphicsProfile graphicsProfile)
        {
            UnityEngine.Debug.Log("?");
            return true;
        }

        private DisplayModeCollection _supportedDisplayModes;
        public DisplayModeCollection SupportedDisplayModes
        {
            get
            {
                if (_supportedDisplayModes==null)
                {
                   
                    List<DisplayMode> list = new List<DisplayMode>();
                    if(_currentDisplayMode ==null)
                    {
                        DisplayMode displayMode = new DisplayMode(UnityEngine.Screen.width, UnityEngine.Screen.height, SurfaceFormat.Rgb32);
                        list.Add(displayMode);
                    }
                    else 
                        list.Add(_currentDisplayMode);
                    return new DisplayModeCollection(list);
                }
                return this._supportedDisplayModes;
            }
        }
    }

    public enum GraphicsProfile
    {
        // Token: 0x0400036E RID: 878
        Reach,
        // Token: 0x0400036F RID: 879
        HiDef
    }

    public class DisplayModeCollection : IEnumerable<DisplayMode>, IEnumerable
    {
        // Token: 0x1700019C RID: 412
        public IEnumerable<DisplayMode> this[SurfaceFormat format]
        {
            get
            {
                List<DisplayMode> list = new List<DisplayMode>();
                foreach (DisplayMode displayMode in this._modes)
                {
                    if (displayMode.Format == format)
                    {
                        list.Add(displayMode);
                    }
                }
                return list;
            }
        }

        // Token: 0x060007F6 RID: 2038 RVA: 0x000096DB File Offset: 0x000078DB
        public IEnumerator<DisplayMode> GetEnumerator()
        {
            return this._modes.GetEnumerator();
        }

        // Token: 0x060007F7 RID: 2039 RVA: 0x000096DB File Offset: 0x000078DB
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._modes.GetEnumerator();
        }

        // Token: 0x060007F8 RID: 2040 RVA: 0x000096ED File Offset: 0x000078ED
        internal DisplayModeCollection(List<DisplayMode> modes)
        {
            modes.Sort(delegate (DisplayMode a, DisplayMode b)
            {
                if (a == b)
                {
                    return 0;
                }
                if (a.Format <= b.Format && a.Width <= b.Width && a.Height <= b.Height)
                {
                    return -1;
                }
                return 1;
            });
            this._modes = modes;
        }

        // Token: 0x04000323 RID: 803
        private readonly List<DisplayMode> _modes;
    }
}
    
