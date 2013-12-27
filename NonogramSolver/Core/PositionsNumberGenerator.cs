using System.Numerics;

namespace NonogramSolver.Core
{
    class PositionsNumberGenerator
    {
        public BigInteger PositionNumber(int count, int summ, int width)
        {
            if (count == 1)
            {
                return width - summ - count + 2;
            }
            else
            {
                BigInteger result = 0;

                for (int i = 1; i < width - summ - count + 3; i++)
                {
                    result += PositionNumber(count-1, summ, width-i);
                }
                return result;
            }
        }
    }
}
