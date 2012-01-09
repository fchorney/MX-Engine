using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;

namespace MX
{
    #region Input

    public class InputHandler
    {

        public bool HasFocus
        {
            get
            {
                return InputState.GetInstance.CheckFocus(this);
            }
        }

        public void TakeFocus()
        {
            InputState.GetInstance.SetFocus(this);
        }

        public InputHandler() { }

        public Mouse Mouse { get { return InputState.GetInstance.GetMouseState(this); } }
        public Keyboard Keyboard { get { return InputState.GetInstance.GetKeyboardState(this); } }
    }

    #endregion

    #region Input State

    internal class InputState : IDisposable
    {
        public InputHandler FocusControl;

        private static InputState _InputState;
        public static InputState GetInstance { get { return _InputState; } }

        public static void Init(System.Windows.Forms.Control targetControl)
        {
            Mouse.Init(targetControl);
            Keyboard.Init(targetControl);
            _InputState = new InputState();
        }

        public void SetFocus(InputHandler input)
        {
            FocusControl = input;
        }

        public bool CheckFocus(InputHandler input)
        {
            return (FocusControl == input);
        }

        public Mouse GetMouseState(InputHandler input)
        {

            if (FocusControl == input || true)
                return Mouse.GetInstance;
            else
                return Mouse.EmptyState;
        }

        public Keyboard GetKeyboardState(InputHandler input)
        {
            if (FocusControl == input || true)
                return Keyboard.GetInstance;
            else
                return Keyboard.EmptyState;
        }

        public void Poll()
        {
            Mouse.GetInstance.Poll();
            Keyboard.GetInstance.Poll();
        }

        public void Dispose()
        {
            Mouse.GetInstance.Dispose();
            Keyboard.GetInstance.Dispose();
        }

    }

    #endregion

    #region Mouse

    public class Mouse : IDisposable
    {
        private static MouseState CurrentMouseState;
        private static byte[] ButtonState;
        private static bool[] ButtonsPressed = new bool[10];
        private static bool[] ButtonsHeld = new bool[10];

        private static Device _Device;
        public  static Device Device {get { return _Device; }}

        private static Mouse _Mouse;
        internal static Mouse GetInstance { get { return _Mouse; } }

        private static Mouse _EmptyState;
        internal static Mouse EmptyState { get { return _EmptyState; } }

		private Mouse() {}

        private static int _x, _y;
        private static bool LockMouse;

        public int dX { get { return CurrentMouseState.X; } }
        public int dY { get { return CurrentMouseState.Y; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public int Z { get { return CurrentMouseState.Z; } }
        public bool BtnHeld(Buttons btn) { return ButtonsHeld[(int)btn]; }
        public bool BtnPressed(Buttons btn) { return ButtonsPressed[(int)btn]; }

        public static void Init(System.Windows.Forms.Control targetControl)
        {
            
            if (_Mouse == null)
                _Mouse = new Mouse();
            if (_EmptyState == null)
                _EmptyState = new Mouse();
            _Device = new Device(SystemGuid.Mouse);
            _Device.SetCooperativeLevel(targetControl, CooperativeLevelFlags.Foreground | CooperativeLevelFlags.NonExclusive);
            _Device.SetDataFormat(DeviceDataFormat.Mouse);
            targetControl.MouseMove += new MouseEventHandler(targetControl_MouseMove);

            try { _Device.Acquire(); }
            catch (DirectXException e) { Console.WriteLine(e.Message); }
        }

        static void targetControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!LockMouse)
            {
                _x = e.X;
                _y = e.Y;
                LockMouse = true;
            }
            
        }

        public void Poll()
        {
            LockMouse = false;
            try
            {
                CurrentMouseState = Device.CurrentMouseState;
                ButtonState = CurrentMouseState.GetMouseButtons();

                for (int i = 0; i < ButtonState.Length; i++)
                {
                    if (ButtonsHeld[i])
                        ButtonsPressed[i] = false;
                    else
                        ButtonsPressed[i] = (ButtonState[i] & 128) > 0;

                    ButtonsHeld[i] = (ButtonState[i] & 128) > 0;
                }
            }
            catch (NotAcquiredException)
            {
                try { _Device.Acquire(); }
                catch (Exception) { }
            }
            catch (InputException) { }
            catch (NullReferenceException) { }
        }

        public void Dispose()
        {
            if (_Device != null)
            {
                _Device.Unacquire();
                _Device.Dispose();
                _Device = null;
            }
        }

    }

    #endregion

    #region Keyboard

    public class Keyboard : IDisposable
	{
        private Key[] KeyState;
        private bool[] PrevKeyState = new bool[256];
        private bool[] KeysPressed = new bool[256];
        private bool[] KeysHeld    = new bool[256];
        private bool[] KeysReleased = new bool[256];
        private double[] KeysElapsedTime = new double[256];
        private double[] KeysPrevElapsedTime = new double[256];

        public bool KeyHeld(Keys key) { return KeysHeld[(int)key]; }
        public bool KeyPressed(Keys key) { return KeysPressed[(int)key]; }
        public bool KeyReleased(Keys key) { return KeysReleased[(int)key]; }
        public double KeyElapsedTime(Keys key) { return KeysElapsedTime[(int)key] - KeysPrevElapsedTime[(int)key]; }

        private static Timer Timer;
        private static Device _Device;
        public  static Device Device {get { return _Device; }}

        private static Keyboard _Keyboard;
        internal static Keyboard GetInstance { get { return _Keyboard; } }

        private static Keyboard _EmptyState;
        internal static Keyboard EmptyState { get { return _EmptyState; } }

		private Keyboard() {}

        public static void Init(System.Windows.Forms.Control targetControl)
        {
 
            if (_Keyboard == null)
                _Keyboard = new Keyboard();
            if (_EmptyState == null)
                _EmptyState = new Keyboard();
            _Device = new Device(SystemGuid.Keyboard);
            _Device.SetCooperativeLevel(targetControl, CooperativeLevelFlags.Foreground | CooperativeLevelFlags.NonExclusive);
            _Device.SetDataFormat(DeviceDataFormat.Keyboard);

            Timer = new Timer();
            double InitTime = Timer.GetAbsoluteTime();
            for (int i = 0; i < _Keyboard.KeysElapsedTime.Length; i++)
                _Keyboard.KeysElapsedTime[i] = InitTime;

            try {_Device.Acquire();}
            catch (DirectXException e) { Console.WriteLine(e.Message); }
        }

        public void Poll()
        {
            try
            {
                _Device.Poll();

                for (int i = 0; i < KeysReleased.Length; i++)
                    PrevKeyState[i] = KeysHeld[i];

                KeyState = _Device.GetPressedKeys();

                for (int i = 0; i < KeysPressed.Length; i++)
                    KeysPressed[i] = false;

                for (int i = 0; i < KeyState.Length; i++)
                    if (!KeysHeld[(int)KeyState[i]])
                    {
                        KeysPressed[(int)KeyState[i]] = true;
                        KeysPrevElapsedTime[(int)KeyState[i]] = KeysElapsedTime[(int)KeyState[i]];
                        KeysElapsedTime[(int)KeyState[i]] = Timer.GetAbsoluteTime();
                    }

                for (int i = 0; i < KeysHeld.Length; i++)
                    KeysHeld[i] = false;

                for (int i = 0; i < KeyState.Length; i++)
                    KeysHeld[(int)KeyState[i]] = true;

                for (int i = 0; i < KeysReleased.Length; i++)
                    KeysReleased[i] = PrevKeyState[i] && !KeysHeld[i];

            }

            catch (NotAcquiredException)
            {
                try { _Device.Acquire(); }
                catch (Exception) { }
            }
            catch (InputException) { }
            catch (NullReferenceException) { }

        }

        public void Dispose()
        {
            if (_Device != null)
            {
                _Device.Unacquire();
                _Device.Dispose();
                _Device = null;
            }
        }
    }

    #endregion

    #region Enumerations

    public enum FocusMode
    {
        Exclusive,
        Background
    }

    public enum Buttons
    {
        Left,
        Right,
        Middle
    }

    public enum Keys
    {
        Escape = 1,
        D1 = 2,
        D2 = 3,
        D3 = 4,
        D4 = 5,
        D5 = 6,
        D6 = 7,
        D7 = 8,
        D8 = 9,
        D9 = 10,
        D0 = 11,
        Minus = 12,
        Equals = 13,
        BackSpace = 14,
        Tab = 15,
        Q = 16,
        W = 17,
        E = 18,
        R = 19,
        T = 20,
        Y = 21,
        U = 22,
        I = 23,
        O = 24,
        P = 25,
        LeftBracket = 26,
        RightBracket = 27,
        Return = 28,
        LeftControl = 29,
        A = 30,
        S = 31,
        D = 32,
        F = 33,
        G = 34,
        H = 35,
        J = 36,
        K = 37,
        L = 38,
        SemiColon = 39,
        Apostrophe = 40,
        Grave = 41,
        LeftShift = 42,
        BackSlash = 43,
        Z = 44,
        X = 45,
        C = 46,
        V = 47,
        B = 48,
        N = 49,
        M = 50,
        Comma = 51,
        Period = 52,
        Slash = 53,
        RightShift = 54,
        NumPadStar = 55,
        LeftMenu = 56,
        Space = 57,
        CapsLock = 58,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        Numlock = 69,
        Scroll = 70,
        NumPad7 = 71,
        NumPad8 = 72,
        NumPad9 = 73,
        NumPadMinus = 74,
        NumPad4 = 75,
        NumPad5 = 76,
        NumPad6 = 77,
        Add = 78,
        NumPad1 = 79,
        NumPad2 = 80,
        NumPad3 = 81,
        NumPad0 = 82,
        Decimal = 83,
        OEM102 = 86,
        F11 = 87,
        F12 = 88,
        F13 = 100,
        F14 = 101,
        F15 = 102,
        Kana = 112,
        AbntC1 = 115,
        Convert = 121,
        NoConvert = 123,
        Yen = 125,
        AbntC2 = 126,
        NumPadEquals = 141,
        Circumflex = 144,
        At = 145,
        Colon = 146,
        Underline = 147,
        Kanji = 148,
        Stop = 149,
        AX = 150,
        Unlabeled = 151,
        NextTrack = 153,
        NumPadEnter = 156,
        RightControl = 157,
        Mute = 160,
        Calculator = 161,
        PlayPause = 162,
        MediaStop = 164,
        VolumeDown = 174,
        VolumeUp = 176,
        WebHome = 178,
        NumPadComma = 179,
        NumPadSlash = 181,
        Down = DownArrow,
        SysRq = 183,
        RightMenu = 184,
        Pause = 197,
        Home = 199,
        Up = 200,
        PageUp = 201,
        Left = LeftArrow,
        LeftArrow = 203,
        Right = 205,
        End = 207,
        DownArrow = 208,
        PageDown = 209,
        Insert = 210,
        Delete = 211,
        LeftWindows = 219,
        RightWindows = 220,
        Apps = 221,
        Power = 222,
        Sleep = 223
    }

    #endregion

}