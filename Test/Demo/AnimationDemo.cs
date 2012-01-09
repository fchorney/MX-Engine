using System;
using System.Collections.Generic;
using System.Text;
using MX;

    public class AnimationDemo : Module
    {
        PhysicsEngine Physics = new PhysicsEngine();
        VectorForce gravity = new VectorForce(0, 2000);
        Ground ground = new Ground(0, 400, 10000, 10000);

        public Animation animTest;
        public CollidableSprite sprite;

        public AnimationDemo(string n) : base(n) { }
        public AnimationDemo(string n, bool running) : base(n, running) { }

        public override void Init()
        {
            animTest = AnimationFactory.GenerateAnimation("Images//Ness2.png", 128, 128, 5, 0.15);
            sprite = new CollidableSprite();
            sprite.AddAnimation(animTest);

            Physics.Add(gravity);
            Physics.Add(ground);
            Physics.Add(sprite);

        }

        public override void Update(double ElapsedTime)
        {

            animTest.Update(ElapsedTime);
            Physics.Update(ElapsedTime);

        }

        public override void ProcessInput(InputHandler Input)
        {
            if (Input.Keyboard.KeyHeld(Keys.Right))
                sprite.Velocity.X = 500;
            if (Input.Keyboard.KeyHeld(Keys.Left))
                sprite.Velocity.X = -500;
            if (Input.Keyboard.KeyPressed(Keys.Up))
                sprite.Velocity.Y = -800;
            if (Input.Keyboard.KeyPressed(Keys.Down))
            {
                sprite.Forces.Add(new VectorForce(50000, 0));
            }

            if (Input.Keyboard.KeyPressed(Keys.Space))
            {
                sprite.X = 0;
                sprite.Y = 0;
            }

            if (Input.Keyboard.KeyHeld(Keys.PageUp))
                ground.Y -= 100 * (float)GameEnvironment.ElapsedTime;
            if (Input.Keyboard.KeyHeld(Keys.PageDown))
                ground.Y += 100 * (float)GameEnvironment.ElapsedTime;
        }

        public override void Render(Graphics g)
        {
            sprite.Render(g);
            ground.Render(g);
            Quad q = new Quad(sprite.Shape as Rectangle);
            q.Render(g);

            g.DrawText("Sprite: [" + sprite.X.ToString("0.0") + ", " + sprite.Y.ToString("0.0") + "]", 10, 40);
            g.DrawText("Shape: [" + sprite.Shape.X.ToString("0.0") + ", " + sprite.Shape.Y.ToString("0.0") + "]", 10, 60);
            g.DrawText("Velocity: [" + sprite.Velocity.X.ToString("00000.0") + ", " + sprite.Velocity.Y.ToString("00000.0") + "]", 10, 80);
            g.DrawText("Acceleration: [" + sprite.Acceleration.X.ToString("00000.0") + ", " + sprite.Acceleration.Y.ToString("00000.0") + "]", 10, 100);

            Physics.Render(g);
        }

    }
