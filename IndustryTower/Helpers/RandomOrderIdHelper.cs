using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.Helpers
{
    public class RandomOrderIdHelper
    {
        public long Generate()
        {
            Random rand = new Random();
            return LongRandom(1000000, long.MaxValue, rand);
        }

        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}