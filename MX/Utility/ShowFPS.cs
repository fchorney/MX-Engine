using System;
using System.Collections.Generic;
using System.Text;
using MX;

namespace MX.Util
{

    public class ShowFPS : Module
    {
        //Properties
        private double sTime;
        private double cTime;
        private int FPSTicks;
        private int FPS;
        private Timer T;

        //Constuctor
        public ShowFPS(string n) : base(n) { }
        public ShowFPS(string n, bool running) : base(n, running) { }

        //Initialiser
        public override void Init()
        {
            T = new Timer();
        }

        //Frame logic
        public override void Update(double ElapsedTime)
        {
            FPSTicks++;
            cTime = T.GetAbsoluteTime() - sTime;
            if (cTime > 1)
            {
                FPS = FPSTicks;
                FPSTicks = 0;
                sTime = T.GetAbsoluteTime();
            }
        }//Update()

        public override void ProcessInput(InputHandler Input)
        {
        }

        //Render to the screen
        public override void Render(Graphics g)
        {
            g.DrawText(FPS.ToString() + " fps", (int)GameEnvironment.Camera.X, (int)GameEnvironment.Camera.Y);
            //g.DrawText("[" + (int)GameEnvironment.Camera.X + ", " + (int)GameEnvironment.Camera.Y + "]", (int)GameEnvironment.Camera.X + 80, (int)GameEnvironment.Camera.Y); 
        }//Render()

    }//class Fader

}
