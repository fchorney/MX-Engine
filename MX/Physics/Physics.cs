using System;
using System.Collections.Generic;
using System.Text;

namespace MX
{

    public enum CollidableType
    {
        Body,
        Boundary,
        Force
    }

    public interface ICollidable
    {
        CollidableType Type { get;}
        IShape Shape { get;}
        void Collide(ICollidable Obj);

    }

    public interface IBody :  ICollidable 
    {
        float Mass { get; set; }
        float X { get; set; }
        float Y { get; set; }
        float W { get; set; }
        float H { get; set; }
        Vector2D Velocity { get; set; }
        Vector2D Acceleration { get; set; }
    }

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
            Body.Acceleration.X += Force.X;
            Body.Acceleration.Y += Force.Y;
        }
    }

    internal class QuadTree : Rectangle, IPositionable 
    {

        QuadTree Root;
        QuadTree[] Children;

        List<ICollidable> Objects;
        PairList<ICollidable> Collisions;

        public Vector2D Offset;

        public QuadTree(float x, float y, float w, float h)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            Root = this;
        }

        private QuadTree(float x, float y, float w, float h, QuadTree Root)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            this.Root = Root;
        }

        //public override bool Intersects(IShape rect)
        //{

        //    if (rect == null) return false;
        //    if (rect.Type == ShapeType.Rectangle)
        //        return MathUtil.CheckAxis((float)Root.Offset.X + X, (float)Root.Offset.X + X + W, rect.X, rect.X + rect.W) &&
        //                MathUtil.CheckAxis((float)Root.Offset.Y + Y, (float)Root.Offset.Y + Y + H, rect.Y, rect.Y + rect.H);
        //    else
        //        return ((Rectangle)this).Intersects(rect);
        //}

        public void Split()
        {
            if (Children != null)
                for (int i = 0; i < Children.Length; i++)
                    Children[i].Split();
            else
            {
                float hW = W / 2;
                float hY = H / 2;

                Children = new QuadTree[4];
                Children[0] = new QuadTree(X, Y, hW, hY, Root);
                Children[1] = new QuadTree(X + hW, Y, hW, hY, Root);
                Children[2] = new QuadTree(X, Y + hY, hW, hY, Root);
                Children[3] = new QuadTree(X + hW, Y + hY, hW, hY, Root);

            }
        }

        public void Add(ICollidable Obj)
        {

            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    if (Children[i].Intersects(Obj.Shape))
                        Children[i].AddToChildren(Obj);
            }
            else
            {
                Objects.Add(Obj);
            }

        }

        private void AddToChildren(ICollidable Obj)
        {
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    if (Children[i].Intersects(Obj.Shape))
                        Children[i].AddToChildren(Obj);
            }
            else
            {
                Objects.Add(Obj);
            }
        }

        public void Clear()
        {
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    Children[i].Clear();
            }
            else
            {
                Objects.Clear();
            }
        }

        public PairList<ICollidable> GetCollisions()
        {
            return Collisions;
        }

        public void CheckCollisions()
        {
            Collisions.Clear();
            CheckChildCollisions(Collisions);
        }

        private void CheckChildCollisions(PairList<ICollidable> CollisionList)
        {
            if (Children != null)
                for (int i = 0; i < Children.Length; i++)
                    Children[i].CheckChildCollisions(CollisionList);
            else
            {
                Collisions.Clear();
                if (Objects.Count >= 2)
                    for (int i = 0; i < Objects.Count - 1; i++)
                        for (int j = i + 1; j < Objects.Count; j++)
                            if (Objects[i].Shape.Intersects(Objects[j].Shape))
                            {
                                CollisionList.Add(Objects[i], Objects[j]);
                                Collisions.Add(Objects[i], Objects[j]);
                            }
            }
        }

        public void Render(Graphics g)
        {
            if (Children != null)
                for (int i = 0; i < Children.Length; i++)
                    Children[i].Render(g);
            else
            {
                g.DrawText(Objects.Count.ToString(), (int)(X + Root.Offset.X), (int)(Y + Root.Offset.Y));

                if (Objects.Count > 0)
                {
                    Quad q = new Quad(this);
                    //q.Color = ColorPicker.RandomColor();
                    q.Render(g);
                    
                    
                    for (int i = 0; i < Objects.Count && Objects.Count > 1; i++)
                    {
                        if (Objects[i].Shape.GetType() == typeof(Rectangle))
                        {
                            q = new Quad((Rectangle)Objects[i].Shape);
                            q.Color = ColorPicker.ColorFromRGB(255, 0, 0);
                            q.Render(g);
                        }
                    }
                    for (int i = 0; i < Collisions.Count; i++)
                    {
                        if (Collisions.Get(i).Get(0).Shape.GetType() == typeof(Rectangle)
                            && Collisions.Get(i).Get(1).Shape.GetType() == typeof(Rectangle))
                        {
                            q = new Quad((Rectangle)Collisions.Get(i).Get(0).Shape);
                            q.Color = ColorPicker.ColorFromRGB(0, 255, 0);
                            q.Render(g);

                            q = new Quad((Rectangle)Collisions.Get(i).Get(1).Shape);
                            q.Color = ColorPicker.ColorFromRGB(0, 255, 0);
                            q.Render(g);
                            }
                    }
                    
                }
            }
        }
    }

    public class PhysicsEngine
    {
        QuadTree QTree;
        List<ICollidable> Objects;

        public PhysicsEngine()
        {

            QTree = new QuadTree(-100, -100, GameEnvironment.Screen.Width + 200, GameEnvironment.Screen.Height + 200);
            QTree.Split();
            QTree.Split();
            
            Objects = new List<ICollidable>();
        }

        public void Add(ICollidable Obj)
        {
            Objects.Add(Obj);
        }

        public void Update(double ElapsedTime)
        {
            QTree.Offset.X = GameEnvironment.Screen.Camera.X;
            QTree.Offset.Y = GameEnvironment.Screen.Camera.Y;
            ApplyPhysics(ElapsedTime, Objects);
            CheckCollisions(ElapsedTime, Objects);
        }

        private void ApplyPhysics(double ElapsedTime, List<ICollidable> Objects)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                switch (Objects[i].Type)
                {
                    case CollidableType.Body:
                        IBody Body = (IBody)Objects[i];


                        Body.Velocity.X += (float)(Body.Acceleration.X * ElapsedTime);
                        Body.Velocity.Y += (float)(Body.Acceleration.Y * ElapsedTime);

                        Body.X += (float)(Body.Velocity.X * ElapsedTime);
                        Body.Y += (float)(Body.Velocity.Y * ElapsedTime);

                        //Body.Acceleration.Clear();
                        
                        break;
                    case CollidableType.Force:
                        //Collide((IBody)Obj);
                        break;
                    case CollidableType.Boundary:
                        //Collide((IBody)Obj);
                        break;
                }
            }
        }

        private void CheckCollisions(double ElapsedTime, List<ICollidable> Objects)
        {
            QTree.Clear();
            for (int i = 0; i < Objects.Count; i++)
                QTree.Add(Objects[i]);
            QTree.CheckCollisions();

            PairList<ICollidable> Collisions = QTree.GetCollisions();
            for (int i = 0; i < Collisions.Count; i++)
            {
                Collisions.Get(i).Get(0).Collide(Collisions.Get(i).Get(1));
                Collisions.Get(i).Get(1).Collide(Collisions.Get(i).Get(0));
            }

        }

        public void Render(Graphics g)
        {
            QTree.Render(g);
        }
    }

}
