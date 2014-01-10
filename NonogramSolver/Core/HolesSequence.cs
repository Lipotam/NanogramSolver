using System.Collections.Generic;
using System.Linq;

namespace NonogramSolver.Core
{
    class HolesSequence
    {
        private int filled = 0;
        private List<int> sequence;
        private int sumTobe;
        private int maxHolesLength;

        private int GetSum()
        {
            return this.sequence.Sum();
        }

        public int GetMinAcceptable()
        {
            if (this.filled + 1 < this.maxHolesLength)
            {
                if (this.filled > 0 && this.filled < this.maxHolesLength - 1) return 1;
                return 0;
            }
            return this.sumTobe - this.GetSum();
        }

        /// <summary>
        /// </summary>
        /// <returns>max holes number on this step + 1 (comfortable for generators)</returns>
        public int GetMaxAcceptable()
        {
            int futureHoles;
            if (this.filled == 0)
            {
                futureHoles = this.maxHolesLength - 2;
            }
            else if (this.filled == this.maxHolesLength - 1)
            {
                futureHoles = 0;
            }
            else
            {
                futureHoles = this.maxHolesLength - this.filled - 2;
            }
            int max = this.sumTobe - this.GetSum() - futureHoles;
            return max + 1;
        }

        public HolesSequence(int maxHolesLength, int sumTobe, int filled = 0)
        {
            this.maxHolesLength = maxHolesLength;
            this.sumTobe = sumTobe;
            this.filled = filled;
            this.sequence = new List<int>(maxHolesLength);
        }

        public bool IsReady()
        {
            return this.filled == this.maxHolesLength;
        }

        public HolesSequence Copy()
        {
            HolesSequence a_copy = new HolesSequence(this.maxHolesLength, this.sumTobe, this.filled);
            a_copy.sequence = new List<int>(this.sequence);
            return a_copy;
        }

        public void SetNext(int value)
        {
            this.sequence.Add(value);
            this.filled++;
        }

        /// <summary>
        /// It returned list of int in python.
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns></returns>
        public List<bool> ShowWithBlocks(List<int> blocks)
        {
            List<bool> result = new List<bool>();
            for (int i = 0; i < this.filled; i++)
            {
                result.AddRange(Enumerable.Repeat(false, sequence[i]));
                if (i < blocks.Count)
                {
                    result.AddRange(Enumerable.Repeat(true, blocks[i]));
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="mask">It was a list of int in python.</param>
        /// <returns></returns>
        public bool SatisfyMask(List<int> blocks, List<bool?> mask)
        {
            List<bool> externalRepresentation = this.ShowWithBlocks(blocks);
            for (int i = 0; i < externalRepresentation.Count; i++)
            {
                if (mask[i] != null && externalRepresentation[i] != mask[i]) return false;
            }
            return true;
        }

    }
}
