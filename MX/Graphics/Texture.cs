using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MX
{

    public class Texture : IResource<Texture>
    {
        private static Microsoft.DirectX.Direct3D.Texture _EmptyTexture;
        private static Microsoft.DirectX.Direct3D.Texture EmptyTexture
        {
            get
            {
                if (_EmptyTexture == null)
                    _EmptyTexture = new Microsoft.DirectX.Direct3D.Texture(Graphics.GetInstance.Device, 4, 4, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
                return _EmptyTexture;
            }
        }

        private Microsoft.DirectX.Direct3D.Texture _DXTexture;

        public int Width
        {
            get { return _DXTexture.GetSurfaceLevel(0).Description.Width; }
        }

        public int Height
        {
            get { return _DXTexture.GetSurfaceLevel(0).Description.Height; }
        }

        internal Microsoft.DirectX.Direct3D.Texture Surface
        {
            get { return _DXTexture; }
        }

        public Texture()
        {
            _DXTexture = Texture.EmptyTexture;
        }

        public Texture(string path)
        {
            LoadFromFile(path);
        }

        public Texture Load(string path)
        {
            LoadFromFile(path);
            return this;
        }

        private void LoadFromFile(string path)
        {
            try
            {
                _DXTexture = TextureLoader.FromFile(Graphics.GetInstance.Device, path);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Texture " + path + " not found!\n" + e);
                _DXTexture = Texture.EmptyTexture;
            }
        }

    }

    /* Deprecated
public class Image : Renderable
{
    private Surface texture;
    protected string texturePath; // Path to the texture file

    public Surface Texture { get { return texture; } }

    public Image(string imagePath, int width, int height) 
    {
        _W = width;
        _H = height;
        SetScaling(_W, _H);
        texturePath = imagePath; // Store the texture's path
        Visible = true;
        texture = Graphics.GetInstance.Device.CreateOffscreenPlainSurface(width, height, Graphics.GetInstance.Settings.BackBufferFormat, Pool.Default);
        SurfaceLoader.FromFile(texture, texturePath, Filter.None, 0);
    }
    
}
*/
}