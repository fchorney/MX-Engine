using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MX
{
    /*
    public interface IControlContainer
    {
        mxGuiManager GuiManager { get; }
        void AddControl(IControl control);
        void RemoveControl(IControl control);
    }//IControlContainer

    public interface IControl : IPositionable
    {
        IControl Parent { get; set; }
        bool HasFocus { get; }
        bool Enabled { get; set; }
        void Update(double ElapsedTime);
    }//IControl

    public interface IClickable
    {
        //void OnClick();
    }

    public class mxGuiManager
    {
        public List<IControl> Controls = new List<IControl>();
        public bool HasFocus;

        public virtual void Update(double ElapsedTime)
        {
            HasFocus = false;
            foreach (IControl control in Controls)
            {
                control.Update(ElapsedTime);
                if (control is IControlContainer)
                {
                    ((IControlContainer)control).GuiManager.Update(ElapsedTime);
                    if (!HasFocus)
                        HasFocus = ((IControlContainer)control).GuiManager.HasFocus;
                }

                if (!HasFocus)
                    HasFocus = control.HasFocus;
                
            }
        }//Update(double ElapsedTime)

        public virtual void Render(Graphics g)
        {
            foreach (IControl control in Controls)
            {
                //control.Render(g);
                if (control is IControlContainer)
                    ((IControlContainer)control).GuiManager.Render(g);
                
            }
        }

    }//mxGuiManager

    public abstract class mxControl : IControl, IPositionable, IRenderable 
    {

        protected IControl _parent;
        public IControl Parent { get { return _parent; } set { _parent = value; } }

        protected mxGuiManager _guiManager = new mxGuiManager();
        public mxGuiManager GuiManager { get { return _guiManager; } }

        protected float x, y, r, w, h, z;
        protected bool _visible, _hasFocus, _enabled;

        public float X { get { return (Parent != null) ? x + Parent.X : x; } set { x = value; } }
        public float Y { get { return (Parent != null) ? y + Parent.Y : y; } set { y = value; } }
        public float W { get { return w; } set { w = value; } }
        public float H { get { return h; } set { h = value; } }
        public float Z { get { return z; } set { z = value; } }
        public float R { get { return r; } set { r = value; } }
        public bool Visible { get { return _visible; } set { _visible = value; } }
        public bool Enabled { get { return _enabled; } set { _enabled = value; } }

        public virtual void Position(float x, float y)
        {
            this.x = x;
            this.y = y;
            Image.Position.Set(x, y);
        }

        protected ITransformable Image;

        protected Color _color;
        public virtual Color Color
        {
            get { return _color; }
            set { 
                _color = value;
                //Image.Color = value;
            }
        }

        public virtual bool HasFocus { get { return _hasFocus; } }

        public abstract void Update(double ElapsedTime);

        protected bool Contains(int x, int y)
        {
            return x >= X && x <= X + W && y >= Y && y <= Y + H;
        }

        public virtual void Render(Graphics g)
        {
            Image.Position.Set(X, Y);
            //Image.Render(g);
            g.DrawText("[" + X + ", " + Y + "]", (int)X, (int)Y);
        }

    }//mxControl

    public class mxPanel : mxControl, IControlContainer
    {
        public delegate void OnClickHandler(object sender, EventArgs e);
        public event OnClickHandler OnClick;

        protected bool Dragging;
        protected float DragX, DragY;

        public override bool HasFocus { get { return _hasFocus || Dragging; } }

        public mxPanel(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            _visible = true;
            _hasFocus = false;
            Image = new Quad(x,y,w,h);

        }

        public override void Update(double ElapsedTime)
        {

            if (Contains(InputState.GetInstance.Mouse.X, InputState.GetInstance.Mouse.Y))
            {
                if (InputState.GetInstance.Mouse.BtnPressed(Buttons.Left))
                {
                    if (OnClick != null)
                        OnClick(this, EventArgs.Empty);

                    Dragging = true;
                    DragX = InputState.GetInstance.Mouse.X - X;
                    DragY = InputState.GetInstance.Mouse.Y - Y;

                    _hasFocus = true;
                    //Color = Color.Red;
                }
            }

            if (InputState.GetInstance.Mouse.BtnHeld(Buttons.Left) && Dragging)
            {

                X = InputState.GetInstance.Mouse.X - DragX;
                Y = InputState.GetInstance.Mouse.Y - DragY;
                Position(X, Y);
            }
            else
            {
                _hasFocus = false;
                Dragging = false;
                //Color = Color.White;
            }

        }//Update(double)


        public void AddControl(IControl control)
        {
            GuiManager.Controls.Add(control);
            control.Parent = this;
        }

        public void RemoveControl(IControl control)
        {
            GuiManager.Controls.Remove(control);
            control = null;
        }

    }//mxDraggable

    public class mxButton : mxControl
    {
        public delegate void OnClickHandler(object sender, EventArgs e);
        public event OnClickHandler OnClick;

        public mxButton(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            _visible = true;
            _hasFocus = false;
            Image = new Quad(x, y, w, h);
        }

        public override void Update(double ElapsedTime)
        {

            _hasFocus = Contains(InputState.GetInstance.Mouse.X, InputState.GetInstance.Mouse.Y);
            if (Contains(InputState.GetInstance.Mouse.X, InputState.GetInstance.Mouse.Y))
            {
                if (InputState.GetInstance.Mouse.BtnPressed(Buttons.Left))
                    OnClick(this, EventArgs.Empty);
                //Color = Color.Red;
            }
            //else
                //Color = Color.White;

        }//Update(double)

    }//mxButton

    public class mxTextbox : mxControl
    {
        public delegate void OnClickHandler(object sender, EventArgs e);
        public event OnClickHandler OnClick;

        public mxTextbox()
        {
            X = 20;
            Y = 90;
            W = 100;
            H = 15;
            _visible = true;
            _hasFocus = false;
            Image = new Quad(x, y, w, h);
        }

        public override void Update(double ElapsedTime)
        {

            if (InputState.GetInstance.Mouse.BtnPressed(Buttons.Left))
            {
                if (Contains(InputState.GetInstance.Mouse.X, InputState.GetInstance.Mouse.Y))
                {
                    if (OnClick != null)
                        OnClick(this, EventArgs.Empty);
                    _hasFocus = true;
                }
                else
                    _hasFocus = false;
            }
                

            //if (HasFocus)
                //Color = Color.Red;
            //else
                //Color = Color.White;

        }//Update(double)

    }//mxButton
    */
}//namespace MX
