using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSim
{
    static class RandomNumber
    {
        static System.Random rnd = new System.Random(DateTime.Now.Second);

        public static long Next()
        {
            return rnd.Next();
        }
    }
}
