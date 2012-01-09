using System;
using System.Drawing;
using System.Collections.Generic;

namespace MX
{
    public interface IParticle : ITransformable, IRenderable 
    {
        double Age { get; }
        double TimeToLive { get; }
        double ElapsedTime { get; }
        TransformFunction Transform { get; }
        double[] Params { get; }
    }

    public delegate void TransformFunction(IParticle p);

    public class Particle : Renderable, IParticle 
    {
        double _Age;
        double _TimeToLive;
        double _ElapsedTime;

        double[] _Params;

        TransformFunction _Function;
        Frame _Image;
        //IRenderable _Model;
        public Animation Model;

        public double Age
        {
            get { return _Age; }
        }

        public double TimeToLive
        {
            get { return _TimeToLive; }
            set { _TimeToLive = value; }
        }

        public double ElapsedTime
        {
            get { return _ElapsedTime; }
        }

        public TransformFunction Transform
        {
            get { return _Function; }
            set { _Function = value; }
        }

        public Frame Image
        {
            get { return _Image; }
            set { 
                _Image = value;
                Size.Set(_Image.Size.X, _Image.Size.Y);
            }
        }

        public double[] Params
        {
            get { return _Params; }
        }

        public Particle(params double[] Parameters)
        {
            _Age = 0;
            _Params = Parameters;
        }

        public void Update(double ElapsedTime)
        {
            _Age += ElapsedTime;
            _ElapsedTime = ElapsedTime;
            _Function(this);
            Model.Update(ElapsedTime);
        }

        public override void Render(Graphics g)
        {
            g.DrawFrame(Model, this, this);
        }

    }

}