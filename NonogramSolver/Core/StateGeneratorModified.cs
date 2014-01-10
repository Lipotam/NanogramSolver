using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NonogramSolver.Models;

namespace NonogramSolver.Core
{
    public class StateGeneratorModified
    {
        private List<bool?> mask = null;
        private List<int> blocks;

        private int maxHolesLength;
        private int holesSumTobe;


        private IEnumerable<HolesSequence> GetHolesSequences()
        {
            Queue<HolesSequence> workQueue = new Queue<HolesSequence>();
            workQueue.Enqueue(new HolesSequence(this.maxHolesLength, this.holesSumTobe));
            while (workQueue.Count > 0)
            {
                HolesSequence sequenceToInlarge = workQueue.Dequeue();
                if (sequenceToInlarge.IsReady())
                {
                    yield return sequenceToInlarge;
                }
                else
                {
                    for (int i = sequenceToInlarge.GetMinAcceptable(); i < sequenceToInlarge.GetMaxAcceptable(); i++)
                    {
                        HolesSequence aCopy = sequenceToInlarge.Copy();
                        aCopy.SetNext(i);
                        if (mask != null && !aCopy.SatisfyMask(blocks, mask))
                        {
                            continue;
                        }
                        workQueue.Enqueue(aCopy);
                    }
                }
            }
        }

        public IEnumerable<CellState[]> GetStatesList(PanelLine line, CellState[] states)
        {
            int lineLength = states.Count();
            if (states != null)
            {
                this.mask = new List<bool?>(states.Count());
                for (int i = 0; i < states.Count(); i++)
                {
                    switch (states[i])
                    {
                        case CellState.Empty:
                            this.mask.Add(false);
                            break;
                        case CellState.Filled:
                            this.mask.Add(true);
                            break;
                        default:
                            this.mask.Add(null);
                            break;
                    }
                }
            }

            this.maxHolesLength = line.LineValues.Count + 1;
            this.holesSumTobe = lineLength - line.GetSum();
            this.blocks = line.LineValues;

            foreach (HolesSequence sequence in this.GetHolesSequences())
            {
                List<bool> a = sequence.ShowWithBlocks(blocks);
                CellState[] result = new CellState[lineLength];
                for (int i = 0; i < lineLength; i++)
                {
                    result[i] = a[i] ? CellState.Filled : CellState.Empty;
                }

                yield return result;
            }
        }
    }
}
