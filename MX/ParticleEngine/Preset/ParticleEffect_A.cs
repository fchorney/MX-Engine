using System;
using System.Collections.Generic;
using System.Text;
using MX;

namespace MX
{

    public class ParticleEffect_A : ParticleEffect
    {
        Limiter _Limiter;
        IPositionable _Dock;
        bool _Effecting;
        public int x, y;

        public ParticleEffect_A() : base()
        {
            _Limiter = new Limiter(0.02);
            _Effecting = false;
            x = 0;
            y = 0;
        }

        public IPositionable Dock
        {
            get { return _Dock; }
            set { _Dock = value; }
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
                //Params:
                //0: x-speed
                //1: y-speed
                Particle p = new Particle(RNG.Next(-20, 20), RNG.Next(-20, 20), RNG.Next(200));
                p.TimeToLive = 20;
                p.X = x;
                p.Y = y;
                p.Color = ColorPicker.Transparent(0);

                p.Transform += AdjustX;
                p.Transform += AdjustY;
                p.Transform += OpacityFunction;

                p.Model = AnimationFactory.GenerateAnimation("Images//particleloop.png", 32, 32, 5, 0.05);
                p.Image = p.Model.Frames[0];
                //p.Image = new Frame("Images\\particle.png", new System.Drawing.Rectangle(0, 0, 32, 32), 0);

                this.Add(p);
            }
        }

        public void AdjustX(IParticle p)
        {
            int direction = (p.X < GameEnvironment.Mouse.X ? 1 : -1);
            double speed = Math.Abs(p.X - GameEnvironment.Mouse.X) * direction;
            p.Params[0] += speed * GameEnvironment.ElapsedTime;
            p.X += (float)(p.Params[0] * GameEnvironment.ElapsedTime * 10);
        }

        public void AdjustY(IParticle p)
        {
            int direction = (p.Y < GameEnvironment.Mouse.Y ? 1 : -1);
            double speed = Math.Abs(p.Y - GameEnvironment.Mouse.Y) * direction;
            p.Params[1] += speed * GameEnvironment.ElapsedTime;
            p.Y += (float)(p.Params[1] * GameEnvironment.ElapsedTime * 10);
        }

        public void OpacityFunction(IParticle p)
        {
            double opacity = p.Params[2] + p.Params[0] + (p.Age * 100) % 200;

            p.Color = ColorPicker.Transparent(opacity < 100 ? opacity : 200 - opacity);
        }


    }

}
