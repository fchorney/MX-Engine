using System;
using System.Collections.Generic;
using System.Text;
using MX;
using MX.Util;


namespace Test
{

    public class Template : Module
    {
        //Properties

        //Constuctor
        public Template(string n) : base(n) { }
        public Template(string n, bool running) : base(n, running) { }

        //Initialiser
        public override void Init()
        {
        }

        //Frame logic
        public override void Update(double ElapsedTime)
        {
        }//Update()

        public override void ProcessInput(InputHandler Input)
        {
        }

        //Render to the screen
        public override void Render(Graphics g)
        {
        }//Render()

    }//class Template

}
