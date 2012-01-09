using System;
using MX;
using MX.Util;
namespace BlockGame
{
    public class Main : Module
    {

        public Main(string n) : base(n) { }

        public override void Init()
        {
            _running = true;
            Modules.Add(new ShowFPS("ShowFPS", true));


            
        }

        public override void Update(double ElapsedTime)
        {
           
        }

        public override void ProcessInput(InputHandler Input)
        {
        
        }

        public override void Render(Graphics g)
        {

        }
    }

}