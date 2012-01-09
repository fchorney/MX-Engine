using System;
using Microsoft.DirectX.DirectInput;
using mxGameLib;

namespace mxModules
{

    public class Template : mxState
    {
        //Properties

        //Constuctor
        public Template(string n) : base(n) { }

        //Initialiser
        public override void Init()
        {
        }

        //Frame logic
        public override void Update(double ElapsedTime)
        {

        }//Run()

        public void CheckInput()
        {
            if (K.KeyPressed(Key.Return))
            {
            }
        }

        //Render to the screen
        public override void Render()
        {
        }//Render()

    }//class ModuleHandler

}