using System;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;

namespace MX
{

	public class Sound : IDisposable
	{

        private static Sound S;
        private static BufferDescription _description;
        public  static BufferDescription Description { get { return _description; } }

        private static Device _device;
        public  static Device Device {get { return _device; }}

        internal static Sound GetInstance
        {
            get { return S; }
        }

        private Sound() { }

        public static void Init(System.Windows.Forms.Control targetControl)
        {
            if (S == null) S = new Sound();
            if (_device == null) _device = new Device();
            _device.SetCooperativeLevel(targetControl,CooperativeLevel.Priority);

            _description = new BufferDescription();
            _description.ControlVolume = true;
            _description.ControlFrequency = true;
        }

        public static void Play(string path)
        {
            try
            {
                SecondaryBuffer sound = new SecondaryBuffer(path, _description, _device);//= mxAudioCache.GetInstance.Get(path);
                if (sound != null) sound.Play(0, BufferPlayFlags.Default);
            }
            catch (Exception e) { System.Windows.Forms.MessageBox.Show("Invalid audio file!\n" + e); }
                
        }

        public void Dispose()
        {
            S = null;
            _device.Dispose();
            _device = null;
        }

    }
    /*
    class mxAudio : IDisposable
    {
        Audio audio;
        private bool _playing;
        public bool Playing
        {
            get { return _playing; }
        }

        public mxAudio(string path)
        {
            audio = new Audio(path);
            _playing = false;
        }

        public void Play()
        {
            audio.Play();
            _playing = true;
        }

        public void Stop()
        {
            audio.Stop();
            _playing = false;
        }

        public void Pause()
        {
            audio.Pause();
            _playing = false;
        }

        public void Seek(double time)
        {
            audio.SeekCurrentPosition(time,SeekPositionFlags.AbsolutePositioning);
        }

        public double CurrentPosition
        {
            get { return audio.CurrentPosition; }
            set { audio.CurrentPosition = value; }
        }

        public double Duration
        {
            get { return audio.Duration; }
        }

        public void Dispose()
        {
            if (audio.Playing)
                audio.Stop();
            audio.Dispose();
        }

    }//class mxAudio
    */
}