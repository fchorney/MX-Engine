using System;
using MX;
using MX.Util;

namespace Test
{

    public class Main : Module
    {
        //Properties
        Fader fader;

        //Constuctor
        public Main(string n) : base(n) { }

        //Initialiser
        public override void Init()
        {
            _running = true;

            fader = new Fader("Fader", true);
            fader.Color = System.Drawing.Color.White;
            //fader.MaxAlpha = 100;

            //Modules.Add(new EBDemo("EBDemo",true));

            Modules.Add(fader);

            Modules.Add(new ShowFPS("ShowFPS", true));

            Modules.Add(new ParticleDemo("ParticleDemo", true));

            Modules.Add(new AnimationDemo("AnimationDemo", true));
            
        }

        //Frame logic
        public override void Update(double ElapsedTime)
        {
            //fader.Color = mxColor.RandomColor();
        }//Run()

        public override void ProcessInput(InputHandler Input)
        {

            if (Input.Keyboard.KeyPressed(Keys.Escape))
                this.Stop();

            if (Input.Keyboard.KeyPressed(Keys.Q))
                ((Fader)GetModule("Fader")).FadeOutAndIn(2);

            if (Input.Keyboard.KeyPressed(Keys.E))
                ((Fader)GetModule("Fader")).FadeIn(1);

            if (Input.Keyboard.KeyPressed(Keys.W))
                ((Fader)GetModule("Fader")).FadeOut(1);

            if (Input.Keyboard.KeyPressed(Keys.A))
                ((Fader)GetModule("Fader")).Pulse(2);

            if (Input.Keyboard.KeyPressed(Keys.D1))
                Modules[1].Running = !Modules[1].Running;

            if (Input.Keyboard.KeyPressed(Keys.D2))
                Modules[2].Running = !Modules[2].Running;

            if (Input.Keyboard.KeyPressed(Keys.D3))
                GetModule("ShowFPS").Running = !GetModule("ShowFPS").Running;
        
        }

        //Render to the screen
        public override void Render(Graphics g)
        {
            //bg.Render(g);
        }//Render()

    }//class ModuleHandler

}