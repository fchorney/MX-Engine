using System;
using System.Windows.Forms;
using MX;

namespace BlockGame
{

    class MainForm
    {

        static Engine engine;

        [STAThread]
        static void Main()
        {
            engine = Engine.GetInstance;
            engine.Start(new Main("Main"), 640, 480);
        }//Main()


    }//class frmMain

}