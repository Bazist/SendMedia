using SendMedia.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMedia
{
    public class SendMediaApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public SendMediaApplicationContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.TrayIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                                                new MenuItem("Exit", Exit)
                                             }),
                Text = "Giphy In Skype",
                Visible = true
            };

            var frm = new MainForm();
            frm.Show();

            MessageBox.Show("The application is running. Open Skype window and type /gif YOURKEYWORD. In short time the window with searched gifs will be opened.",
                           "Information",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);

        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}
