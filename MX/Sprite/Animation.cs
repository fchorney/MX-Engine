using System;
using System.Drawing;
using System.Collections.Generic;

namespace MX
{

    public class Frame : Renderable
    {
        private string _TexturePath;
        private Texture _Texture;
        private System.Drawing.Rectangle _FrameArea;
        private IShape _Shape;
        private Vector2D _Offset;

        public Texture Texture{ 
            get { return _Texture; } 
        }

        public System.Drawing.Rectangle FrameArea
        {
            get { return _FrameArea; }
        }

        public IShape Shape
        {
            get { return _Shape; }
            set { _Shape = value; }
        }

        public Point Offset
        {
            get { return _Offset; }
        }

        public Frame(string texturePath, System.Drawing.Rectangle area)
        {
            _TexturePath = texturePath;
            _Texture = GameResources.Load<Texture>(_TexturePath);
            _FrameArea = area;
            _Offset = new Vector2D();
            _BaseWidth = area.Width;
            _BaseHeight = area.Height;
            Size.Set(_BaseWidth, _BaseHeight);

            Visible = true;
            Color = Color.White;
        }

        public override void ReloadResources()
        {
            _Texture = GameResources.Load<Texture>(_TexturePath);  // Load texture 
        }

        public override void Render(Graphics g)
        {
            g.DrawFrame(this, this);
        }
    }

    public class Animation : Renderable
    {

        public List<Frame> Frames;  //Rectangles for each frame of animation
        public int CurrentFrame;    //current frame of animation
        public double AnimationSpeed;

        internal int _TotalFrames;  //total number of frames
        public int TotalFrames
        {
            get { return _TotalFrames; }
        }

        private double _TimeInFrame;

        internal Animation() { }

        public override void Render(Graphics g)
        {
            Frames[CurrentFrame].Render(g);
        }

        public void Update(double ElapsedTime)
        {
            _TimeInFrame += ElapsedTime;
            if (_TimeInFrame >= AnimationSpeed)
            {
                CurrentFrame++;
                if (CurrentFrame == TotalFrames)
                    CurrentFrame = 0;
                _TimeInFrame = 0;
            }
        }

    }

    public class AnimationFactory
    {
        public static Animation GenerateAnimation(string texturePath, int frameWidth, int frameHeight, int animLength, double animSpeed)
        {
            Animation _Animation = new Animation();
            Texture _Texture = GameResources.Load<Texture>(texturePath);

            int Cols = _Texture.Width / frameWidth;
            int Rows = _Texture.Height / frameHeight;

            _Animation.W = frameWidth;
            _Animation.H = frameHeight;
            _Animation.Size.Set(frameWidth, frameHeight);

            _Animation.Frames = new List<Frame>();
            _Animation.CurrentFrame = 0;
            _Animation._TotalFrames = animLength; // Cols* Rows;

            for (int j = 0; j < Rows; j++)
                for (int i = 0; i < Cols && j * Cols + i < animLength; i++)
                {
                    Frame _Frame = new Frame(texturePath, new System.Drawing.Rectangle(i * frameWidth, j * frameHeight, frameWidth, frameHeight));
                    _Frame.Offset.X = 40;
                    _Frame.Offset.Y = 30;

                    _Frame.Shape = new Rectangle(0,0,32,65);
                    _Animation.Frames.Add(_Frame);
                }

            _Animation.AnimationSpeed = animSpeed;
            return _Animation;
        }

    }

}