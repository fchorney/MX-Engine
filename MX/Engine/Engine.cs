using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MX;

namespace MX
{

	public sealed class Engine 
    {
        private static Engine _Engine;
        private IModule  _StartingModule;
        private Timer _Timer;
        private Control _ViewForm;
        private static double _ElapsedTime;
        private bool _Running;
        private const double _MinElapsedTime = 0.0333333;
        private const double _MaxElapsedTime = 0.0166667;
        private double _FrameElapsedTime;

        internal double ElapsedTime
        {
            get { return _FrameElapsedTime; }
        }

        public static Engine GetInstance
        {
            get { 
                if (_Engine == null) 
                    _Engine = new Engine();
                return _Engine;
            }
        }

        private Engine() { }

        public void Init(int width, int height)
        {
            _ViewForm = new ViewForm();

            InputState.Init(_ViewForm);
            Sound.Init(_ViewForm);
            Graphics.Init(_ViewForm, width, height);

            _ViewForm.Show();

            _Running = true;
            _Timer = new Timer();

        }

        public void Start(IModule startingModule, int width, int height)
        {
            
            Init(width, height);

            _StartingModule = startingModule;
            _StartingModule.Start();

            while (_Running && _StartingModule.Running)
            {
                
             
                _ElapsedTime = _Timer.GetElapsedTime();
                _FrameElapsedTime += _ElapsedTime;

                if (_FrameElapsedTime > _MaxElapsedTime)
                {

                    InputState.GetInstance.Poll();

                    if (_FrameElapsedTime > _MinElapsedTime)
                        _FrameElapsedTime = _MinElapsedTime;

                    _StartingModule.UpdateAll(_FrameElapsedTime);

                    if (Graphics.GetInstance != null)
                    {
                        Graphics.GetInstance.Clear();
                        Graphics.GetInstance.BeginScene();

                        _StartingModule.RenderAll(Graphics.GetInstance);

                        Graphics.GetInstance.EndScene();
                    }

                    _FrameElapsedTime = 0;
                }

                Application.DoEvents();
            }

            Dispose();
        }
		
        public void Dispose()
        {
            _Running = false;
            _StartingModule.DisposeAll();
            _ViewForm.Dispose();

            if (Graphics.GetInstance != null) Graphics.GetInstance.Dispose();
            if (Keyboard.GetInstance != null) Keyboard.GetInstance.Dispose();
            if (Mouse.GetInstance != null) Mouse.GetInstance.Dispose();
            if (Sound.GetInstance != null) Sound.GetInstance.Dispose();
        }

	}

}