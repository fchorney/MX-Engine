//using System;
//using System.Collections.Generic;
//using System.Text;
//using mxGameLib;
//using MX;

//namespace mxGameLib
//{

//    public class Odometer : Module
//    {
//        enum OdometerState
//        {
//            Ready,
//            Rolling,
//            Pause
//        }

//        //Properties
//        Sprite Numbers;
//        Rectangle[] Digit = new Rectangle[4];
//        bool[] Rolling = new bool[4];
//        OdometerState State;

//        double RollSpeed;
//        double ActualValue;
//        int CurrentValue;
//        int TargetValue;
//        int PrevValue;

//        int dW, dH, bW, bH;

//        //Constuctor
//        public Odometer(string n) : base(n) { }
//        public Odometer(string n, bool running) : base(n, running) { }

//        //Initialiser
//        public override void Init()
//        {

//            RollSpeed = 0.2;

//            TargetValue = 9999;
//            CurrentValue = 0;

//            PrevValue = CurrentValue;
//            ActualValue = CurrentValue;

//            dW = 16;
//            dH = 16;
//            bW = 2;
//            bH = 2;

//            Numbers = new Sprite("Images\\numbers.png", 32, 256);

//            for (int i = 0; i < Digit.Length; i++)
//            {
//                Digit[i] = new Rectangle(0, 0, dW + bW, dH + bH);
//                Digit[i].Y = dH * GetDigit(CurrentValue, i);
//            }

//            State = OdometerState.Ready;
//        }

//        //Frame logic
//        public override void Update(double ElapsedTime)
//        {
//            switch (State)
//            {
//                case OdometerState.Ready:
//                    break;

//                case OdometerState.Pause:
//                    break;

//                case OdometerState.Rolling:

//                    int D = (CurrentValue > TargetValue ? 1 : -1);

//                    ActualValue -= ElapsedTime / RollSpeed * D;

//                    int n = (int)(ActualValue + (D < 0 || ActualValue % 1 == 0 ? 0 : 1));

//                    if (CurrentValue != n)
//                    {
//                        PrevValue = CurrentValue;
//                        CurrentValue = n;
//                    }

//                    if ((ActualValue <= TargetValue && D > 0) || (ActualValue >= TargetValue && D < 0))
//                    {
//                        ActualValue = TargetValue;
//                        CurrentValue = TargetValue;
//                        State = OdometerState.Ready;

//                        for (int i = 0; i < Digit.Length; i++)
//                            Digit[i].Y = dH * GetDigit(CurrentValue, i);
//                    }

//                    if (CurrentValue != TargetValue)
//                    {
//                        Roll(0, D, ElapsedTime);
//                    }

//                    for (int i = 1; i < Digit.Length; i++)
//                    {
//                        if (Rolling[i-1] && 
//                            ((D > 0 && GetDigit(CurrentValue, i - 1) == 0) ||
//                            (D < 0 && GetDigit(CurrentValue, i - 1) == 9)))
//                        {
//                            Roll(i, D, ElapsedTime);
//                        }
//                        else
//                        {
//                            Rolling[i] = false;
//                            Digit[i].Y = dH * GetDigit(CurrentValue, i);
//                        }
//                    }

//                    break;

//            }

//        }

//        private void Roll(int i, int D, double ElapsedTime)
//        {
//            Rolling[i] = true;
//            Digit[i].Y = dH * (GetDigit(ActualValue,i) + (float)(ActualValue % 1));
//        }

//        private int GetDigit(double n, int decimalPlace)
//        {
//            for (int i = 0; i < decimalPlace; i++)
//                n /= 10;
//            return (int)n % 10;
//        }

//        public override void ProcessInput(InputHandler Input)
//        {
//            if (Input.Keyboard.KeyPressed(Keys.Up))
//            {
//                State = OdometerState.Rolling;
//            }
//            if (Input.Keyboard.KeyPressed(Keys.Down))
//            {
//                State = OdometerState.Pause;
//            }

//            if (Input.Keyboard.KeyPressed(Keys.Left))
//            {
//                TargetValue -= 10;
//            }
//            if (Input.Keyboard.KeyPressed(Keys.Right))
//            {
//                TargetValue += 10;
//            }

//            if (Input.Keyboard.KeyPressed(Keys.Comma))
//            {
//                RollSpeed *= 1.5;
//            }
//            if (Input.Keyboard.KeyPressed(Keys.Period))
//            {
//                RollSpeed /= 1.5;
//            }
//        }

//        //Render to the screen
//        public override void Render(Graphics g)
//        {
//            Numbers.Frames[0] = new System.Drawing.Rectangle((int)Digit[3].X, (int)Digit[3].Y, (int)Digit[3].W, (int)Digit[3].H);
//            Numbers.Position.Set(50, 50);
//            Numbers.Render(g);

//            Numbers.Frames[0] = new System.Drawing.Rectangle((int)Digit[2].X, (int)Digit[2].Y, (int)Digit[2].W, (int)Digit[2].H);
//            Numbers.Position.Set(50 + 16, 50);
//            Numbers.Render(g);

//            Numbers.Frames[0] = new System.Drawing.Rectangle((int)Digit[1].X, (int)Digit[1].Y, (int)Digit[1].W, (int)Digit[1].H);
//            Numbers.Position.Set(50 + 32, 50);
//            Numbers.Render(g);

//            Numbers.Frames[0] = new System.Drawing.Rectangle((int)Digit[0].X, (int)Digit[0].Y, (int)Digit[0].W, (int)Digit[0].H);
//            Numbers.Position.Set(50 + 48, 50);
//            Numbers.Render(g);


//            //g.DrawText(Numbers.Frames[0].ToString(), 0, 0);
//            g.DrawText("Actual:  " + ActualValue.ToString(), 0, 0);
//            g.DrawText("Current: " + CurrentValue.ToString(), 0, 20);
//            g.DrawText("Target:  " + TargetValue.ToString(), 0, 40);
//        }

//    }

//}
