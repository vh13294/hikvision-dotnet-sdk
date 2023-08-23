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
        delegate void SetPictureBoxCallback(Form form, System.Windows.Forms.PictureBox pictureBox, bool visibility);
        /// <summary>
        /// Set text property of various controls
        /// </summary>
        /// <param name="form">The calling form</param>
        /// <param name="ctrl"></param>
        /// <param name="text"></param>
        public static void SetPictureBoxVisibility(Form form, System.Windows.Forms.PictureBox pictureBox, bool visibility)
        {
            // InvokeRequired required compares the thread ID of the 
            // calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (pictureBox.InvokeRequired)
            {
                SetPictureBoxCallback d = new SetPictureBoxCallback(SetPictureBoxVisibility);
                form.Invoke(d, new object[] { form, pictureBox, visibility });
            }
            else
            {
                pictureBox.Visible = visibility;
            }
        }
    }
}
