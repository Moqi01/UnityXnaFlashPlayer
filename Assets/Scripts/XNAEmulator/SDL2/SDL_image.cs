using System;
using System.Runtime.InteropServices;

namespace SDL2
{
    // Token: 0x020002CA RID: 714
    public static class SDL_image
    {
      

       
        // Token: 0x060015BA RID: 5562
       
        public static extern int IMG_Init(SDL_image.IMG_InitFlags flags);

        // Token: 0x060015BB RID: 5563
       
        public static extern void IMG_Quit();

        // Token: 0x060015BC RID: 5564
       
        public static extern IntPtr IMG_Load([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string file);

        // Token: 0x060015BD RID: 5565
       
        public static extern IntPtr IMG_Load_RW(IntPtr src, int freesrc);

        // Token: 0x060015BE RID: 5566
       
        public static extern IntPtr IMG_LoadTyped_RW(IntPtr src, int freesrc, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string type);

        // Token: 0x060015BF RID: 5567
       
        public static extern IntPtr IMG_LoadTexture(IntPtr renderer, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string file);

        // Token: 0x060015C0 RID: 5568
       
        public static extern IntPtr IMG_LoadTexture_RW(IntPtr renderer, IntPtr src, int freesrc);

        // Token: 0x060015C1 RID: 5569
       
        public static extern IntPtr IMG_LoadTextureTyped_RW(IntPtr renderer, IntPtr src, int freesrc, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string type);

        // Token: 0x060015C2 RID: 5570
       
        public static extern int IMG_InvertAlpha(int on);

        // Token: 0x060015C3 RID: 5571
       
        public static extern IntPtr IMG_ReadXPMFromArray([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] [In] string[] xpm);

        // Token: 0x060015C4 RID: 5572
       
        public static extern int IMG_SavePNG(IntPtr surface, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LPUtf8StrMarshaler))] [In] string file);

        // Token: 0x060015C5 RID: 5573
       
        public static extern int IMG_SavePNG_RW(IntPtr surface, IntPtr dst, int freedst);

        // Token: 0x04001000 RID: 4096
        private const string nativeLibName = "SDL2_image.dll";

        // Token: 0x04001001 RID: 4097
        public const int SDL_IMAGE_MAJOR_VERSION = 2;

        // Token: 0x04001002 RID: 4098
        public const int SDL_IMAGE_MINOR_VERSION = 0;

        // Token: 0x04001003 RID: 4099
        public const int SDL_IMAGE_PATCHLEVEL = 0;

        // Token: 0x020002CB RID: 715
        [Flags]
        public enum IMG_InitFlags
        {
            // Token: 0x04001005 RID: 4101
            IMG_INIT_JPG = 1,
            // Token: 0x04001006 RID: 4102
            IMG_INIT_PNG = 2,
            // Token: 0x04001007 RID: 4103
            IMG_INIT_TIF = 4,
            // Token: 0x04001008 RID: 4104
            IMG_INIT_WEBP = 8
        }
    }
}
