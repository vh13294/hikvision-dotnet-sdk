using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreviewDemo.Helpers
{
    public static class ThreadHelperClass
    {
        delegate void SetPictureBoxCallback(System.Windows.Forms.PictureBox pictureBox, bool visibility);
  
        public static void SetPictureBoxVisibility(System.Windows.Forms.PictureBox pictureBox, bool visibility)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (pictureBox.InvokeRequired)
            {
                SetPictureBoxCallback d = new SetPictureBoxCallback(SetPictureBoxVisibility);
                pictureBox.Invoke(d, new object[] { pictureBox, visibility });
            }
            else
            {
                pictureBox.Visible = visibility;
            }
        }
    }
}
