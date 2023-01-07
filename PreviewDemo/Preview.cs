using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using PreviewDemo.Helpers;
using static PreviewDemo.Helpers.LoadJson;

namespace PreviewDemo
{
    /// <summary>
    /// </summary>
    public class Preview : System.Windows.Forms.Form
    {
        private LoadJson.Config config;
        private Dictionary<string, string> errorDictionary;
        private DVR[] dvrs;
        private System.Windows.Forms.PictureBox[] VideoFrames;

        private bool m_bInitSDK = false;

        /// <summary>
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Preview()
        {
            // Load Config
            config = LoadJson.GetDVRs();
            // Load Error
            errorDictionary = LoadJson.GetErrorMessageDict();
            //
            InitDVR();
            //
            InitializeComponent();
            //
            //
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                //To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            }
            //
            StartStream();
            //
        }

        /// <summary>
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            foreach (DVR dvr in dvrs)
            {
                dvr.Dispose();
            }

            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Windows 123
        /// <summary>
        /// </summary>
        private void InitializeComponent()
        {
            InitLayout(this.Controls);
            this.SuspendLayout();

            this.BackColor = Color.FromName("Black");

            //
            // GO FULL SCREEN
            //
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            //
            this.AutoScaleMode = AutoScaleMode.Dpi;
            //
            // Multiple screens / monitors
            this.Location = getScreen().WorkingArea.Location;

            this.Name = "Preview";
            this.Text = "Preview";

            this.Load += new System.EventHandler(this.Preview_Load);

            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }
                
            Application.Run(new Preview());
        }

        private Screen getScreen()
        {
            if (Screen.AllScreens.Length > config.displayScreen)
            {
                return Screen.AllScreens[config.displayScreen];
            }
            return Screen.PrimaryScreen;
        }

        private void InitLayout(Control.ControlCollection controls)
        {
            int screenNumber = config.numberOfScreen;
            VideoFrames = new System.Windows.Forms.PictureBox[screenNumber * screenNumber];

            int videoFrameWidth = getScreen().Bounds.Width / screenNumber;
            int videoFrameHeight = getScreen().Bounds.Height / screenNumber;

            int posX = 0;
            int posY = 0;
            int frameIndex = 0;

            foreach (DVR dvr in dvrs)
            {
                foreach (Int16 channel in dvr.channels)
                {
                    createVideoFrame(frameIndex, controls, posX, posY, videoFrameWidth, videoFrameHeight);
                    frameIndex++;
                    if (frameIndex % screenNumber == 0)
                    {
                        posY += videoFrameHeight;
                    }
                    posX += videoFrameWidth;
                    posX = posX % (getScreen().Bounds.Width - 1);
                }
            }
        }

        private void createVideoFrame(int frameIndex, Control.ControlCollection controls, int posX, int posY, int videoFrameWidth, int videoFrameHeight)
        {
            VideoFrames[frameIndex] = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(VideoFrames[frameIndex])).BeginInit();

            // mouse event
            VideoFrames[frameIndex].MouseDown += mouseClick;

            VideoFrames[frameIndex].Location = new System.Drawing.Point(posX, posY);
            VideoFrames[frameIndex].Size = new System.Drawing.Size(videoFrameWidth, videoFrameHeight);
            VideoFrames[frameIndex].BackColor = Color.FromName("Black");

            controls.Add(VideoFrames[frameIndex]);
            ((System.ComponentModel.ISupportInitialize)(VideoFrames[frameIndex])).EndInit();
        }

        private void InitDVR()
        {
            int deviceCount = config.Devices.Count;
            dvrs = new DVR[deviceCount];

            for (int i = 0; i < deviceCount; i++)
            {
                Device device = config.Devices[i];
                dvrs[i] = new DVR(device.ip, device.username, device.password, device.port, device.channels.ToArray(), errorDictionary);
            }
        }

        private void StartStream()
        {
            int frameIndex = 0;
            foreach (DVR dvr in dvrs)
            {
                dvr.loginAsync();
                for (int channelIndex = 0; channelIndex < dvr.channels.Length; channelIndex++)
                {
                    dvr.previewAsync(VideoFrames[frameIndex], channelIndex);
                    frameIndex++;
                }
            }
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string message = "";

                foreach (DVR dvr in dvrs)
                {
                    message += dvr.getDeviceInfo();
                }
                MessageBox.Show(message);
            }
        }

        private void Preview_Load(object sender, EventArgs e)
        {

        }

    }
}
