using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Microsoft.Xna.Framework.Input
{
    public class KeyboardState
    {
        private bool[] keyStates;
        private KeyCode[] keyMapping;
        public KeyboardState(bool[] keyStates)
        {
            // TODO: Complete member initialization
            this.keyStates = keyStates;
        }

        public KeyboardState()
        {
            keyStates = new bool[256];
            //keyMapping = Keyboard.GetKeyMapping();
            //for (int i = 0; i < keyStates.Length; i++)
            //{
            //    if (keyMapping[i] != KeyCode.None)
            //    {
            //        keyStates[i] = UnityEngine.Input.GetKey(keyMapping[i]);
            //    }
            //}
        }

        public bool IsKeyDown(Keys keys)
        {
             return keyStates[(int)keys];
            //return UnityEngine.Input.GetKeyDown(keyMapping[(int)keys]);
        }


        public bool IsKeyUp(Keys keys)
        {
            return keyStates[(int)keys];
            //return UnityEngine.Input.GetKeyUp(keyMapping[(int)keys]);
        }

        public bool IsKey(Keys keys)
        {
            return keyStates[(int)keys];
            //return UnityEngine.Input.GetKey(keyMapping[(int)keys]);
        }

        public KeyState this[Keys key]
        {
            get
            {
                //if (!this.InternalGetKey(key))

                if (IsKeyDown(key))
                {
                    return KeyState.Down;
                }
                return KeyState.Up;

                //if (IsKeyUp(key))
                //{
                //    return KeyState.Up;
                //}
                //if (IsKeyDown(key))
                //{
                //    return KeyState.Down;
                //}
                //return KeyState.Up;
            }
        }

        //private bool InternalGetKey(Keys key)
        //{
        //    uint num = 1u << (int)key;
        //    uint num2;
        //    switch ((Keys)((int)key >> 5))
        //    {
        //        case Keys.None:
        //            num2 = this.keys0;
        //            break;
        //        case (Keys)1:
        //            num2 = this.keys1;
        //            break;
        //        case (Keys)2:
        //            num2 = this.keys2;
        //            break;
        //        case (Keys)3:
        //            num2 = this.keys3;
        //            break;
        //        case (Keys)4:
        //            num2 = this.keys4;
        //            break;
        //        case (Keys)5:
        //            num2 = this.keys5;
        //            break;
        //        case (Keys)6:
        //            num2 = this.keys6;
        //            break;
        //        case (Keys)7:
        //            num2 = this.keys7;
        //            break;
        //        default:
        //            num2 = 0u;
        //            break;
        //    }
        //    return (num2 & num) != 0u;
        //}
    }

    public struct MouseState
    {
        // Token: 0x17000355 RID: 853
        // (get) Token: 0x060010EA RID: 4330 RVA: 0x0003703C File Offset: 0x0003523C
        // (set) Token: 0x060010EB RID: 4331 RVA: 0x00037044 File Offset: 0x00035244
        public int X { get; internal set; }

        // Token: 0x17000356 RID: 854
        // (get) Token: 0x060010EC RID: 4332 RVA: 0x0003704D File Offset: 0x0003524D
        // (set) Token: 0x060010ED RID: 4333 RVA: 0x00037055 File Offset: 0x00035255
        public int Y { get; internal set; }

        // Token: 0x17000357 RID: 855
        // (get) Token: 0x060010EE RID: 4334 RVA: 0x0003705E File Offset: 0x0003525E
        // (set) Token: 0x060010EF RID: 4335 RVA: 0x00037066 File Offset: 0x00035266
        public ButtonState LeftButton { get; internal set; }

        // Token: 0x17000358 RID: 856
        // (get) Token: 0x060010F0 RID: 4336 RVA: 0x0003706F File Offset: 0x0003526F
        // (set) Token: 0x060010F1 RID: 4337 RVA: 0x00037077 File Offset: 0x00035277
        public ButtonState MiddleButton { get; internal set; }

        // Token: 0x17000359 RID: 857
        // (get) Token: 0x060010F2 RID: 4338 RVA: 0x00037080 File Offset: 0x00035280
        // (set) Token: 0x060010F3 RID: 4339 RVA: 0x00037088 File Offset: 0x00035288
        public ButtonState RightButton { get; internal set; }

        // Token: 0x1700035A RID: 858
        // (get) Token: 0x060010F4 RID: 4340 RVA: 0x00037091 File Offset: 0x00035291
        // (set) Token: 0x060010F5 RID: 4341 RVA: 0x00037099 File Offset: 0x00035299
        public int ScrollWheelValue { get; internal set; }

        // Token: 0x1700035B RID: 859
        // (get) Token: 0x060010F6 RID: 4342 RVA: 0x000370A2 File Offset: 0x000352A2
        // (set) Token: 0x060010F7 RID: 4343 RVA: 0x000370AA File Offset: 0x000352AA
        public ButtonState XButton1 { get; internal set; }

        // Token: 0x1700035C RID: 860
        // (get) Token: 0x060010F8 RID: 4344 RVA: 0x000370B3 File Offset: 0x000352B3
        // (set) Token: 0x060010F9 RID: 4345 RVA: 0x000370BB File Offset: 0x000352BB
        public ButtonState XButton2 { get; internal set; }

        // Token: 0x060010FA RID: 4346 RVA: 0x000370C4 File Offset: 0x000352C4
        public MouseState(int x, int y, int scrollWheel, ButtonState leftButton, ButtonState middleButton, ButtonState rightButton, ButtonState xButton1, ButtonState xButton2)
        {
            this = default(MouseState);
            this.X = x;
            this.Y = y;
            this.ScrollWheelValue = scrollWheel;
            this.LeftButton = leftButton;
            this.MiddleButton = middleButton;
            this.RightButton = rightButton;
            this.XButton1 = xButton1;
            this.XButton2 = xButton2;
        }

        // Token: 0x060010FB RID: 4347 RVA: 0x00037118 File Offset: 0x00035318
        public static bool operator ==(MouseState left, MouseState right)
        {
            return left.X == right.X && left.Y == right.Y && left.LeftButton == right.LeftButton && left.MiddleButton == right.MiddleButton && left.RightButton == right.RightButton && left.ScrollWheelValue == right.ScrollWheelValue;
        }

        // Token: 0x060010FC RID: 4348 RVA: 0x00037187 File Offset: 0x00035387
        public static bool operator !=(MouseState left, MouseState right)
        {
            return !(left == right);
        }

        // Token: 0x060010FD RID: 4349 RVA: 0x00037193 File Offset: 0x00035393
        public override bool Equals(object obj)
        {
            return obj is MouseState && this == (MouseState)obj;
        }

        // Token: 0x060010FE RID: 4350 RVA: 0x000371B0 File Offset: 0x000353B0
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class Mouse
    {
        // Token: 0x17000353 RID: 851
        // (get) Token: 0x060010E3 RID: 4323 RVA: 0x00036F31 File Offset: 0x00035131
        // (set) Token: 0x060010E4 RID: 4324 RVA: 0x00036F38 File Offset: 0x00035138
        public static IntPtr WindowHandle { get; set; }

        // Token: 0x17000354 RID: 852
        // (get) Token: 0x060010E5 RID: 4325 RVA: 0x00036F40 File Offset: 0x00035140
        // (set) Token: 0x060010E6 RID: 4326 RVA: 0x00036F4C File Offset: 0x0003514C
        public static bool IsRelativeMouseModeEXT
        {
            get
            {
                //return FNAPlatform.GetRelativeMouseMode();
                return true;
            }
            set
            {
                //FNAPlatform.SetRelativeMouseMode(value);

            }
        }

        // Token: 0x060010E7 RID: 4327 RVA: 0x00036F5C File Offset: 0x0003515C
        public static MouseState GetState()
        {
            int num=0;
            int num2=0;
            ButtonState leftButton=ButtonState.Pressed;
            ButtonState middleButton=ButtonState.Pressed;
            ButtonState rightButton = ButtonState.Pressed;
            ButtonState xButton = ButtonState.Pressed;
            ButtonState xButton2 = ButtonState.Pressed;
            //FNAPlatform.GetMouseState(Mouse.WindowHandle, out num, out num2, out leftButton, out middleButton, out rightButton, out xButton, out xButton2);
            num = (int)((double)num * (double)Mouse.INTERNAL_BackBufferWidth / (double)Mouse.INTERNAL_WindowWidth);
            num2 = (int)((double)num2 * (double)Mouse.INTERNAL_BackBufferHeight / (double)Mouse.INTERNAL_WindowHeight);
            return new MouseState(num, num2, Mouse.INTERNAL_MouseWheel, leftButton, middleButton, rightButton, xButton, xButton2);
        }

        // Token: 0x060010E8 RID: 4328 RVA: 0x00036FC0 File Offset: 0x000351C0
        public static void SetPosition(int x, int y)
        {
            if (Mouse.IsRelativeMouseModeEXT)
            {
                return;
            }
            x = (int)((double)x * (double)Mouse.INTERNAL_WindowWidth / (double)Mouse.INTERNAL_BackBufferWidth);
            y = (int)((double)y * (double)Mouse.INTERNAL_WindowHeight / (double)Mouse.INTERNAL_BackBufferHeight);
           // FNAPlatform.SetMousePosition(Mouse.WindowHandle, x, y);
        }

        // Token: 0x040009C9 RID: 2505
        internal static int INTERNAL_WindowWidth = GraphicsDeviceManager.DefaultBackBufferWidth;

        // Token: 0x040009CA RID: 2506
        internal static int INTERNAL_WindowHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

        // Token: 0x040009CB RID: 2507
        internal static int INTERNAL_BackBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;

        // Token: 0x040009CC RID: 2508
        internal static int INTERNAL_BackBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

        // Token: 0x040009CD RID: 2509
        internal static int INTERNAL_MouseWheel = 0;
    }
}
