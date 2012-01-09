using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MX
{
	public class Graphics : IDisposable
    {

        #region Variables

        private static Graphics _Graphics;
        private static Font     F;
        private static int _W;
        private static int _H;
        private Vector2D _Camera;
        private Microsoft.DirectX.Direct3D.Sprite _SpriteBatch;
        private Device _Device;
        private PresentParameters _PresentParams;
        private System.Windows.Forms.Control _TargetControl;

        #endregion

        #region Properties

        internal Microsoft.DirectX.Direct3D.Sprite SpriteBatch { get { return _SpriteBatch; } }
        internal Device Device { get { return _Device; } }
        internal PresentParameters PresentParams { get { return _PresentParams; } }

        public int Width { get { return _W; } }
        public int Height { get { return _H; } }
        public Vector2D Camera { get { return _Camera; } }
        
        #endregion

        internal static void Init(System.Windows.Forms.Control targetControl, int width, int height)
        {
            if (_Graphics == null) _Graphics = new Graphics(targetControl,width,height);
            if (_Graphics._SpriteBatch == null) _Graphics._SpriteBatch = new Microsoft.DirectX.Direct3D.Sprite(_Graphics._Device);

            if (F == null) F = new Font();
        }

        internal static Graphics GetInstance
        {
            get { return _Graphics; }
        }

        internal Graphics(System.Windows.Forms.Control targetControl, int width, int height)
        {
            _W = width;
            _H = height;
            _Camera = new Vector2D();
            _TargetControl = targetControl;
            _TargetControl.Size =
                new Size(_W + 6, 
                         _H + 22); //TODO: Window border measurements
            _PresentParams = new PresentParameters();
            _PresentParams.Windowed = true;
            
            /*
            //full screen present params
            _presentParams.Windowed = false;
            _presentParams.DeviceWindow = targetControl;
            _presentParams.BackBufferFormat = Format.X8R8G8B8;
            _presentParams.BackBufferHeight = targetControl.Size.Height;
            _presentParams.BackBufferWidth = targetControl.Size.Width;
            _presentParams.PresentationInterval = PresentInterval.Default;
            */

            _PresentParams.PresentationInterval = PresentInterval.Immediate;
            _PresentParams.SwapEffect = SwapEffect.Discard;
            //_presentParams.AutoDepthStencilFormat = DepthFormat.D16;
            //_presentParams.EnableAutoDepthStencil = true;
            
            int adapterOrdinal = Manager.Adapters.Default.Adapter;

            Caps caps = Manager.GetDeviceCaps(adapterOrdinal, DeviceType.Hardware);
            CreateFlags createFlags;
            
            if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                createFlags = CreateFlags.HardwareVertexProcessing;
            else
                createFlags = CreateFlags.SoftwareVertexProcessing;

            if (caps.DeviceCaps.SupportsPureDevice)
                createFlags |= CreateFlags.PureDevice;

            createFlags |= CreateFlags.FpuPreserve;
            
            _Device = new Device(adapterOrdinal, DeviceType.Hardware, _TargetControl,
                  createFlags, _PresentParams);
            
            _Device.DeviceReset += new EventHandler(this.OnDeviceReset);

            OnDeviceReset(_Device, null);

        }

        private void OnDeviceReset(object sender, EventArgs e)
        {
            Device newDevice = (Device)sender;
            
            // get camera vectors
            float width = (float)_TargetControl.Size.Width;
            float height = (float)_TargetControl.Size.Height;

            /*
            float centerX = width / 2.0f;
            float centerY = height / 2.0f;
            Vector3 cameraPosition = new Vector3(centerX, centerY, -5.0f);
            Vector3 cameraTarget = new Vector3(centerX, centerY, 0.0f);

            // create our transforms
            
            newDevice.Transform.View = Matrix.LookAtLH(cameraPosition, cameraTarget,
                  new Vector3(0.0f, 1.0f, 0.0f));

            newDevice.Transform.Projection = Matrix.OrthoLH(width, height, -10.0f, 10.0f);
            */
            newDevice.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, width / height, 1f, 50f);
            //newDevice.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 30), new Vector3(0, 1, 0), new Vector3(0, 1, 0));
            newDevice.RenderState.Lighting = false;
            newDevice.RenderState.CullMode = Cull.None;

            //enable alphablending
            newDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            newDevice.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            newDevice.RenderState.AlphaBlendEnable = true;

          
        }

        internal void Clear()
        {
            _Device.Clear(ClearFlags.Target | ClearFlags.Target, System.Drawing.Color.Black, 1.0f, 0);
        }

        internal void BeginScene()
        {
            _Device.BeginScene();
            _SpriteBatch.Begin(SpriteFlags.AlphaBlend);
        }

        internal void EndScene()
        {
            _SpriteBatch.End();

            _Device.EndScene();
            
            _Device.Present();
        }

        public void DrawFrame(MX.Frame frame, ITransformable transformObj)
        {
            if (transformObj.X + this.Width - Camera.X > 0 && transformObj.Y + this.Height - Camera.Y > 0 &&
                transformObj.X - Camera.X < Width && transformObj.Y - Camera.Y < Height &&
                frame.Visible)
            {
                Vector3 Position = new Vector3((float)transformObj.Position.X, (float)transformObj.Position.Y, 0);
                Position.X -= (float)Camera.X;
                Position.Y -= (float)Camera.Y;
                SetTransform((float)frame._BaseWidth, (float)frame._BaseHeight, (float)transformObj.X, (float)transformObj.Y, (float)transformObj.W, (float)transformObj.H, (float)transformObj.Rotation);
                SpriteBatch.Draw(frame.Texture.Surface, frame.FrameArea, Vector3.Empty, Position, frame.Color.ToArgb());
            }
        }

        public void DrawFrame(MX.Frame frame, ITransformable transformObj, IRenderable renderableObj)
        {
            if (transformObj.X + this.Width - Camera.X > 0 && transformObj.Y + this.Height - Camera.Y > 0 &&
                transformObj.X - Camera.X < Width && transformObj.Y - Camera.Y < Height &&
                renderableObj.Visible)
            {
                Vector3 Position = new Vector3((float)transformObj.Position.X, (float)transformObj.Position.Y, 0);
                Position.X -= (float)Camera.X;
                Position.Y -= (float)Camera.Y;
                SetTransform((float)frame._BaseWidth, (float)frame._BaseHeight, (float)transformObj.X, (float)transformObj.Y, (float)transformObj.W, (float)transformObj.H, (float)transformObj.Rotation);
                SpriteBatch.Draw(frame.Texture.Surface, frame.FrameArea, Vector3.Empty, Position, renderableObj.Color.ToArgb());
            }
        }

        public void DrawFrame(MX.Animation animation, ITransformable transformObj)
        {
            DrawFrame(animation.Frames[animation.CurrentFrame], transformObj);
        }

        public void DrawFrame(MX.Animation animation, ITransformable transformObj, IRenderable renderableObj)
        {
            DrawFrame(animation.Frames[animation.CurrentFrame], transformObj, renderableObj);
        }

        public void DrawSprite(MX.Sprite sprite)
        {
            if (sprite.X + this.Width - Camera.X > 0 && sprite.Y + this.Height - Camera.Y > 0 &&
                sprite.X - Camera.X < Width && sprite.Y - Camera.Y < Height &&
                sprite.Visible)
            {

                Vector3 Position = new Vector3((float)(sprite.Position.X - sprite.CurrentFrame.Offset.X), (float)(sprite.Position.Y - sprite.CurrentFrame.Offset.Y), 0);
                Position.X -= (float)Camera.X;
                Position.Y -= (float)Camera.Y;
                SetTransform((float)sprite.CurrentFrame._BaseWidth, (float)sprite.CurrentFrame._BaseHeight, (float)sprite.X, (float)sprite.Y, (float)sprite.W, (float)sprite.H, (float)sprite.Rotation);
                SpriteBatch.Draw(sprite.CurrentFrame.Texture.Surface, sprite.CurrentFrame.FrameArea, Vector3.Empty, Position, sprite.Color.ToArgb());
            }
        }

        internal void SetTransform(MX.ITransformable objTransformable)
        {
            //rotation
            Quaternion rotate = new Quaternion();
            rotate.RotateYawPitchRoll(0, 0, (float)objTransformable.Rotation);

            //scaling
            Vector3 scale = new Vector3((float)(objTransformable.Size.X / objTransformable.W), (float)(objTransformable.Size.Y / objTransformable.H), 1.0f);
            Vector3 translation = Vector3.Empty;
            if (scale.X != 1 || scale.Y != 1)
                translation = new Vector3((float)(Camera.X * (scale.X - 1)), (float)(Camera.Y * (1 - scale.Y)), 0);

            this.SpriteBatch.Transform = Matrix.Transformation(
                new Vector3((float)objTransformable.Position.X, (float)objTransformable.Position.Y, 0), //scales from top left
                Quaternion.Identity,
                scale,
                new Vector3((float)(objTransformable.Position.X + objTransformable.Size.X / 2), (float)(objTransformable.Position.Y + objTransformable.Size.Y / 2), 0.0f), //rotates from center
                rotate,
                translation);
        }
        
        internal void SetTransform(float baseW, float baseH, float x, float y, float w, float h, float r)
        {
            //rotation
            Quaternion RotateVector = new Quaternion();
            RotateVector.RotateYawPitchRoll(0, 0, r);

            //scaling
            Vector3 ScaleVector = new Vector3(w / baseW, h / baseH, 1.0f);
            Vector3 TranslateVector = Vector3.Empty;
            if (ScaleVector.X != 1 || ScaleVector.Y != 1)
                TranslateVector = new Vector3((float)(Camera.X * (ScaleVector.X - 1)), (float)(Camera.Y * (1 - ScaleVector.Y)), 0);

            this.SpriteBatch.Transform = Matrix.Transformation(
                new Vector3(x, y, 0), //scales from top left
                Quaternion.Identity,
                ScaleVector,
                new Vector3(x + w / 2, y + h / 2, 0.0f), //rotates from center
                RotateVector,
                TranslateVector);
        }

        public void DrawText(string text, int x, int y)
        {
            F.Render(text, x - (int)Camera.X, y - (int)Camera.Y);
        }

        public void DrawText(string text, int x, int y, Font f)
        {
            F.Render(text, x - (int)Camera.X, y - (int)Camera.Y);
        }

        public void Dispose()
        {
            _Graphics = null;
            F = null;
            _TargetControl.Dispose();
            _TargetControl = null;
            _SpriteBatch.Dispose();
            _SpriteBatch = null;
            _Device.Dispose();
            _Device = null;
        }

	}

}