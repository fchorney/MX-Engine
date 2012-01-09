using System;
using System.Windows.Forms;

namespace MX
{

    public class ViewForm : Form
    {

        public ViewForm()
        {
            this.Text = "";

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            //this.Cursor.Dispose();

            this.GotFocus += new EventHandler(ReacquireResources);
            this.Closing += new System.ComponentModel.CancelEventHandler(HandleClosing);

            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.Opaque |
                          ControlStyles.AllPaintingInWmPaint, true);

        }//frmMain()

        public void ReacquireResources(object sender, EventArgs e)
        {
            InputState.Init(this);
        }

        public void HandleClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dispose();
            Engine.GetInstance.Dispose();
        }//HandleClosing(object, CancelEventArgs)

        protected override void OnPaint(PaintEventArgs e) { }//Invalidate(); }

    }//class frmMain

}