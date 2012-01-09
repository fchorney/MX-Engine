using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MX
{
    public class Limiter
    {
        double interval;
        double elapsedTime;
        public Limiter(double interval)
        {
            this.interval = interval;
            elapsedTime = double.MaxValue;
        }

        public void Update(double ElapsedTime)
        {
            this.elapsedTime += ElapsedTime;
        }

        public bool Ready
        {
            get
            {
                bool result = elapsedTime >= interval;
                if (result) elapsedTime = 0;
                return result;
            }
        }
    }

}
