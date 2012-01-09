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
        double Mass { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double W { get; set; }
        double H { get; set; }
        Vector2D Velocity { get; set; }
        Vector2D Acceleration { get; set; }
        List<IForce> Forces { get; }
    }

    public class PhysicsEngine
    {
        QuadTree QTree;
        List<ICollidable> Objects;
        ICollidable ObjA, ObjB;
        Vector2D AccelerationVector;

        public PhysicsEngine()
        {

            QTree = new QuadTree(-100, -100, GameEnvironment.Screen.Width + 200, GameEnvironment.Screen.Height + 200);
            QTree.Split();
            QTree.Split();
            
            Objects = new List<ICollidable>();
            AccelerationVector = new Vector2D();
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

                        AccelerationVector.Clear();
                        AccelerationVector += Body.Acceleration;

                        for (int j = 0; j < Body.Forces.Count; j++)
                        {
                            AccelerationVector += Body.Forces[j].Force;
                        }

                        Body.Velocity.X += (AccelerationVector.X * ElapsedTime);
                        Body.Velocity.Y += (AccelerationVector.Y * ElapsedTime);

                        Body.X += Body.Velocity.X * ElapsedTime;
                        Body.Y += Body.Velocity.Y * ElapsedTime;

                        Body.Forces.Clear();
                        
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
                ObjA = Collisions.Get(i).Get(0);
                ObjB = Collisions.Get(i).Get(1);

                ObjA.Collide(ObjB);
                ObjB.Collide(ObjA);

                //Collisions.Get(i).Get(0).Collide(Collisions.Get(i).Get(1));
                //Collisions.Get(i).Get(1).Collide(Collisions.Get(i).Get(0));
            }

        }

        public void Render(Graphics g)
        {
            QTree.Render(g);
        }
    }

}
