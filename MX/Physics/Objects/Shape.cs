using System;
using System.Collections.Generic;

namespace MX
{
    public enum ShapeType
    {
        Rectangle,
        Circle,
        Everywhere,
        Top,
        Bottom
    }

    public interface IShape
    {
        ShapeType Type { get; }
        bool Intersects(IShape shape);
        double X { get; set; }
        double Y { get; set; }
        double W { get; set; }
        double H { get; set; }
    }

    public class Everywhere : IShape
    {
        public ShapeType Type { get { return ShapeType.Everywhere; } }

        public Everywhere() { }

        public double X { get { return -int.MaxValue / 2; } set { ;} }
        public double Y { get { return -int.MaxValue / 2; } set { ;} }
        public double W { get { return int.MaxValue; } set { ;} }
        public double H { get { return int.MaxValue; } set { ;} }

        public bool Intersects(IShape shape)
        {
            return Intersect.EverywhereToShape(this, shape);
        }
    }

    public class Bottom : IShape
    {
        public ShapeType Type { get { return ShapeType.Bottom; } }

        private double x,y,w,h;

        public double X { get { return x; } set { x = value;} }
        public double Y { get { return y; } set { y = value; } }
        public double W { get { return w; } set { w = value;} }
        public double H { get { return h; } set { h = value;} }

        public Bottom(double height)
        {
            y = height;
        }

        public bool Intersects(IShape shape)
        {
            return Intersect.BottomToShape(this, shape);
        }

        public void Render(Graphics g)
        {
            Line line = new Line(0, (int)y, 5000, (int)y);
            line.Color = ColorPicker.ColorFromRGB(255, 0, 0);
            line.Render(g);
        }
    }

    public class Top : IShape
    {
        public ShapeType Type { get { return ShapeType.Top; } }

        private double y;

        public double X { get { return double.MaxValue; } set { ;} }
        public double Y { get { return y; } set { y = value; } }
        public double W { get { return double.MaxValue; } set { ;} }
        public double H { get { return double.MaxValue; } set { ;} }

        public Top(double height)
        {
            y = height;
        }

        public bool Intersects(IShape shape)
        {
            return Intersect.TopToShape(this, shape);
        }

        public void Render(Graphics g)
        {
            Line line = new Line(0, (int)y, 5000, (int)y);
            line.Color = ColorPicker.ColorFromRGB(255, 0, 0);
            line.Render(g);
        }
    }

    public class Circle : IShape
    {
        public ShapeType Type { get { return ShapeType.Circle; } }

        double x;
        double y;
        double r;

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public double W
        {
            get { return r * 2; }
            set { r = value / 2; }
        }
        public double H
        {
            get { return r * 2; }
            set { r = value / 2; }
        }
        public double R
        {
            get { return r; }
            set { r = value; }
        }

        public double cX
        {
            get { return x + r; }
        }

        public double cY
        {
            get { return y + r; }
        }

        public Circle(float x, float y, float r)
        {
            this.x = x;
            this.y = y;
            this.r = r;
        }

        public bool Intersects(IShape shape)
        {
            switch (shape.Type)
            {
                case ShapeType.Everywhere:
                    return Intersect.EverywhereToShape((Everywhere)shape, this);
                case ShapeType.Bottom:
                    return Intersect.BottomToShape((Bottom)shape, this);
                case ShapeType.Top:
                    return Intersect.TopToShape((Top)shape, this);
                case ShapeType.Rectangle:
                    return Intersect.CircleToRectangle(this, (Rectangle)shape);
                case ShapeType.Circle:
                    return Intersect.CircleToCircle(this, (Circle)shape );
                default:
                    return false;
            }
        }
        
    }

    public class Rectangle : IShape
    {
        public ShapeType Type { get { return ShapeType.Rectangle; } }

        Vector2D v;

        public double X
        {
            get { return v.Position.X; }
            set { v.Position.X = value; }
        }
        public double Y
        {
            get { return v.Position.Y; }
            set { v.Position.Y = value; }
        }
        public double W
        {
            get { return v.X; }
            set { v.X = value; }
        }
        public double H
        {
            get { return v.Y; }
            set { v.Y = value; }
        }

        private Rectangle() { }

        public Rectangle(double x, double y, double w, double h)
        {
            v = new Vector2D(w, h, x, y);
        }

        public virtual bool Intersects(IShape shape)
        {
            switch (shape.Type)
            {
                case ShapeType.Everywhere:
                    return Intersect.EverywhereToShape((Everywhere)shape, this);
                case ShapeType.Bottom:
                    return Intersect.BottomToShape((Bottom)shape, this);
                case ShapeType.Top:
                    return Intersect.TopToShape((Top)shape, this);
                case ShapeType.Circle:
                    return Intersect.CircleToRectangle((Circle)shape, this);
                case ShapeType.Rectangle:
                    return Intersect.RectangleToRectangle((Rectangle)shape, this);
                default:
                    return false;
            }
        }

        public bool Contains(Point p)
        {
            return (p.X >= X && p.X <= X + W && p.Y >= Y && p.Y <= Y + H);
        }
    }
}
