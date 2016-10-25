using FightTheProxy.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace FightTheProxy
{
    static class Program
    {

        private readonly static int SLEEP_TIMER = 2000;

        private readonly static string REGEDIT_PROXY_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\";
        private readonly static string REGEDIT_PROXY_VALUE = "AutoConfigURL";

        private static int proxyDeletedCount = 0;

        private static Tray tray;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (tray = new Tray())
            {
                tray.Display();
                RunRemoveRegeditProxyBackgroundWorker();
                Application.Run();
            }
        }

        /// <summary>
        /// Based on http://stackoverflow.com/questions/363377/how-do-i-run-a-simple-bit-of-code-in-a-new-thread
        /// </summary>
        private static void RunRemoveRegeditProxyBackgroundWorker()
        {
            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;
                while (true)
                {
                    if (RemoveRegeditProxyKey())
                    {
                        proxyDeletedCount++;
                        tray.ChangeProxyRemovalNumber(proxyDeletedCount);
                    }
                    Thread.Sleep(SLEEP_TIMER);
                }
            });

            bw.RunWorkerAsync();
        }

        private static bool RemoveRegeditProxyKey()
        {
            using (RegistryKey Key = Registry.CurrentUser.OpenSubKey(REGEDIT_PROXY_KEY, true))
            {
                if (Key != null)
                {
                    string val = (string)Key.GetValue(REGEDIT_PROXY_VALUE);
                    if (!string.IsNullOrEmpty(val))
                    {
                        Key.DeleteValue(REGEDIT_PROXY_VALUE);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}