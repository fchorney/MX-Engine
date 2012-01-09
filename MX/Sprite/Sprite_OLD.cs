//using System;
//using System.Drawing;
//using System.Collections.Generic;
//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;

//namespace MX
//{

//    public class Sprite : Renderable
//    {
//        protected Texture texture;      // Texture object used for rendering
//        public Texture Texture { get { return texture; } }

//        protected string texturePath;   // Path to the texture file

//        public List<System.Drawing.Rectangle> Frames; //Rectangles for each frame of animation
//        public int CurrentFrame;       //current frame of animation
//        protected int TotalFrames;        //total number of frames

//        protected Sprite() { }
//        public Sprite(string imagePath, int width, int height) : this(imagePath, 0, 0, width, height) { }
//        public Sprite(string imagePath, int x, int y, int width, int height)
//        {
//            _BaseWidth = width;
//            _BaseHeight = height;
//            Size.Set(_BaseWidth, _BaseHeight);
//            texturePath = imagePath;                   // Store the texture's path
//            texture = GameResources.Load<Texture>(texturePath); // Load texture 

//            Frames = new List<System.Drawing.Rectangle>();
//            Frames.Add(new System.Drawing.Rectangle(x, y, (int)_BaseWidth, (int)_BaseHeight));
//            CurrentFrame = 0;

//            Visible = true;
//        }

//        public override void ReloadResources()
//        {
//            texture = GameResources.Load<Texture>(texturePath);  // Load texture 
//        }

//        public override void Render(Graphics g)
//        {
//            Render(g, Color.White);
//        }//Render()

//        public override void Render(Graphics g, Color color)
//        {
//            g.DrawSprite(this);
//            this.Color = color;
//        }//Render()

//    }//class mxStaticSprite

//    public class AnimatedSprite : Sprite
//    {
//        int[] AnimStart;    //beginning indexes of each animation
//        int[] AnimLength;   //ending indexes of each animation 
//        int _AnimState;    //current state of animation
//        double _AnimSpeed;  //time to spend in this frame (ms)
//        double TimeInFrame; //time in current frame (ms)

//        public double AnimSpeed
//        {
//            get { return _AnimSpeed; }
//            set { _AnimSpeed = value; }
//        }//AnimSpeed

//        public int AnimState
//        {
//            get { return _AnimState; }
//            set
//            {
//                if (value < AnimStart.Length && value != _AnimState)
//                {
//                    if (value != _AnimState)
//                        CurrentFrame = AnimStart[value];
//                    _AnimState = value;
//                }
//            }
//        }//AnimState

//        public void Update(double ElapsedTime)
//        {
//            TimeInFrame += ElapsedTime;
//            if (TimeInFrame >= _AnimSpeed)
//            {
//                NextFrame();
//                //LastUpdated -= animSpeed;
//                TimeInFrame = 0;
//            }
//        }//Update(double)

//        public void NextFrame()
//        {
//            CurrentFrame =
//            ((CurrentFrame >= (AnimStart[_AnimState] + AnimLength[_AnimState]) - 1) ?
//                AnimStart[_AnimState] : //reset current animation
//                CurrentFrame + 1);      //advance current frame
//        }//NextFrame()

//        public AnimatedSprite(string imagePath, int width, int height, int[] length)
//            : base(imagePath, width, height)
//        {
//            GenerateFrames();
//            _AnimState = 0;
//            AnimLength = length;
//            AnimStart = new int[AnimLength.Length];
//            for (int i = 1; i < AnimLength.Length; i++)
//                AnimStart[i] = AnimStart[i - 1] + AnimLength[i - 1];
//        }//mxAnimatedSprite()

//        protected void GenerateFrames()
//        {

//            int frmW = texture.Width  / (int)_BaseWidth;
//            int frmH = texture.Height / (int)_BaseHeight;

//            TotalFrames = frmW * frmH;

//            Frames.Clear();

//            for (int j = 0; j < frmH; j++)
//                for (int i = 0; i < frmW; i++)
//                    Frames.Add(new System.Drawing.Rectangle(i * (int)_BaseWidth, j * (int)_BaseHeight, (int)_BaseWidth, (int)_BaseHeight));
//        }//GenerateFrames()

//    }//class mxAnimatedSprite

//}