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
        // DEBUG MODES
        public static bool ts_pre_debug_mode = false;
        public static bool debug_mode = false;
        // ======================================================================================================
        [STAThread]
        static void Main(){
            SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
            //
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TSPreloader());
        }
    }
}