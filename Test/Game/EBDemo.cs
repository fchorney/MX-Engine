//using System;
//using System.Collections.Generic;
//using System.Text;
//using mxGameLib;
//using mxGameLib.Util;
//using MX;

//namespace EB
//{

//    public class EBDemo : Module
//    {
//        //Properties
//        Rectangle bb = new Rectangle(45, 27, 40, 70);
//        Rectangle bb2 = new Rectangle(45, 27, 40, 70);
//        PhysicsEngine Physics = new PhysicsEngine();
//        VectorForce gravity = new VectorForce(0,2000);
//        bool physicsOn = true;
//        Vector2D Camera = GameEnvironment.Camera;

//        EBSprite ness;
//        EBSprite bob;

//        Ground ground;

//        int speed = 20;

//        //Constuctor
//        public EBDemo(string n) : base(n) { }
//        public EBDemo(string n, bool running) : base(n, running) { }

//        //Initialiser
//        public override void Init()
//        {
//            AnimatedSprite nessSprite = new AnimatedSprite("Images\\Ness2.png", 128, 128, new int[] {1, 10, 5 ,1, 10 , 5 , 10, 6, 9});

//            nessSprite.AnimSpeed = 0.15;

//            ness = new EBSprite(0, 0, 128, 128, nessSprite);
//            ness.name = "Ness";

//            nessSprite = new AnimatedSprite("Images\\Ness2.png", 128, 128, new int[] { 1, 10, 5, 1, 10, 5, 10, 6, 9 });

//            bob = new EBSprite(0, 0, 128, 128, nessSprite);
//            bob.X = 500;
//            bob.name = "Bob";

//            ness.SetBoundingBox(bb);

//            bob.SetBoundingBox(bb2);

//            ground = new Ground(-300,400,900,500, new Sprite("Images\\ground.png",128,128));
//            ground.Friction = 1200;

//            Physics.Add(bob);
//            Physics.Add(ness);
//            Physics.Add(gravity);
//            Physics.Add(ground);

//        }

//        //Frame logic
//        public override void Update(double ElapsedTime)
//        {
//            bob.Update(ElapsedTime);

//            ness.Update(ElapsedTime);

//            if(physicsOn) Physics.Update(ElapsedTime);

//            Camera.X = (ness.X - 250);

//        }//Update()

//        public override void ProcessInput(InputHandler Input)
//        {
//            if (!Input.Keyboard.KeyHeld(Keys.Left) && !Input.Keyboard.KeyHeld(Keys.Right))
//            {
//                ness.State = SpriteState.Standing;
//            }

//            if (Input.Keyboard.KeyPressed(Keys.Left))
//            {
//                if (Input.Keyboard.KeyElapsedTime(Keys.Left) < 0.3)
//                    ness.State = SpriteState.Running;
//                else
//                    ness.State = SpriteState.Walking;
//                ness.Direction = SpriteDirection.Left;
//            }

//            if (Input.Keyboard.KeyPressed(Keys.Right))
//            {
//                if (Input.Keyboard.KeyElapsedTime(Keys.Right) < 0.3)
//                    ness.State = SpriteState.Running;
//                else
//                    ness.State = SpriteState.Walking;
//                ness.Direction = SpriteDirection.Right;
//            }

//            if (Input.Keyboard.KeyReleased(Keys.Left) && Input.Keyboard.KeyHeld(Keys.Right))
//            {
//                ness.Direction = SpriteDirection.Right;
//            }

//            if (Input.Keyboard.KeyReleased(Keys.Right) && Input.Keyboard.KeyHeld(Keys.Left))
//            {
//                ness.Direction = SpriteDirection.Left;
//            }

//            if (Input.Keyboard.KeyHeld(Keys.C))
//            {
//                ness.ChangeAnimation(6);
//            }

//            if (Input.Keyboard.KeyHeld(Keys.V))
//            {
//                ness.ChangeAnimation(8);
//            }

//            if (Input.Keyboard.KeyPressed(Keys.Space))
//            {
//                ness.Velocity.Y = -800;
//            }

//            if (Input.Keyboard.KeyPressed(Keys.Up))
//            {
//                ness.Velocity.Y = -800;
//            }

//            if (Input.Keyboard.KeyHeld(Keys.Insert))
//            {
//                GameEnvironment.Camera.X += speed * (float)GameEnvironment.ElapsedTime;
//            }

//            if (Input.Keyboard.KeyHeld(Keys.Home))
//            {
//                GameEnvironment.Camera.X -= speed * (float)GameEnvironment.ElapsedTime;
//            }

//            if (Input.Keyboard.KeyPressed(Keys.LeftControl))
//            {
//                physicsOn = !physicsOn;
//            }

//        }

//        //Render to the screen
//        public override void Render(Graphics g)
//        {
//            //Physics.Render(g);

//            ness.Render(g);
//            bob.Render(g);

//            g.DrawText("[" + ness.X + ", " + ness.Y + "]", (int)GameEnvironment.Camera.X, 20);
//            g.DrawText(ness.Velocity.Y.ToString(), (int)GameEnvironment.Camera.X, 40);
//            g.DrawText(ness.State.ToString(), (int)GameEnvironment.Camera.X, 60);

//            ground.Render(g);

//        }//Render()

//    }//class Template

//}
