using System;
using MX;

namespace MX.Util
{

    public class Fader : Module
    {
        //Properties
        private System.Drawing.Color _FadeColor;
        private int _MaxAlpha;
        private int _MinAlpha;
        private int FadeAlpha;
        private double FadeTime;
        private bool Faded;
        private bool Fading;
        private bool AutoFadeBack;
        private bool Pulsing;
        private Timer FadeTimer;
        private Quad FadeScreen;
 
        public System.Drawing.Color Color
        {
            get { return _FadeColor; }
            set { _FadeColor = value; }
        }

        public bool IsFaded { get { return Faded; } }

        public int MaxAlpha
        {
            get { return _MaxAlpha; }
            set {
                if (value > 255)
                    _MaxAlpha = 255;
                else if (!(value < 0 || value < _MinAlpha))
                    _MaxAlpha = value;
            }
        }

        public int MinAlpha
        {
            get { return _MinAlpha; }
            set
            {
                if (value < 0)
                    _MinAlpha = 0;
                else if (!(value > 255 || value > _MaxAlpha))
                    _MinAlpha = value;
            }
        }

        //Constuctor
        public Fader(string n) : base(n) { }
        public Fader(string n, bool running) : base(n, running) { }

        //Initialiser
        public override void Init()
        {
            _FadeColor = System.Drawing.Color.Black;
            _MinAlpha = 0;
            _MaxAlpha = 255;
            FadeAlpha = 0;
            Faded = false;
            Fading = false;
            AutoFadeBack = false;
            FadeTimer = new Timer();
            FadeScreen = new Quad(0, 0, GameEnvironment.Screen.Width, GameEnvironment.Screen.Height);
        }

        //Frame logic
        public override void Update(double ElapsedTime)
        {
            if (Fading)
            {
                FadeScreen.X = GameEnvironment.Camera.X;
                FadeScreen.Y = GameEnvironment.Camera.Y;

                if (Faded)
                    FadeAlpha = (int)(_MaxAlpha * Math.Abs(FadeTimer.GetTime() - FadeTime) / FadeTime);
                else
                    FadeAlpha = _MaxAlpha - (int)(_MaxAlpha * Math.Abs(FadeTimer.GetTime() - FadeTime) / FadeTime);

                if (FadeAlpha > _MaxAlpha) FadeAlpha = _MaxAlpha;
                if (FadeAlpha < _MinAlpha) FadeAlpha = _MinAlpha;

                double t = FadeTimer.GetTime();

                if (FadeTimer.GetTime() > FadeTime)
                {
                    Fading = false;
                    Faded = !Faded;

                    FadeTimer.Reset();
                    FadeTimer.Stop();
                    if (AutoFadeBack)
                    {
                        FadeIn(FadeTime);
                        AutoFadeBack = false;
                    }
                    if (Pulsing)
                    {
                        FadeOutAndIn(FadeTime*2);
                    }
                }
            }
        }//Update()

        public override void ProcessInput(InputHandler Input) { }

        //Render to the screen
        public override void Render(Graphics graphics)
        {
            if (Fading || Faded)
            {
                FadeScreen.Color = ColorPicker.FromArgb(FadeAlpha, Color);
                FadeScreen.Render(graphics);
            }
        }//Render()

        public void FadeOut(double time)
        {
            if (!Fading && !Faded)
            {
                FadeTimer.Reset();

                FadeTime = time;
                Fading = true;
            }
        }//FadeOut(double)

        public void FadeIn(double time)
        {
            if (!Fading && Faded)
            {
                FadeTimer.Reset();

                FadeTime = time;
                Fading = true;
            }
        }//FadeTime(double)

        public void FadeOutAndIn(double time)
        {
            if (!Fading && !Faded)
            {
                FadeTimer.Reset();

                FadeTime = time / 2;
                Fading = true;
                AutoFadeBack = true;
            }
        }//FadeOutAndIn(double)

        public void Pulse(double time)
        {
            if (!Pulsing)
            {
                Pulsing = true;
                FadeOutAndIn(time);
            }
            else
                Pulsing = false;
        }

    }//class Fader

}
