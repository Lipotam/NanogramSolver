using System;
using System.Collections.Generic;
using System.Numerics;
using NonogramSolver.Models;

namespace NonogramSolver.Core
{
    class StatesGenerator
    {
        public StatesGenerator(CellState[] initialCells, PanelLine numbers)
        {
            initialCellStates = initialCells;
            initialState = "";
            foreach (CellState cellState in initialCellStates)
            {
                switch (cellState)
                {
                    case CellState.Empty:
                        initialState += "0";
                        break;
                    case CellState.Filled:
                        initialState += "1";
                        break;
                    case CellState.Undefined:
                        initialState += "u";
                        break;
                    default: throw new Exception("Invalid generator initialization");
                }
            }

            currentPossibleState = 0;
            length = initialCellStates.Length;
            numbersForState = numbers;
        }

        private CellState[] initialCellStates;
        private string initialState;
        private BigInteger currentPossibleState;
        private int length;
        private PanelLine numbersForState;

        public CellState[] GetNextState()
        {
            string result = calcBinary(currentPossibleState, length);

            while (!IsPossibleForCurrentState(result) && !IsPossibleForStateCode(result))
            {
                currentPossibleState++;
                result = calcBinary(currentPossibleState, length);
            }

            CellState[] output = new CellState[length];
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == '0')
                {
                    output[i] = CellState.Empty;
                }
                else
                {
                    output[i] = CellState.Filled;
                }
            }

            return output;
        }

        private bool IsPossibleForCurrentState(string possibleState)
        {
            for (int i = 0; i < initialState.Length; i++)
            {
                if (initialState[i] == '0' && possibleState[i] == '1')
                {
                    return false;
                }

                if (initialState[i] == '1' && possibleState[i] == '0')
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsPossibleForStateCode(string possibleState)
        {
            List<int> possibleStateCode = new List<int>();
            int currentCodeValue = 0;

            for (int i = 0; i < possibleState.Length; i++)
            {
                if (possibleState[i] == '1')
                {
                    currentCodeValue++;
                }
                if (possibleState[i] == '1' && possibleState[i + 1] == '0')
                {
                    possibleStateCode.Add(currentCodeValue);
                    currentCodeValue = 0;
                }
            }

            for (int i = 0; i < numbersForState.LineValues.Count; i++)
            {
                if (numbersForState.LineValues[i] != possibleStateCode[i])
                {
                    return false;
                }
            }

            return true;
        }

        private string calcBinary(BigInteger tee, int lengthForBinary)
        {

            BigInteger t = tee;
            int len = lengthForBinary;

            string fullnum = "";
            for (int i = len; i >= 0; i--)
            {
                int inum;

                BigInteger num = (t >> i) & 1;
                if (num == 1)
                {
                    inum = 1;
                }
                else if (num == 0)
                {
                    inum = 0;
                }
                else
                {
                    inum = 666;
                }
                fullnum = fullnum + Convert.ToString(inum);

            }
            return fullnum;
        }
    }
}

