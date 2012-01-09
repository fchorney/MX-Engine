using System;
using System.Drawing;
using System.Collections.Generic;

namespace MX
{

    public class ParticleEffect
    {
        List<Particle> _Particles;

        public ParticleEffect()
        {
            _Particles = new List<Particle>();
        }

        public virtual void Update(double ElapsedTime)
        {
            for (int i = 0; i < _Particles.Count; i++)
            {
                _Particles[i].Update(ElapsedTime);

                if (_Particles[i].TimeToLive > 0 && _Particles[i].Age > _Particles[i].TimeToLive)
                {
                    _Particles.RemoveAt(i--); //TODO: this is probably dangerous.. find a better way?
                }
            }
        }

        public virtual void Render(Graphics g)
        {
            for (int i = 0; i < _Particles.Count; i++)
            {
                _Particles[i].Render(g);
            }
        }

        public virtual void Add(Particle p)
        {
            _Particles.Add(p);
        }

        public virtual void Start() { }
        public virtual void Stop() { }

    }

}