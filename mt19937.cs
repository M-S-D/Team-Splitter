using System;
namespace SplitTeams
{
    /// <summary>
    /// This code was posted by arithma http://www.lebgeeks.com/forums/viewtopic.php?pid=60482#p60482
    /// </summary>
    public class mt19937
    {
        /* Period parameters */
        private const int N = 312;
        private const int M = 156;
        private const ulong MATRIX_A = 0xB5026F5AA96619E9; /* constant vector a */
        private const ulong UPPER_MASK = 0xFFFFFFFF80000000; /* most significant w-r bits */
        private const ulong LOWER_MASK = 0x7FFFFFFF; /* least significant r bits */

        /* Tempering parameters */
        private const ulong TEMPERING_MASK_B = 0x9d2c5680;
        private const ulong TEMPERING_MASK_C = 0xefc60000;

        private static ulong TEMPERING_SHIFT_U(ulong y) { return (y >> 29); }
        private static ulong TEMPERING_SHIFT_S(ulong y) { return (y << 17); }
        private static ulong TEMPERING_SHIFT_T(ulong y) { return (y << 37); }
        private static ulong TEMPERING_SHIFT_L(ulong y) { return (y >> 43); }

        //static unsigned long mt[N]; /* the array for the state vector  */
        private static ulong[] mt = new ulong[N];

        // static int mti=N+1; /* mti==N+1 means mt[N] is not initialized */
        private static uint mti = N + 1; /* mti==N+1 means mt[N] is not initialized */

        /* initializing the array with a NONZERO seed */
        public static void sgenrand(ulong seed)
        {
            /* setting initial seeds to mt[N] using         */
            /* the generator Line 25 of Table 1 in          */
            /* [KNUTH 1981, The Art of Computer Programming */
            /*    Vol. 2 (2nd Ed.), pp102]                  */

            mt[0] = seed;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = (6364136223846793005 * (mt[mti - 1] ^ (mt[mti - 1] >> 62)) + mti);
            }
        }

        private static ulong[/* 2 */] mag01 = { 0x0, MATRIX_A };
        /* generating reals */
        /* unsigned long */
        /* for integer generation */
        public static ulong genrand()
        {
            ulong y;

            /* mag01[x] = x * MATRIX_A  for x=0,1 */
            if (mti >= N) /* generate N words at one time */
            {
                short kk;

                if (mti == N + 1) /* if sgenrand() has not been called, */
                {
                    sgenrand(5489); /* a default initial seed is used   */
                }

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];

                mti = 0;
            }

            y = mt[mti++];
            y ^= (y >> 29) & 0x5555555555555555;
            y ^= (y << 17) & 0x71D67FFFEDA60000;
            y ^= (y << 37) & 0xFFF7EEE000000000;
            y ^= (y >> 43);

            return y;
        }
    }
}