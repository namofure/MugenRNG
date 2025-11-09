using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugenRNG
{
    internal class MTSeed
    {
        private uint Seed;
        uint[] stateVector = new uint[624];
        protected const int N = 624;
        protected const int M = 397;
        protected const uint MATRIX_A = 0x9908b0df;
        protected const uint UPPER_MASK = 0x80000000;
        protected const uint LOWER_MASK = 0x7fffffff;

        public MTSeed(uint Seed)
        {
            this.Seed = Seed;
            //-----------------------------------------------------------------------------------
            stateVector[0] = Seed;
            for (uint i = 1; i < stateVector.Length; i++)
            {
                stateVector[i] = 0x6C078965 * (stateVector[i - 1] ^ (stateVector[i - 1] >> 30)) + i;
            }

            for (var k = 0; k < N - M; k++)
            {
                var temp = (stateVector[k] & UPPER_MASK) | (stateVector[k + 1] & LOWER_MASK);
                stateVector[k] = stateVector[k + M] ^ (temp >> 1);
                if ((temp & 1) == 1) stateVector[k] ^= MATRIX_A;
            }
            for (var k = N - M; k < N - 1; k++)
            {
                var temp = (stateVector[k] & UPPER_MASK) | (stateVector[k + 1] & LOWER_MASK);
                stateVector[k] = stateVector[k + (M - N)] ^ (temp >> 1);
                if ((temp & 1) == 1) stateVector[k] ^= MATRIX_A;
            }
            {
                var temp = (stateVector[N - 1] & UPPER_MASK) | (stateVector[0] & LOWER_MASK);
                stateVector[N - 1] = stateVector[M - 1] ^ (temp >> 1);
                if ((temp & 1) == 1) stateVector[N - 1] ^= MATRIX_A;
            }
            //--------------------------------------------------------------------------------------
        }
        public (ulong, ulong) UnovaRNG()
        {
            ulong val;
            ulong temp1, temp2, temp3;
            ulong MTSeed;

            val = stateVector[2];

            temp1 = (val >> 0xB) ^ val;
            temp2 = ((temp1 << 0x7) & 0x9D2C5680) ^ temp1;
            temp3 = ((temp2 << 0xF) & 0xEFC60000) ^ temp2;
            MTSeed = ((temp3 >> 0x12) ^ temp3);

            ulong Seed1 = NextSeed(MTSeed);
            ulong Seed2 = NextSeed(Seed1) >> 32;
            Seed1 =  Seed1 >> 32;

            return (Seed1, Seed2);

        }
        private ulong NextSeed(ulong Seed)
        {
            return Seed * 0x5D588B656C078965UL + 0x269EC3UL;
        }
    } 
}
