using System;
using System.Collections.Generic;

namespace MX
{

    public class Intersect
    {

        public static bool EverywhereToShape(Everywhere a, IShape b)
        {
            return true;
        }

        public static bool TopToShape(Top a, IShape b)
        {
            return b.Y < a.Y;
        }

        public static bool BottomToShape(Bottom a, IShape b)
        {
            return b.Y + b.H > a.Y;
        }

        public static bool LeftToShape(Top a, IShape b)
        {
            return b.X < a.X;
        }

        public static bool RightToShape(Top a, IShape b)
        {
            return b.X + b.W > a.X;
        }

        public static bool CircleToCircle(Circle a, Circle b)
        {

            double c2 = (a.cX - b.cX) * (a.cX - b.cX) + (a.cY - b.cY) * (a.cY - b.cY);
            double r2 = (a.R + b.R) * (a.R + b.R);

            return c2 <= r2;
        }

        public static bool CircleToRectangle(Circle a, Rectangle b)
        {
            double dmin = 0;
            double[] C = new double[] { a.cX, a.cY };
            double[] Bmin = new double[] { b.X, b.Y };
            double[] Bmax = new double[] { b.X + b.W, b.Y + b.H };
            for (int i = 0; i < C.Length; i++)
            {
                if (C[i] < Bmin[i]) dmin += (C[i] - Bmin[i]) * (C[i] - Bmin[i]);
                else if (C[i] > Bmax[i]) dmin += (C[i] - Bmax[i]) * (C[i] - Bmax[i]);
            }
            return dmin <= a.R * a.R;
        }

        public static bool RectangleToRectangle(Rectangle a, Rectangle b)
        {
            if (b == null) return false;
            return MathUtil.CheckAxis(a.X, a.X + a.W, b.X, b.X + b.W) &&
                   MathUtil.CheckAxis(a.Y, a.Y + a.H, b.Y, b.Y + b.H);
        }
    }

}
