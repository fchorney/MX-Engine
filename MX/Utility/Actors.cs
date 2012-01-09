//using System;
//using System.Drawing;

//namespace MX
//{
//    public class StaticActor : IRenderable, ITransformable, IPositionable, IBody
//    {
//        //properties

//        public CollidableType Type
//        {
//            get { return CollidableType.Body; }
//        }

//        protected Renderable image;
//        public virtual Renderable Image
//        {
//            get
//            {
//                image.Position.Set(x, y);
//                image.Size.Set(w, h); 
//                image.Rotation = r; 
//                return image;
//            }
//            set { image = value; }
//        }

//        protected Color color;
//        public virtual Color Color
//        {
//            get
//            {
//                return color;
//            }
//            set { color = value; image.Color = value; }
//        }

//        protected float x, y, r, w, h, z;
//        protected bool  visible;

//        //accessor fields
//        public float X { get { return x; } set { x = value; if (shape != null)shape.X = x + 45; } }
//        public float Y { get { return y; } set { y = value; if (shape != null)shape.Y = y + 27; } }
//        public float W { get { return w; } set { w = value; } }
//        public float H { get { return h; } set { h = value; } }
//        public float Z { get { return z; } set { z = value; } }
//        public float Rotation {  get { return r; } set { r = value;} }
//        public float Xf { get { return x; } set { x = value; } }
//        public float Yf { get { return y; } set { y = value; } }
//        public bool Visible { get { return visible; } set { visible = value; } }

//        internal Vector2D _Size = new Vector2D(1, 1);
//        internal Vector2D _Position = new Vector2D(0, 0);

//        public Vector2D Position
//        {
//            get { return _Position; }
//        }

//        public Vector2D Size
//        {
//            get { return _Size; }
//        }

//        protected IShape shape;
//        public IShape Shape
//        {
//            get { return shape;}
//        }

//        protected float mass;
//        public float Mass
//        {
//            get { return mass; }
//            set { mass = value; }
//        }

//        protected Vector2D velocity;
//        public Vector2D Velocity
//        {
//            get { return velocity; }
//            set { velocity = value; }
//        }

//        protected Vector2D acceleration;
//        public Vector2D Acceleration
//        {
//            get { return acceleration; }
//            set { acceleration = value; }
//        }

//        public virtual void Collide(ICollidable Obj) { }

//        //constructor
//        private StaticActor() { }
//        public StaticActor(float x, float y, float w, float h, Renderable i)
//        {
//            this.x = x;
//            this.y = y;
//            this.w = w;
//            this.h = h;
//            image = i;
//            visible = true;
//            velocity = new Vector2D();
//            acceleration = new Vector2D();
//        }

//        public virtual void SetBoundingBox(IShape shape)
//        {
//            this.shape = shape;
//        }

//        public virtual void Update(double ElapsedTime) 
//        {
            
//        }

//        public virtual void Render(Graphics graphics) {
//            Render(graphics, Color.White);
//        }

//        public virtual void Render(Graphics g, Color color)
//        {
//            if (Visible)
//            {
//                image.Position.Set(x, y);
//                image.Size.Set(w, h);
//                image.Rotation = r;
//                image.Render(g,color);

//            }
//        }

//    }

//    public class AnimatedActor : StaticActor
//    {

//        new protected AnimatedSprite image;

//        //private mxAnimatedActor() { }
//        public AnimatedActor(float x, float y, float w, float h, AnimatedSprite i)
//            : base(x, y, w, h, i)
//        {
//            image = i;
//        }

//        public void AnimationSpeed(double speed)
//        {
//            image.AnimSpeed = speed;
//        }

//        public void ChangeAnimation(int state)
//        {
//            image.AnimState = state;
//        }

//        public override void Update(double ElapsedTime)
//        {
//            base.Update(ElapsedTime);

//            image.Update(ElapsedTime);

//        }

//    }

//    public class Ground : ICollidable, IRenderable
//    {

//        private IShape shape;
//        private Sprite _Image;

//        private float _x;
//        private float _y;
//        private float _w;
//        private float _h;
//        private float _Friction = 1200;

//        public float Friction
//        {
//            get { return _Friction; }
//            set { _Friction = value; }
//        }
//        public float X { get { return _x; } set { _x = value; } }
//        public float Y { get { return _y; } set { _y = value; shape.Y = _y; } }
//        public float W { get { return _w; } set { _w = value; } }
//        public float H { get { return _h; } set { _h = value; } }

//        protected bool _visible;
//        public bool Visible
//        {
//            get { return _visible; }
//            set { _visible = value; }
//        }

//        protected Color _color;
//        public Color Color
//        {
//            get { return _color; }
//            set { _color = value; }
//        }

//        public Ground(int x, int y, int w, int h, Sprite Image)
//        {
//            _x = x;
//            _y = y;
//            _w = w;
//            _h = h;
//            _Image = Image;
//            shape = new Bottom(_y);
//        }

//        public CollidableType Type
//        {
//            get { return CollidableType.Boundary; }
//        }

//        public IShape Shape
//        {
//            get { return shape; }
//            set { shape = value; }
//        }

//        public void Collide(ICollidable Obj)
//        {
//            switch (Obj.Type)
//            {
//                case CollidableType.Body:
//                    Collide((IBody)Obj);
//                    break;
//                case CollidableType.Force:
//                    //Collide((IBody)Obj);
//                    break;
//                case CollidableType.Boundary:
//                    //Collide((IBody)Obj);
//                    break;
//            }
//        }

//        private void Collide(IBody Body)
//        {
//            Body.Y = shape.Y - Body.H + 31;
//            Body.Velocity.Y = 0;

//            if (Body.Velocity.X != 0)
//                if (Body.Velocity.X > 0)
//                {
//                    Body.Velocity.X -= (float)(_Friction * GameEnvironment.ElapsedTime);
//                    if (Body.Velocity.X < 0) Body.Velocity.X = 0;
//                }
//                else
//                {
//                    Body.Velocity.X += (float)(_Friction * GameEnvironment.ElapsedTime);
//                    if (Body.Velocity.X > 0) Body.Velocity.X = 0;
//                }

//        }

//        public void Render(Graphics g)
//        {
//            float w = _Image.W;
//            float h = _Image.H;
//            int cols = (int)Math.Ceiling(_w / _Image.W);
//            int rows = (int)Math.Ceiling(_h / _Image.H);

//            for (int row = 0; row < rows; row++)
//            {
//                for (int col = 0; col < cols; col++)
//                {
//                    _Image.Position.Set(_x + col * w, _y + row * h);
//                    _Image.Render(g);
//                }
//            }
//            ((Bottom)shape).Render(g);
//        }//Render(mxGraphics g)
//    }//Ground

//}//namespace MX