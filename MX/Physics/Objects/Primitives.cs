using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MX
{

    public abstract class PrimitiveShape : ITransformable, IRenderable
    {
        int numPoly = 1;

        protected double x, y, r, w, h, z, angle;
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public double W { get { return w; } set { w = value; } }
        public double H { get { return h; } set { h = value; } }
        public double Z { get { return z; } set { z = value; } }
        public double Rotation { get { return r; } set { r = value; } }

        internal Vector2D _Size = new Vector2D(1, 1);
        internal Vector2D _Position = new Vector2D(0, 0);

        public Vector2D Position
        {
            get { return _Position; }
        }

        public Vector2D Size
        {
            get { return _Size; }
        }

        protected bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        protected Color _color;
        public virtual Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                for (int i = 0; i < Vertices.Length; i++)
                    Vertices[i].Color = _color.ToArgb();
            }
        }

        protected CustomVertex.PositionColored[] Vertices;

        protected PrimitiveShape() { }

        public PrimitiveShape(CustomVertex.PositionColored[] vertices)
        {
            Vertices = new CustomVertex.PositionColored[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
                Vertices[i] = vertices[i];
        }

        public virtual void SetSize(float w, float h)
        {
            
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X = (float)(X + (Vertices[i].X - X) / W * w);
                Vertices[i].Y = (float)(Y + (Vertices[i].Y - Y) / H * h);
            }

            this.w = w;
            this.h = h;
        }

        public virtual void SetPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
            float dX = x - Vertices[0].X;
            float dY = y - Vertices[0].Y;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X += dX;
                Vertices[i].Y += dY;
            }
        }

        public virtual void SetRotation(float z)
        {
            angle = r;
        }

        public virtual void Render(Graphics g)
        {
            if (Visible)
            {
                g.Device.VertexFormat = CustomVertex.PositionColored.Format;
                g.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, numPoly, Vertices);
            }
        }
    }//mxPrimitive

    public class Line : PrimitiveShape
    {

        public Line(double x1, double y1, double x2, double y2) : this((float)x1, (float)y1, (float)x2, (float)y2) { }
        public Line(float x1, float y1, float x2, float y2)
        {

            Vertices = new CustomVertex.PositionColored[4];

            Vertices[0] = new CustomVertex.PositionColored(
                new Vector3(x1, y2, 0.0f), Color.White.ToArgb());
            Vertices[1] = new CustomVertex.PositionColored(
                new Vector3(x1, y1, 0.0f), Color.White.ToArgb());
            Vertices[2] = new CustomVertex.PositionColored(
                new Vector3(x2, y2, 0.0f), Color.White.ToArgb());
            Vertices[3] = new CustomVertex.PositionColored(
                new Vector3(x2, y1, 0.0f), Color.White.ToArgb());
        }

        public override void Render(Graphics g)
        {
            if (Visible)
            {
                g.SpriteBatch.Flush();
                g.Device.VertexFormat = CustomVertex.PositionColored.Format;
                g.Device.SetTexture(0, null);
                g.Device.Transform.World = Matrix.Translation(-(float)(X + GameEnvironment.Camera.X), -(float)(Y + GameEnvironment.Camera.Y), 0);
                g.Device.DrawUserPrimitives(PrimitiveType.LineStrip, Vertices.Length - 1, Vertices);
            }
        }
    }

    public class LineGrid : PrimitiveShape
    {

        public LineGrid(float w, float h, int rows, int cols)
        {

            Vertices = new CustomVertex.PositionColored[(rows + cols + 2) * 2];

            for (int i = 0; i < rows + 1; i++)
            {
                Vertices[i * 2] = new CustomVertex.PositionColored(
                    new Vector3(i * (w / rows), 0, 0.0f), ColorPicker.ColorFromRGB(255,255,255).ToArgb());
                Vertices[i * 2 + 1] = new CustomVertex.PositionColored(
                    new Vector3(i * (w / rows), h, 0.0f), ColorPicker.ColorFromRGB(255, 255, 255).ToArgb());
            }


            for (int i = rows + 1; i < rows + cols + 2; i++)
            {
                Vertices[i * 2] = new CustomVertex.PositionColored(
                    new Vector3(0, (i - rows - 1) * (h / cols), 0.0f), ColorPicker.ColorFromRGB(255, 255, 255).ToArgb());
                Vertices[i * 2 + 1] = new CustomVertex.PositionColored(
                    new Vector3(w, (i - rows - 1) * (h / cols), 0.0f), ColorPicker.ColorFromRGB(255, 255, 255).ToArgb());
            }
        }

        public override void Render(Graphics g)
        {
            if (Visible)
            {
                g.SpriteBatch.Flush();
                g.Device.VertexFormat = CustomVertex.PositionColored.Format;
                g.Device.SetTexture(0, null);
                g.Device.DrawUserPrimitives(PrimitiveType.LineList, Vertices.Length / 2, Vertices);
            }
        }
    }

    public class Triangle : PrimitiveShape
    {

        public Triangle()
        {

            Vertices = new CustomVertex.PositionColored[4];

            Vertices[0].Position = new Vector3(5f, 0f, 5f);
            Vertices[0].Color = Color.Red.ToArgb();
            Vertices[1].Position = new Vector3(5f, 15f, 5f);
            //Vertices[1].Color = Color.Green.ToArgb();
            Vertices[2].Position = new Vector3(10f, 0f, -5f);
            //Vertices[2].Color = Color.Yellow.ToArgb();
            Vertices[3].Position = new Vector3(10f, 15f, -5f);
            Vertices[3].Color = Color.Blue.ToArgb();

        }

        public override void Render(Graphics g)
        {
            angle += 0.05f;

            //g.D3DSprite.Flush();
            g.Device.VertexFormat = CustomVertex.PositionColoredTextured.Format;
            g.Device.Transform.World = Matrix.Translation(-7.5f, -7.5f, 0) * Matrix.RotationAxis(new Vector3(1, 0, 0.5f), (float)angle) * Matrix.Translation(7.5f, 7.5f, 0);
            g.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, Vertices);
        }
    }

    public class Quad : PrimitiveShape
    {
        public Quad(double x, double y, double w, double h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;

            Vertices = new CustomVertex.PositionColored[4];

            Vertices[0] = new CustomVertex.PositionColored(
                new Vector3((float)x, (float)y, 0.0f), Color.White.ToArgb());
            Vertices[1] = new CustomVertex.PositionColored(
                new Vector3((float)x, (float)(y + h), 0.0f), Color.White.ToArgb());
            Vertices[2] = new CustomVertex.PositionColored(
                new Vector3((float)(x + w), (float)y, 0.0f), Color.White.ToArgb());
            Vertices[3] = new CustomVertex.PositionColored(
                new Vector3((float)(x + w), (float)(y + h), 0.0f), Color.White.ToArgb());

        }

        public Quad(Rectangle rect)
            : this(rect.X, rect.Y, rect.W, rect.H)
        { }

        public override void Render(Graphics g)
        {
            if (Visible)
            {
                g.SpriteBatch.Flush();
                g.Device.VertexFormat = CustomVertex.PositionColored.Format;
                g.Device.SetTexture(0, null);
                g.Device.Transform.World = Matrix.Translation((float)(-(X + GameEnvironment.Camera.X) - W / 2), (float)-((Y + GameEnvironment.Camera.Y) - H / 2), 0) * Matrix.RotationAxis(new Vector3(0, 0, 1), (float)angle) * Matrix.Translation((float)(X + W / 2), (float)(Y + H / 2), 0);
                g.Device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, Vertices);
            }
        }

    }

}//namespace MX