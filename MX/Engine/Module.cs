using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MX 
{

    public interface IModule : IComparable<IModule>
    {
        string ID { get; }
        void Start();
        void Stop();
        void Init();
        void Render(Graphics graphics);
        void RenderAll(Graphics graphics);
        void Update(double ElapsedTime);
        void UpdateAll(double ElapsedTime);
        void ProcessInput(InputHandler Input);
        InputHandler InputHandler { get; }
        void Dispose();
        void DisposeAll();
        bool HasActiveModules();
        IModule GetModule(IModule module);
        IModule GetModule(string module);
        IModule Parent { get; set; }
        int  Priority    { get; set; }
        bool Initialized { get; set; }
        bool JustStarted { get; set; }
        bool Running     { get; set; }
    }

	public abstract class Module :IModule
    {

        #region Variables

        protected List<IModule> Modules;
        public double RunningTime;
		protected string _id;
        protected IModule _parent;
        protected int _priority;
        protected bool _initialized;
        protected bool  _justStarted;
        protected bool  _running;
        protected bool HasFocus = true;
        protected InputHandler _InputHandler;

        #endregion

        #region Properties

        public string ID 
        { 
            get { return _id; } 
        }

        public IModule Parent 
        { 
            get { return _parent; } 
            set { _parent = value; } 
        }

        public int Priority { 
            get { return _priority; } 
            set { _priority = value; }
        }

        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        public bool JustStarted
        {
            get { return _justStarted; }
            set { _justStarted = value; }
        }

        public bool Running
        {
            get { return _running; }
            set
            {
                if (value)
                    Start();
                else
                    Stop();
            }
        }

        public InputHandler InputHandler
        {
            get { return _InputHandler; }
        }


        #endregion

        #region Abstract Functions

        public abstract void Init();
        public abstract void ProcessInput(InputHandler Input);
        public abstract void Update(double ElapsedTime);
        public abstract void Render(Graphics graphics);
        public virtual  void Dispose() { }

    #endregion

        #region Internal Module Functions

        public virtual void UpdateAll(double ElapsedTime)
        {
            
            foreach (IModule state in Modules)
            {
                if (!state.JustStarted)
                {
                    if (state == this)
                    {
                        //GuiManager.Update(ElapsedTime);
                        //if(!GuiManager.HasFocus)
                        state.ProcessInput(InputHandler);
                        
                        state.Update(ElapsedTime);

                    }
                    else if (state.Running)
                        state.UpdateAll(ElapsedTime);
                }
                else if(Running) 
                    state.JustStarted = false;
            }
            
        }

        public virtual void RenderAll(Graphics g)
        {
            foreach (IModule state in Modules)
                if (state == this)
                {
                    state.Render(g);
                    //GuiManager.Render(g);
                }
                else if (state.Running)
                    state.RenderAll(g);
        }

        public virtual void DisposeAll()
        {
            foreach (IModule state in Modules)
                if (state == this)
                    state.Dispose();
                else if (state.Running)
                    state.DisposeAll();
        }

        public bool HasActiveModules()
        {
            foreach (IModule child in Modules)
                if (child.Running && child != this)
                    return true;
            return false;
        }

        public void Start()
        {
            if (!_running)
            {
                if (!Initialized)
                {
                    Init();
                    Initialized = true;
                }

                _running = true;
                _justStarted = true;

            }

        }

        public void Stop()
        {
            _running = false;
        }

        public void AddModule(IModule module)
        {
            if (!module.Initialized ) module.Init();
            module.Parent = this;
            Modules.Add(module);
            Modules.Sort();
        }

        public IModule GetModule(IModule module)
        {
            if (Modules.Contains(module))
                return Modules[Modules.IndexOf(module)];
            return null;
        }

        public IModule GetModule(string id)
        {
            foreach (IModule state in Modules)
                if (state.ID.Equals(id))
                    return state;
            return null;
        }

        public virtual int CompareTo(IModule module)
        {
            return Priority - module.Priority;
        }

        #endregion

        #region Constructors

        public Module()
		{
            Modules = new List<IModule>();
            _InputHandler = new InputHandler();

            this.Priority = 0;
            Modules.Add(this);

		}

		public Module(string n) : this()
		{
			_id = n;
		}

        public Module(string n, bool running) : this(n)
        {
            Running = running;
        }

        #endregion

    }

}//namespace MX