using System;
using System.Drawing;
using System.Collections.Generic;

namespace MX
{

    public interface IForce : ICollidable
    {
        Vector2D Force { get; set; }
    }

    public class VectorForce : IForce
    {
        Vector2D force;
        IShape shape;

        public CollidableType Type
        {
            get { return CollidableType.Force; }
        }

        public VectorForce(int x, int y)
        {
            force = new Vector2D(x, y);
            shape = new Everywhere();
        }

        public Vector2D Force
        {
            get { return force; }
            set { force = value; }
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
                //case CollidableType.Force:
                //    Collide((IBody)Obj);
                //    break;
                //case CollidableType.Boundary:
                //    Collide((IBody)Obj);
                //    break;
            }
        }

        private void Collide(IBody Body)
        {
            if (!Body.Forces.Contains(this))
                Body.Forces.Add(this);
        }
    }

}
