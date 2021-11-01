using System.Diagnostics;

namespace Microsoft.Xna.Framework.Input
{
    public static class GamePad
    {
        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            // TODO: Improve
            return new GamePadState();            
        }

        public static bool SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
        {
            UnityEngine.Debug.Log("?");
            XINPUT_VIBRATION xinput_VIBRATION;
            xinput_VIBRATION.LeftMotorSpeed = (short)(leftMotor * 65535f);
            xinput_VIBRATION.RightMotorSpeed = (short)(rightMotor * 65535f);
            ErrorCodes errorCodes=ErrorCodes.Empty;
            if (GamePad.ThrottleDisconnectedRetries(playerIndex))
            {
                errorCodes = ErrorCodes.NotConnected;
            }
            else
            {
                //errorCodes = UnsafeNativeMethods.SetState(playerIndex, ref xinput_VIBRATION);
                GamePad.ResetThrottleState(playerIndex, errorCodes);
            }
            if (errorCodes == ErrorCodes.Success)
            {
                return true;
            }
            if (errorCodes != ErrorCodes.Success && errorCodes != ErrorCodes.NotConnected && errorCodes != ErrorCodes.Busy)
            {
               // throw new InvalidOperationException(FrameworkResources.InvalidController);
            }
            return false;
        }

        private static bool ThrottleDisconnectedRetries(PlayerIndex playerIndex)
        {
            if (playerIndex < PlayerIndex.One || playerIndex > PlayerIndex.Four)
            {
                return false;
            }
            if (!GamePad._disconnected[(int)playerIndex])
            {
                return false;
            }
            long timestamp = Stopwatch.GetTimestamp();
            for (int i = 0; i < 4; i++)
            {
                if (GamePad._disconnected[i])
                {
                    long num = timestamp - GamePad._lastReadTime[i];
                    long num2 = Stopwatch.Frequency;
                    if (i != (int)playerIndex)
                    {
                        num2 /= 4L;
                    }
                    if (num >= 0L && num <= num2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void ResetThrottleState(PlayerIndex playerIndex, ErrorCodes result)
        {
            if (playerIndex < PlayerIndex.One || playerIndex > PlayerIndex.Four)
            {
                return;
            }
            if (result == ErrorCodes.NotConnected)
            {
                GamePad._disconnected[(int)playerIndex] = true;
                GamePad._lastReadTime[(int)playerIndex] = Stopwatch.GetTimestamp();
                return;
            }
            GamePad._disconnected[(int)playerIndex] = false;
        }

        private static bool[] _disconnected = new bool[4];

        // Token: 0x040003D0 RID: 976
        private static long[] _lastReadTime = new long[4];
    }

    public struct XINPUT_VIBRATION
    {
        // Token: 0x040004BB RID: 1211
        public short LeftMotorSpeed;

        // Token: 0x040004BC RID: 1212
        public short RightMotorSpeed;
    }
}