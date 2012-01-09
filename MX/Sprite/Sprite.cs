using System;
using System.Drawing;
using System.Collections.Generic;

namespace MX
{

    public class Sprite : Renderable
    {
        public List<Animation> AnimationSet;
        public Animation CurrentAnimation;

        public Frame CurrentFrame
        {
            get { return CurrentAnimation.Frames[CurrentAnimation.CurrentFrame]; }
        }

        public Sprite()
        {
            AnimationSet = new List<Animation>();
        }

        public void AddAnimation(Animation anim)
        {
            AnimationSet.Add(anim);
            if (AnimationSet.Count == 1)
            {
                CurrentAnimation = anim;
                Size.Set(CurrentFrame._BaseWidth, CurrentFrame._BaseHeight);
            }
        }

        public void SetAnimation(int animIndex)
        {
            CurrentAnimation = AnimationSet[animIndex];
        }

        public override void Render(Graphics g)
        {
            g.DrawSprite(this);
        }
    }

    public class CollidableSprite : Sprite, IBody
    {
        protected IShape _Shape;
        protected Vector2D _Velocity;
        protected Vector2D _Acceleration;
        protected List<IForce> _Forces;
        protected double _Mass;

        public override double X
        {
            set 
            {
                Shape.X = value;
                Position.X = value;
            }
        }


        public override double Y
        {
            set
            {
                Shape.Y = value;
                Position.Y = value;
            }
        }

        public CollidableType Type
        {
            get { return CollidableType.Body; }
        }

        public IShape Shape
        {
            get { return CurrentFrame.Shape; }
        }

        public double Mass
        {
            get { return _Mass; }
            set { _Mass = value; }
        }

        public Vector2D Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; }
        }

        public Vector2D Acceleration
        {
            get { return _Acceleration; }
            set { _Acceleration = value; }
        }

        public List<IForce> Forces
        {
            get { return _Forces; }
        }

        public CollidableSprite() : base()
        {
            _Acceleration = new Vector2D();
            _Velocity = new Vector2D();
            _Forces = new List<IForce>();
        }

        public virtual void Collide(ICollidable Obj) { }

    }

}