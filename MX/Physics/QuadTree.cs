using System;
using System.Collections.Generic;
using System.Text;

namespace MX
{
    internal class QuadTree : Rectangle, IPositionable 
    {

        QuadTree Root;
        QuadTree[] Children;

        List<ICollidable> Objects;
        PairList<ICollidable> Collisions;

        public Vector2D Offset;

        public QuadTree(double x, double y, double w, double h)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            Root = this;
        }

        private QuadTree(double x, double y, double w, double h, QuadTree Root)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            this.Root = Root;
        }

        private bool QuadIntersects(IShape obj)
        {
            if (obj == null) return false;
            return MathUtil.CheckAxis(Root.Offset.X + X, Root.Offset.X + X + W, obj.X, obj.X + obj.W) &&
                   MathUtil.CheckAxis(Root.Offset.Y + Y, Root.Offset.Y + Y + H, obj.Y, obj.Y + obj.H);
        }

        public void Split()
        {
            if (Children != null)
                for (int i = 0; i < Children.Length; i++)
                    Children[i].Split();
            else
            {
                double hW = W / 2;
                double hY = H / 2;

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
                    if (Children[i].QuadIntersects(Obj.Shape))
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
                    if (Children[i].QuadIntersects(Obj.Shape))
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
                    //q.Render(g);
                    
                    
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
}
