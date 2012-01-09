using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MX
{

    public interface IRenderable
    {
        bool Visible { get; set; }
        Color Color { get; set; }
        void Render(Graphics g);
    }

    public interface IPositionable
    {
        double X { get; set; }
        double Y { get; set; }
        double W { get; set; }
        double H { get; set; }

    }

    public interface ITransformable : IPositionable
    {
        Vector2D Size { get; }
        Vector2D Position { get; }
        double Rotation { get; set; }
        double Z { get; set; }
    }

    public abstract class Renderable : ICloneable, ITransformable, IRenderable
    {

        internal Vector2 texCenter = new Vector2();
        internal Vector2D _Size = new Vector2D(1, 1);
        internal Vector2D _Position = new Vector2D(0, 0);
        internal double _Rotation;
        internal double _BaseWidth, _BaseHeight, _Z;

        public virtual double X { get { return _Position.X; } set { _Position.X = value; } }
        public virtual double Y { get { return _Position.Y; } set { _Position.Y = value; } }
        public virtual double W { get { return _Size.X; } set { _Size.X = value; } }
        public virtual double H { get { return _Size.Y; } set { _Size.Y = value; } }
        public virtual double Rotation { get { return (_Rotation * 180 / Math.PI); } set { _Rotation = (value * Math.PI / 180); } }
        public virtual double Z { get { return _Z; } set { _Z = value; } }
        
        protected bool _visible = true;
        protected Color _color = Color.White;

        public virtual bool Visible 
        { 
            get { return _visible; } 
            set { _visible = value; } 
        }

        public virtual Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        protected Renderable() { }
        public Renderable(string imagePath, int width, int height) { }

        public virtual object Clone() { return this.MemberwiseClone(); }
        public virtual void ReloadResources() { }
        //public virtual void Transform(Graphics g) { }
        public virtual void Render(Graphics g) { }
        public virtual void Render(Graphics g, Color color) { }

        public Vector2D Position
        {
            get { return _Position; }
        }

        public Vector2D Size
        {
            get { return _Size; }
        }

        /*
        public virtual void SetSize(float width, float height)
        {
            _Size.X = width;
            _Size.Y = height;
        }

        public virtual void Position.Set(float x, float y)
        {
            _Position.X = x;
            _Position.Y = y;
        }

        public virtual void SetRotation(float r)
        {
            _Rotation.Z = (float)(r*Math.PI/180);
        }

        */

    }

    public class Font
    {
        private System.Drawing.Font windowsFont;
        private Microsoft.DirectX.Direct3D.Font font;
        private System.Drawing.Color c = System.Drawing.Color.White;
        public static Font Default = new Font();
        public Color color
        {
            get { return c; }
            set { c = value; }
        }

        public Font() : this("Arial", 8.0f) {}

        public Font(string name, float size)
        {
            try
            {
                windowsFont = new System.Drawing.Font(name, size);
                font = new Microsoft.DirectX.Direct3D.Font(Graphics.GetInstance.Device, windowsFont);
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public void Render(string text, int x, int y)
        {
            try{
                Graphics.GetInstance.Device.SetTransform(TransformType.World, Matrix.Identity);
                Graphics.GetInstance.SpriteBatch.Transform = Matrix.Identity;
                font.DrawText(Graphics.GetInstance.SpriteBatch,text,x, y, c);
            }
            catch (Exception) { }
        }

        public static Font GetDefault
        {
            get { return new Font(); }
        }
    }

    public class ColorPicker
    {
        public static Color Transparent(double alpha)
        {
            alpha = (alpha / 100 * 255) % 255;
            if (alpha < 0) alpha = 0;
            if (alpha > 255) alpha = 255;
            return Color.FromArgb((int)alpha, Color.White);
        }

        public static Color ColorFromRGB(int r, int g, int b)
        {
            return Color.FromArgb(255, r, g, b);
        }

        public static Color RandomColor()
        {
            return Color.FromArgb(255, RNG.Next(255), RNG.Next(255), RNG.Next(255));
        }

        public static Color RandomColorWithAlpha()
        {
            return Color.FromArgb(RNG.Next(255), RNG.Next(255), RNG.Next(255), RNG.Next(255));
        }

        public static int RandomColorToInt()
        {
            return Color.FromArgb(RNG.Next(255), RNG.Next(255), RNG.Next(255), RNG.Next(255)).ToArgb();
        }

       public static Color FromArgb(int FadeAlpha, Color C)
       {
           return Color.FromArgb(FadeAlpha, C.R, C.G, C.B);
       }
    }

}