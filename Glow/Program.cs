using System;
using System.Threading;
using System.Windows.Forms;
//
using static Glow.TSModules;

namespace Glow{
    public class Program{
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        // ======================================================================================================
        // GLOBAL TS CANCEL TOKEN
        public static CancellationTokenSource TS_TokenEngine;
        // ======================================================================================================
        // TS PRELOADER DEBUG MODE
        public static bool ts_pre_debug_mode = false;
        // ======================================================================================================
        // DEBUG MODE
        public static bool debug_mode = false;
        // ======================================================================================================
        // VERSION MODE
        public static int ts_version_mode = 0;
        // ======================================================================================================
        // DPI AWARE MODERN API
        static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = new IntPtr(-4);
        // ======================================================================================================
        [STAThread]
        static void Main(){
            SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2); // PerMonitorV2
            //
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TSPreloader());
        }
    }
}