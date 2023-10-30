using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

public class UnityTool{

    [MenuItem("UnityTools/ChangeVpk")]
    // Use this for initialization
    static void ChangeVpk()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        processStartInfo.FileName = "C:/UnityTools.exe";
        processStartInfo.Arguments = @"-i C:\Game\Flash\SWFX00000 -o E:\SWF -f -r -p";
        Process.Start(processStartInfo);
    }
}
