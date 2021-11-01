using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SDL2
{
    // Token: 0x020002CC RID: 716
    internal class LPUtf8StrMarshaler : ICustomMarshaler
    {
        // Token: 0x060015C6 RID: 5574 RVA: 0x0004542C File Offset: 0x0004362C
        public static ICustomMarshaler GetInstance(string cookie)
        {
            if (cookie != null && cookie == "LeaveAllocated")
            {
                return LPUtf8StrMarshaler._leaveAllocatedInstance;
            }
            return LPUtf8StrMarshaler._defaultInstance;
        }

        // Token: 0x060015C7 RID: 5575 RVA: 0x00045456 File Offset: 0x00043656
        public LPUtf8StrMarshaler(bool leaveAllocated)
        {
            this._leaveAllocated = leaveAllocated;
        }

        // Token: 0x060015C8 RID: 5576 RVA: 0x00045468 File Offset: 0x00043668
        public  object MarshalNativeToManaged(IntPtr pNativeData)
        {
           // if (pNativeData == IntPtr.Zero)
          //  {
                return null;
            //}
            //byte* ptr = (byte*)((void*)pNativeData);
            //while (*ptr != 0)
            //{
            //    ptr++;
            //}
            //byte[] array = new byte[(long)((byte*)ptr - (byte*)((void*)pNativeData))];
            //Marshal.Copy(pNativeData, array, 0, array.Length);
            //return Encoding.UTF8.GetString(array);
        }

        // Token: 0x060015C9 RID: 5577 RVA: 0x000454C0 File Offset: 0x000436C0
        public  IntPtr MarshalManagedToNative(object ManagedObj)
        {
            //if (ManagedObj == null)
            //{
            //    return IntPtr.Zero;
            //}
            //string text = ManagedObj as string;
            //if (text == null)
            //{
            //    throw new ArgumentException("ManagedObj must be a string.", "ManagedObj");
            //}
            //byte[] bytes = Encoding.UTF8.GetBytes(text);
            //IntPtr intPtr = SDL.SDL_malloc((IntPtr)(bytes.Length + 1));
            //Marshal.Copy(bytes, 0, intPtr, bytes.Length);
            //((byte*)((void*)intPtr))[bytes.Length] = 0;
            //return intPtr;
            return IntPtr.Zero;
        }

        // Token: 0x060015CA RID: 5578 RVA: 0x00045524 File Offset: 0x00043724
        public void CleanUpManagedData(object ManagedObj)
        {
        }

        // Token: 0x060015CB RID: 5579 RVA: 0x00045526 File Offset: 0x00043726
        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (!this._leaveAllocated)
            {
                SDL.SDL_free(pNativeData);
            }
        }

        // Token: 0x060015CC RID: 5580 RVA: 0x00045536 File Offset: 0x00043736
        public int GetNativeDataSize()
        {
            return -1;
        }

        // Token: 0x060015CD RID: 5581 RVA: 0x00045539 File Offset: 0x00043739
        // Note: this type is marked as 'beforefieldinit'.
        static LPUtf8StrMarshaler()
        {
        }

        // Token: 0x04001009 RID: 4105
        public const string LeaveAllocated = "LeaveAllocated";

        // Token: 0x0400100A RID: 4106
        private static ICustomMarshaler _leaveAllocatedInstance = new LPUtf8StrMarshaler(true);

        // Token: 0x0400100B RID: 4107
        private static ICustomMarshaler _defaultInstance = new LPUtf8StrMarshaler(false);

        // Token: 0x0400100C RID: 4108
        private bool _leaveAllocated;
    }
}
