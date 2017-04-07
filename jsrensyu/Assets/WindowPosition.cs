using UnityEngine;
using System.Collections;

using System;
//using System.Runtime.InteropServices;

namespace Utils
{

    // ここからコードを拝借させてもらいました！
    // http://answers.unity3d.com/questions/13523/is-there-a-way-to-set-the-position-of-a-standalone.html
    // http://stackoverflow.com/questions/2825528/removing-the-title-bar-of-external-application-using-c-sharp

    /*
     * 注意点！
     * 実行するアプリケーションはfullscreenではなく、windowedの状態にしておく必要がある。
     */
    public class WindowControl : MonoBehaviour
    {

        [SerializeField]
        string windowName = "ChinaTrust2nd";
        [SerializeField]
        int x = 0;
        [SerializeField]
        int y = 0;
        [SerializeField]
        int width = 1920;
        [SerializeField]
        int height = 1080;
        [SerializeField]
        bool hideTitleBar = true;

        //[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        //private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        //[DllImport("user32.dll", EntryPoint = "FindWindow")]
        //public static extern IntPtr FindWindow(System.String className, System.String windowName);

        //// Sets window attributes
        //[DllImport("user32.dll")]
        //public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //// Gets window attributes
        //[DllImport("user32.dll")]
        //public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        //// assorted constants needed
        //private static int GWL_STYLE = -16;
        //private static int WS_CHILD = 0x40000000; //child window
        //private static int WS_BORDER = 0x00800000; //window with border
        //private static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        //private static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar

        void Start()
        {
            x = 10;
        }

        //void Awake()
        //{
        //    var window = FindWindow(null, windowName);
        //    if (hideTitleBar)
        //    {
        //        int style = GetWindowLong(window, GWL_STYLE);
        //        SetWindowLong(window, GWL_STYLE, (style & ~WS_CAPTION));
        //    }
        //    SetWindowPos(window, 0, x, y, width, height, width * height == 0 ? 1 : 0);
        //}

    }

}