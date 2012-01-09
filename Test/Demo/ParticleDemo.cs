using System;
using System.Collections.Generic;
using System.Text;
using MX;


    public class ParticleDemo : Module
    {
        ParticleEffect particleEffect_A;
        ParticleEffect particleEffect_B;

        public ParticleDemo(string n) : base(n) { }
        public ParticleDemo(string n, bool running) : base(n, running) { }

        public override void Init()
        {
            particleEffect_A = new ParticleEffect_A();
            particleEffect_B = new ParticleEffect_B();
        }

        public override void Update(double ElapsedTime)
        {
            ((ParticleEffect_A)particleEffect_A).x = GameEnvironment.Mouse.X;
            ((ParticleEffect_A)particleEffect_A).y = GameEnvironment.Mouse.Y;
            particleEffect_A.Update(ElapsedTime);
            particleEffect_B.Update(ElapsedTime);
        }

        public override void ProcessInput(InputHandler Input)
        {
            if (Input.Mouse.BtnHeld(Buttons.Left))
                particleEffect_A.Start();
            else
                particleEffect_A.Stop();

            if (Input.Mouse.BtnHeld(Buttons.Right))
                particleEffect_B.Start();
            else
                particleEffect_B.Stop();
        }

        public override void Render(Graphics g)
        {
            particleEffect_A.Render(g);
            particleEffect_B.Render(g);
        }

    }
