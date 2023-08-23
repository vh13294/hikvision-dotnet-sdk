using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Vip.Notification;

namespace PreviewDemo.Helpers
{
    public class DVR
    {
        CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        CHCNetSDK.REALDATACALLBACK RealData = null;
        

        private Int32 m_lUserID = -1;
        private Int32[] m_lRealHandles;

        private System.Windows.Forms.Form form;
        private Dictionary<Int32, System.Windows.Forms.PictureBox> realHandlesAndFrameList;

        private string ip;
        private Int16 port;
        private uint streamType;
        private string username;
        private string password;
        public Int16[] channels;
        public Dictionary<string, string> errorDictionary;

        public DVR(System.Windows.Forms.Form form, string ip, string username, string password, Int16 port, uint streamType, Int16[] channels, Dictionary<string, string> errorDictionary)
        {
            this.form = form;
            this.ip = ip;
            this.username = username;
            this.password = password;
            this.port = port;
            this.streamType = streamType;
            this.channels = channels;
            this.errorDictionary = errorDictionary;

            this.m_lRealHandles = new int[channels.Length];
            for (int i = 0; i < channels.Length; i++)
            {
                this.m_lRealHandles[i] = -1;
            }

            this.realHandlesAndFrameList = new Dictionary<Int32, System.Windows.Forms.PictureBox>();
            this.RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);
        }

        public async void loginAsync()
        {
            bool succeeded = false;
            while (!succeeded)
            {
                succeeded = login();
                await Task.Delay(30000);
            }
        }

        private bool login()
        {
            if (this.m_lUserID < 0)
            {
                DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

                //Login the device
                this.m_lUserID = CHCNetSDK.NET_DVR_Login_V30(this.ip, this.port, this.username, this.password, ref DeviceInfo);
                if (this.m_lUserID < 0)
                {
                    showError();
                    return false;
                }

            }
            return true;
        }

        public async void previewAsync(System.Windows.Forms.PictureBox RealPlayWnd, int index)
        {
            bool succeeded = false;
            while (!succeeded)
            {
                succeeded = preview(RealPlayWnd, index);
                await Task.Delay(30000);
            }
        }

        private bool preview(System.Windows.Forms.PictureBox RealPlayWnd, int index)
        {
            if (this.m_lUserID < 0)
            {
                return false;
            }

            if (this.m_lRealHandles[index] < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;
                // lChannel Channel number, the current analog channel number of the device starts from 1, 
                // and the start channel number of the digital channel is obtained through NET_DVR_GetDVRConfig 
                // (configuration command NET_DVR_GET_IPPARACFG_V40) (dwStartDChan).
                lpPreviewInfo.lChannel = channels[index];
                // Stream type: 0-main stream, 1-sub stream, 2-three stream, 3-virtual stream, and so on
                lpPreviewInfo.dwStreamType = this.streamType;
                // Connection mode: 0- TCP mode, 1- UDP mode, 2- multicast mode, 3- RTP mode,
                // 4- RTP/RTSP, 5- RTP/HTTP, 6- HRUDP (reliable transmission)
                lpPreviewInfo.dwLinkMode = 0;
                // 0 - non - blocking stream fetching, 1 - blocking stream fetching
                // If it is set to non - blocking, it means that the connection with the device is considered successful when the connection is initiated.If there is a code stream receiving failure,
                // playback failure, etc., the upper layer will be notified by preview abnormality. The pause time can be shortened during loop playback, which is consistent with NET_DVR_RealPlay.
                // If it is set to block, it means that the success or failure will not be returned until the playback operation is completed.When the internal connection of the SDK fails when the network is abnormal,
                // there will be a 5s timeout before the return can be returned, which is not suitable for polling and streaming operations.
                lpPreviewInfo.bBlocked = true;
                lpPreviewInfo.dwDisplayBufNum = 15;
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;

                IntPtr pUser = new IntPtr();

                // Start live view 
                this.m_lRealHandles[index] = CHCNetSDK.NET_DVR_RealPlay_V40(this.m_lUserID, ref lpPreviewInfo, RealData, pUser);
                this.realHandlesAndFrameList.Add(this.m_lRealHandles[index], RealPlayWnd);
                if (this.m_lRealHandles[index] < 0)
                {
                    showError();
                    return false;
                }
            }
            return true;
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            System.Windows.Forms.PictureBox frame = this.realHandlesAndFrameList[lRealHandle];

            if (dwBufSize > 0)
            {
                // connection active
                if(!frame.Visible)
                {
                    ThreadHelperClass.SetPictureBoxVisibility(this.form, frame, true);
                }
            }
            else
            {
                // connection lost
                ThreadHelperClass.SetPictureBoxVisibility(this.form, frame, false);
            }
        }

        private void showError()
        {
            string error = this.errorDictionary[CHCNetSDK.NET_DVR_GetLastError().ToString()];
            Alert.ShowError(error, 15000);
            Console.WriteLine(error);
        }

        public string getDeviceInfo()
        {
            return $"Device Info: {this.ip} : {this.port} {Environment.NewLine}" + 
                $"DeviceInfo.byIPChanNum / total IP Cam : {DeviceInfo.byIPChanNum} {Environment.NewLine}" +
                $"DeviceInfo.byStartDChan / start of IP cam : {DeviceInfo.byStartDChan} {Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";
        }

        public void Dispose()
        {
            foreach (Int32 m_lRealHandle in m_lRealHandles)
            {
                if (m_lRealHandle >= 0)
                {
                    CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                }
            }

            if (this.m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(this.m_lUserID);
            }
        }
    }
}
