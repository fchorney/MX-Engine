using System;
using System.Drawing;
using System.Collections.Generic;

namespace MX
{

    public class Ground : ICollidable, IRenderable
    {

        private IShape shape;

        private double _x;
        private double _y;
        private double _w;
        private double _h;

        protected bool _visible;
        protected Color _color;

        private IForce _Friction = new VectorForce(7200,0);

        public double X { get { return _x; } set { _x = value; shape.X = _x; } }
        public double Y { get { return _y; } set { _y = value; shape.Y = _y; } }
        public double W { get { return _w; } set { _w = value; shape.W = _w; } }
        public double H { get { return _h; } set { _h = value; shape.H = _h; } }

        public IForce Friction
        {
            get { return _Friction; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }


        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Ground(int x, int y, int w, int h)
        {

            shape = new Bottom(_y);
            //<33333
            X = x;
            Y = y;
            W = w;
            H = h;
            
        }

        public CollidableType Type
        {
            get { return CollidableType.Boundary; }
        }

        public IShape Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public void Collide(ICollidable Obj)
        {
            switch (Obj.Type)
            {
                case CollidableType.Body:
                    Collide((IBody)Obj);
                    break;
                case CollidableType.Force:
                    //Collide((IBody)Obj);
                    break;
                case CollidableType.Boundary:
                    //Collide((IBody)Obj);
                    break;
            }
        }

        private void Collide(IBody Body)
        {
            Body.Y = shape.Y - Body.Shape.H - 1;
            Body.Velocity.Y = 0;

            if (Body.Velocity.X != 0)
            {
                if (Body.Velocity.X > 0)
                {
                    Body.Velocity.X -= _Friction.Force.X * GameEnvironment.ElapsedTime;
                    if (Body.Velocity.X < 0)
                        Body.Velocity.X = 0;
                }
                else
                {
                    Body.Velocity.X += _Friction.Force.X * GameEnvironment.ElapsedTime;
                    if (Body.Velocity.X > 0)
                        Body.Velocity.X = 0;
                }
            }

        }

        public void Render(Graphics g)
        {
            ((Bottom)shape).Render(g);
        }//Render(mxGraphics g)
    }//Ground

}
