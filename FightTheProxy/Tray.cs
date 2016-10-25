using FightTheProxy.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FightTheProxy
{
    public class Tray : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public void Display()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.fist,
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit)
            }),
                Visible = true,
                Text = "Fight the Proxy"
            };
        }

        public void ChangeProxyRemovalNumber(int newNumber)
        {
            trayIcon.Text = string.Format("Fight the Proxy ({0} times removed)", newNumber);
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}