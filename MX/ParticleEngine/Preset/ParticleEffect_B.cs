using System;
using System.Collections.Generic;
using System.Text;
using MX;

namespace MX
{

    public class ParticleEffect_B : ParticleEffect
    {
        Limiter _Limiter;
        bool _Effecting;
        public int x, y;

        public ParticleEffect_B() : base()
        {
            _Limiter = new Limiter(0.005);
            _Effecting = false;
            x = 200;
            y = 100;
        }

        public override void Start()
        {
            _Effecting = true;
        }

        public override void Stop()
        {
            _Effecting = false;
        }

        public override void Update(double ElapsedTime)
        {
            base.Update(ElapsedTime);
            _Limiter.Update(ElapsedTime);
            if (_Limiter.Ready && _Effecting)
            {
                Particle p = new Particle(RNG.Next(320), RNG.Next(4) + 1, RNG.Next(12));
                p.TimeToLive = 5;
                p.X = x;
                p.Y = y;
                p.Color = ColorPicker.FromArgb(0, ColorPicker.RandomColor());
                
                p.Transform += SinXFunction;
                p.Transform += SinYFunction;
                p.Transform += OpacityFunction;

                p.Model = AnimationFactory.GenerateAnimation("Images//particleloop.png", 32, 32, 5, 0.05);
                p.Image = p.Model.Frames[0];
                //p.Image = new Frame("Images\\particle.png", new System.Drawing.Rectangle(0, 0, 32, 32), 0);

                this.Add(p);
            }
        }

        public void SinXFunction(IParticle p)
        {
            p.X = GameEnvironment.Mouse.X + (float)(Math.Cos(p.Age * p.Params[1] + p.Params[2] * 50) * p.Params[0]);
        }

        public void SinYFunction(IParticle p)
        {
            p.Y = GameEnvironment.Mouse.Y + (float)(Math.Sin(p.Age * p.Params[1] + p.Params[2] * 50) * p.Params[0]);
        }

        public void OpacityFunction(IParticle p)
        {
            int opacity = (int)(p.Params[0] + (p.Age * 100) % 500);
            opacity = Math.Abs((opacity < 250 ? opacity : 250 - opacity) % 255);

            p.Color = ColorPicker.FromArgb(opacity, p.Color);
        }

    }

}
