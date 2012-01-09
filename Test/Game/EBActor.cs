//using System;
//using System.Collections.Generic;
//using System.Text;
//using mxGameLib;
//using mxGameLib.Util;
//using MX;

//namespace EB
//{

//    enum SpriteDirection
//    {
//        Right,
//        Left
//    }
//    enum SpriteState
//    {
//        Standing,
//        Walking,
//        Running,
//        Jumping
//    }

//    class EBSprite : AnimatedActor
//    {
//        public string name = "";
//        private SpriteDirection _Direction;
//        public SpriteDirection Direction
//        {
//            get { return _Direction; }
//            set { _Direction = value; }
//        }

//        private SpriteState _State;
//        public SpriteState State
//        {
//            get { return _State; }
//            set { _State = value; }
//        }

//        private int _Speed;
//        public int Speed
//        {
//            get { return _Speed; }
//            set { _Speed = value; }
//        }

//        public EBSprite(float x, float y, float w, float h, AnimatedSprite i) : base(x, y, w, h, i) 
//        {
//            _Speed = 200;
//        }

//        public override void Update(double ElapsedTime)
//        {

//            base.Update(ElapsedTime);

//            switch (_State)
//            {
//                case SpriteState.Standing:
//                    if (Direction == SpriteDirection.Right)
//                        ChangeAnimation(0);
//                    else if (Direction == SpriteDirection.Left)
//                        ChangeAnimation(3);
//                    break;

//                case SpriteState.Walking:
//                    if (Direction == SpriteDirection.Right)
//                    {
//                        Velocity.X = _Speed;
//                        ChangeAnimation(1);
//                    }
//                    else if (Direction == SpriteDirection.Left)
//                    {
//                        Velocity.X = _Speed * -1;
//                        ChangeAnimation(4);
//                    }
//                    break;

//                case SpriteState.Running:
//                    if (Direction == SpriteDirection.Right)
//                    {
//                        Velocity.X = _Speed * 2;
//                        ChangeAnimation(1);
//                    }
//                    else if (Direction == SpriteDirection.Left)
//                    {
//                        Velocity.X = _Speed * -2;
//                        ChangeAnimation(4);
//                    }
//                    break;

//                case SpriteState.Jumping:

//                    break;
//                default:
//                    break;

//            }

//        }

//        public override void Collide(ICollidable Obj)
//        {
//            if (Obj.GetType() == typeof(EBSprite))
//            {
//            }
//        }

//    }


//}//namespace EB
