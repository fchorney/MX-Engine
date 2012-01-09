//using System;
//using System.Collections.Generic;
//using System.Text;
//using mxGameLib;
//using mxGameLib.Util;
//using MX;

//namespace mxGameLib
//{

//    public class Test : Module
//    {
//        //Properties
//        Sprite circle;
//        Circle c1, c2;
//        Rectangle r1;

//        //Constuctor
//        public Test(string n) : base(n) { }
//        public Test(string n, bool running) : base(n, running) { }

//        //Initialiser
//        public override void Init()
//        {
//            circle = new Sprite("Images\\circle.png", 128, 128);
//            c1 = new Circle(240, 160, 32);
//            c2 = new Circle(160, 224, 128);
//            r1 = new Rectangle(100, 100, 100, 100);
//        }

//        //Frame logic
//        public override void Update(double ElapsedTime)
//        {
//        }//Update()

//        public override void ProcessInput(InputHandler Input)
//        {

//            if (Input.Keyboard.KeyHeld(Keys.Left))
//            {
//                c2.X -= 50 * (float)GameEnvironment.ElapsedTime;
//            }
//            if (Input.Keyboard.KeyHeld(Keys.Right))
//            {
//                c2.X += 50 * (float)GameEnvironment.ElapsedTime;
//            }
//            if (Input.Keyboard.KeyHeld(Keys.Up))
//            {
//                c2.Y -= 50 * (float)GameEnvironment.ElapsedTime;
//            }
//            if (Input.Keyboard.KeyHeld(Keys.Down))
//            {
//                c2.Y += 50 * (float)GameEnvironment.ElapsedTime;
//            }
//            if (Input.Keyboard.KeyHeld(Keys.Z))
//            {
//                c2.R += 50 * (float)GameEnvironment.ElapsedTime;
//            }
//            if (Input.Keyboard.KeyHeld(Keys.X))
//            {
//                c2.R -= 50 * (float)GameEnvironment.ElapsedTime;
//            }
//        }

//        //Render to the screen
//        public override void Render(Graphics g)
//        {
//            Quad rect = new Quad(r1);
//            if (r1.Intersects(c2))
//                rect.Color = ColorPicker.ColorFromRGB(100, 255, 100);
//            else
//                rect.Color = ColorPicker.ColorFromRGB(255, 0, 0);
//            rect.Render(g);

//            float d = c1.R * 2;
//            circle.Size.Set(d, d);
//            circle.Position.Set(c1.X, c1.Y);
//            if (c1.Intersects(c2))
//                circle.Color = ColorPicker.ColorFromRGB(100, 255, 100);
//            else
//                circle.Color = ColorPicker.ColorFromRGB(255, 255, 255);
//            circle.Render(g);

//            d = c2.R * 2;
//            circle.Size.Set(d, d);
//            circle.Position.Set(c2.X, c2.Y);
//            if (c2.Intersects(c1) || c2.Intersects(r1))
//                circle.Color = ColorPicker.ColorFromRGB(100, 255, 100);
//            else
//                circle.Color = ColorPicker.ColorFromRGB(255, 255, 255);
//            circle.Render(g);

//            g.DrawText("Circle 1:" + c1.X, (int)GameEnvironment.Camera.X + 200, 0);
//            g.DrawText("Circle 2:" + c2.X, (int)GameEnvironment.Camera.X + 200, 20);
//        }//Render()

//    }//class Template

//}
