using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SDL2
{
    // Token: 0x02000259 RID: 601
    public static class SDL
    {
        // Token: 0x060013D3 RID: 5075 RVA: 0x00044C7E File Offset: 0x00042E7E
        public static uint SDL_FOURCC(byte A, byte B, byte C, byte D)
        {
            return (uint)((int)A | (int)B << 8 | (int)C << 16 | (int)D << 24);
        }

        // Token: 0x060013D4 RID: 5076
        
        internal static extern IntPtr SDL_malloc(IntPtr size);

        // Token: 0x060013D5 RID: 5077
        
        internal static extern void SDL_free(IntPtr memblock);

        // Token: 0x060013D6 RID: 5078
        
        internal static extern IntPtr INTERNAL_SDL_RWFromFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string file, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string mode);

        // Token: 0x060013D7 RID: 5079
        
        public static extern IntPtr SDL_RWFromMem(byte[] mem, int size);

        // Token: 0x060013D8 RID: 5080
        
        public static extern void SDL_SetMainReady();

        // Token: 0x060013D9 RID: 5081
        
        public static extern int SDL_Init(uint flags);

        // Token: 0x060013DA RID: 5082
        
        public static extern int SDL_InitSubSystem(uint flags);

        // Token: 0x060013DB RID: 5083
        
        public static extern void SDL_Quit();

        // Token: 0x060013DC RID: 5084
        
        public static extern void SDL_QuitSubSystem(uint flags);

        // Token: 0x060013DD RID: 5085
        
        public static extern uint SDL_WasInit(uint flags);

        // Token: 0x060013DE RID: 5086

        //[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler), MarshalCookie = "LeaveAllocated")]
        public static string SDL_GetPlatform() { return "Windows"; }

        // Token: 0x060013DF RID: 5087
        
        public static extern void SDL_ClearHints();

        // Token: 0x060013E0 RID: 5088
        
        
        public static extern string SDL_GetHint([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string name);

        // Token: 0x060013E1 RID: 5089
        
       
        // Token: 0x060013E4 RID: 5092
        
        public static extern void SDL_ClearError();

        // Token: 0x060013E5 RID: 5093
        
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler), MarshalCookie = "LeaveAllocated")]
        public static extern string SDL_GetError();

        // Token: 0x060013E6 RID: 5094
        
        public static extern void SDL_SetError([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013E7 RID: 5095
        
        public static extern void SDL_Log([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013E8 RID: 5096
        
        public static extern void SDL_LogVerbose(int category, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013E9 RID: 5097
        
        public static extern void SDL_LogDebug(int category, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013EA RID: 5098
        
        public static extern void SDL_LogInfo(int category, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013EB RID: 5099
        
        public static extern void SDL_LogWarn(int category, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013EC RID: 5100
        
        public static extern void SDL_LogError(int category, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013ED RID: 5101
        
        public static extern void SDL_LogCritical(int category, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string fmt, __arglist);

        // Token: 0x060013EE RID: 5102
       

        // Token: 0x060013FD RID: 5117
        
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler), MarshalCookie = "LeaveAllocated")]
        public static extern string SDL_GetRevision();

        // Token: 0x060013FE RID: 5118
        
        public static extern int SDL_GetRevisionNumber();

        // Token: 0x060013FF RID: 5119 RVA: 0x00044E73 File Offset: 0x00043073
        public static int SDL_WINDOWPOS_UNDEFINED_DISPLAY(int X)
        {
            return 536805376 | X;
        }

        // Token: 0x06001400 RID: 5120 RVA: 0x00044E7C File Offset: 0x0004307C
        public static bool SDL_WINDOWPOS_ISUNDEFINED(int X)
        {
            return true;
        }

        // Token: 0x06001401 RID: 5121 RVA: 0x00044E8F File Offset: 0x0004308F
        public static int SDL_WINDOWPOS_CENTERED_DISPLAY(int X)
        {
            return 805240832 | X;
        }

        // Token: 0x06001402 RID: 5122 RVA: 0x00044E98 File Offset: 0x00043098
        public static bool SDL_WINDOWPOS_ISCENTERED(int X)
        {
            return true;
        }

        // Token: 0x06001403 RID: 5123
        

        // Token: 0x06001404 RID: 5124
        

        // Token: 0x06001405 RID: 5125
        
        public static extern IntPtr SDL_CreateWindowFrom(IntPtr data);

        // Token: 0x06001406 RID: 5126
        
        public static extern void SDL_DestroyWindow(IntPtr window);

        // Token: 0x06001407 RID: 5127
        
        public static extern void SDL_DisableScreenSaver();

        // Token: 0x06001408 RID: 5128
        
        public static extern void SDL_EnableScreenSaver();

        // Token: 0x06001409 RID: 5129
        
      
        
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler), MarshalCookie = "LeaveAllocated")]
        public static extern string SDL_GetDisplayName(int index);

        // Token: 0x0600140E RID: 5134
        
        //public static extern int SDL_GetDisplayBounds(int displayIndex, out SDL.SDL_Rect rect);

        // Token: 0x0600140F RID: 5135
        
        public static extern int SDL_GetDisplayDPI(int displayIndex, out float ddpi, out float hdpi, out float vdpi);

        // Token: 0x06001410 RID: 5136
        

        // Token: 0x06001411 RID: 5137
        
        //public static extern int SDL_GetDisplayUsableBounds(int displayIndex, out SDL.SDL_Rect rect);

        // Token: 0x06001412 RID: 5138
        
        public static extern int SDL_GetNumDisplayModes(int displayIndex);

        // Token: 0x06001413 RID: 5139
        
        public static extern int SDL_GetNumVideoDisplays();

        // Token: 0x06001414 RID: 5140
        
        public static extern int SDL_GetNumVideoDrivers();

        // Token: 0x06001415 RID: 5141
        
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler), MarshalCookie = "LeaveAllocated")]
        public static extern string SDL_GetVideoDriver(int index);

        // Token: 0x06001416 RID: 5142
        
        public static extern float SDL_GetWindowBrightness(IntPtr window);

        // Token: 0x06001417 RID: 5143
        
        public static extern int SDL_SetWindowOpacity(IntPtr window, float opacity);

        // Token: 0x06001418 RID: 5144
        
        public static extern int SDL_GetWindowOpacity(IntPtr window, out float out_opacity);

        // Token: 0x06001419 RID: 5145
        
        public static extern int SDL_SetWindowModalFor(IntPtr modal_window, IntPtr parent_window);

        // Token: 0x0600141A RID: 5146
        
        public static extern int SDL_SetWindowInputFocus(IntPtr window);

        // Token: 0x0600141B RID: 5147
        
        public static extern IntPtr SDL_GetWindowData(IntPtr window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string name);

        // Token: 0x0600141C RID: 5148
        
        public static extern int SDL_GetWindowDisplayIndex(IntPtr window);

        // Token: 0x0600141D RID: 5149
        

        // Token: 0x0600141E RID: 5150
        
        public static extern uint SDL_GetWindowFlags(IntPtr window);

        // Token: 0x0600141F RID: 5151
        
        public static extern IntPtr SDL_GetWindowFromID(uint id);

        // Token: 0x06001420 RID: 5152
        
        public static extern int SDL_GetWindowGammaRamp(IntPtr window, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] [Out] ushort[] red, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] [Out] ushort[] green, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] [Out] ushort[] blue);

        // Token: 0x06001421 RID: 5153
        

        // Token: 0x06001422 RID: 5154
        
        public static extern uint SDL_GetWindowID(IntPtr window);

        // Token: 0x06001423 RID: 5155
        
        public static extern uint SDL_GetWindowPixelFormat(IntPtr window);

        // Token: 0x06001424 RID: 5156
        
        public static extern void SDL_GetWindowMaximumSize(IntPtr window, out int max_w, out int max_h);

        // Token: 0x06001425 RID: 5157
        
        public static extern void SDL_GetWindowMinimumSize(IntPtr window, out int min_w, out int min_h);

        // Token: 0x06001426 RID: 5158
        
        public static extern void SDL_GetWindowPosition(IntPtr window, out int x, out int y);

        // Token: 0x06001427 RID: 5159
        
        public static extern void SDL_GetWindowSize(IntPtr window, out int w, out int h);

        // Token: 0x06001428 RID: 5160
        
        public static extern IntPtr SDL_GetWindowSurface(IntPtr window);

        // Token: 0x06001429 RID: 5161
        
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler), MarshalCookie = "LeaveAllocated")]
        public static extern string SDL_GetWindowTitle(IntPtr window);

        // Token: 0x0600142A RID: 5162
        
        public static extern int SDL_GL_BindTexture(IntPtr texture, out float texw, out float texh);

        // Token: 0x0600142B RID: 5163
        
        public static extern IntPtr SDL_GL_CreateContext(IntPtr window);

        // Token: 0x0600142C RID: 5164
        
        public static extern void SDL_GL_DeleteContext(IntPtr context);

        // Token: 0x0600142D RID: 5165
        
        public static extern IntPtr SDL_GL_GetProcAddress([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string proc);

        // Token: 0x0600142E RID: 5166
        

        // Token: 0x0600142F RID: 5167
        
        public static extern void SDL_GL_ResetAttributes();

        // Token: 0x06001430 RID: 5168
        

        // Token: 0x06001431 RID: 5169
        
        public static extern int SDL_GL_GetSwapInterval();

        // Token: 0x06001432 RID: 5170
        
        public static extern int SDL_GL_MakeCurrent(IntPtr window, IntPtr context);

        // Token: 0x06001433 RID: 5171
        
        public static extern IntPtr SDL_GL_GetCurrentWindow();

        // Token: 0x06001434 RID: 5172
        
        public static extern IntPtr SDL_GL_GetCurrentContext();

        // Token: 0x06001435 RID: 5173
        
        public static extern void SDL_GL_GetDrawableSize(IntPtr window, out int w, out int h);

        // Token: 0x06001436 RID: 5174
        

        // Token: 0x06001437 RID: 5175
        
        public static extern int SDL_GL_SetSwapInterval(int interval);

        // Token: 0x06001438 RID: 5176
        
        public static extern void SDL_GL_SwapWindow(IntPtr window);

        // Token: 0x06001439 RID: 5177
        
        public static extern int SDL_GL_UnbindTexture(IntPtr texture);

        // Token: 0x0600143A RID: 5178
        
        public static extern void SDL_HideWindow(IntPtr window);

        // Token: 0x0600143B RID: 5179
        

        // Token: 0x0600143C RID: 5180
        
        public static extern void SDL_MaximizeWindow(IntPtr window);

        // Token: 0x0600143D RID: 5181
        
        public static extern void SDL_MinimizeWindow(IntPtr window);

        // Token: 0x0600143E RID: 5182
        
        public static extern void SDL_RaiseWindow(IntPtr window);

        // Token: 0x0600143F RID: 5183
        
        public static extern void SDL_RestoreWindow(IntPtr window);

        // Token: 0x06001440 RID: 5184
        
        public static extern int SDL_SetWindowBrightness(IntPtr window, float brightness);

        // Token: 0x06001441 RID: 5185
        
        public static extern IntPtr SDL_SetWindowData(IntPtr window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string name, IntPtr userdata);

        // Token: 0x06001442 RID: 5186
        

        // Token: 0x06001443 RID: 5187
        
        public static extern int SDL_SetWindowFullscreen(IntPtr window, uint flags);

        // Token: 0x06001444 RID: 5188
        
        public static extern int SDL_SetWindowGammaRamp(IntPtr window, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] [In] ushort[] red, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] [In] ushort[] green, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)] [In] ushort[] blue);

        // Token: 0x06001445 RID: 5189
        

        // Token: 0x06001446 RID: 5190
        
        public static extern void SDL_SetWindowIcon(IntPtr window, IntPtr icon);

        // Token: 0x06001447 RID: 5191
        
        public static extern void SDL_SetWindowMaximumSize(IntPtr window, int max_w, int max_h);

        // Token: 0x06001448 RID: 5192
        
        public static extern void SDL_SetWindowMinimumSize(IntPtr window, int min_w, int min_h);

        // Token: 0x06001449 RID: 5193
        
        public static extern void SDL_SetWindowPosition(IntPtr window, int x, int y);

        // Token: 0x0600144A RID: 5194
        
        public static extern void SDL_SetWindowSize(IntPtr window, int w, int h);

        // Token: 0x0600144B RID: 5195
        

        // Token: 0x0600144C RID: 5196
        
        public static extern int SDL_GetWindowBordersSize(IntPtr window, out int top, out int left, out int bottom, out int right);

        // Token: 0x0600144D RID: 5197
        

        // Token: 0x0600144E RID: 5198
        
        public static extern void SDL_SetWindowTitle(IntPtr window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string title);

        // Token: 0x0600144F RID: 5199
        
        public static extern void SDL_ShowWindow(IntPtr window);

        // Token: 0x06001450 RID: 5200
        
        public static extern int SDL_UpdateWindowSurface(IntPtr window);

        // Token: 0x06001451 RID: 5201
        
        //public static extern int SDL_UpdateWindowSurfaceRects(IntPtr window, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.class, SizeParamIndex = 2)] [In] SDL.SDL_Rect[] rects, int numrects);

        // Token: 0x06001452 RID: 5202
        
        public static extern int SDL_VideoInit([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string driver_name);

        // Token: 0x06001453 RID: 5203
        
        public static extern void SDL_VideoQuit();

        // Token: 0x06001454 RID: 5204
      

        // Token: 0x06001457 RID: 5207
        
        public static extern IntPtr SDL_CreateSoftwareRenderer(IntPtr surface);

        // Token: 0x06001458 RID: 5208
        
        public static extern IntPtr SDL_CreateTexture(IntPtr renderer, uint format, int access, int w, int h);

        // Token: 0x06001459 RID: 5209
        
        public static extern IntPtr SDL_CreateTextureFromSurface(IntPtr renderer, IntPtr surface);

        // Token: 0x0600145A RID: 5210
        
        public static extern void SDL_DestroyRenderer(IntPtr renderer);

        // Token: 0x0600145B RID: 5211
        
        public static extern void SDL_DestroyTexture(IntPtr texture);

        // Token: 0x0600145C RID: 5212
        
        public static extern int SDL_GetNumRenderDrivers();

        // Token: 0x0600145D RID: 5213
        
       
        
        public static extern int SDL_GetTextureColorMod(IntPtr texture, out byte r, out byte g, out byte b);

     

// Token: 0x020002C9 RID: 713
public enum SDL_PowerState
{
    // Token: 0x04000FFB RID: 4091
    SDL_POWERSTATE_UNKNOWN,
    // Token: 0x04000FFC RID: 4092
    SDL_POWERSTATE_ON_BATTERY,
    // Token: 0x04000FFD RID: 4093
    SDL_POWERSTATE_NO_BATTERY,
    // Token: 0x04000FFE RID: 4094
    SDL_POWERSTATE_CHARGING,
    // Token: 0x04000FFF RID: 4095
    SDL_POWERSTATE_CHARGED
}
	}
}
