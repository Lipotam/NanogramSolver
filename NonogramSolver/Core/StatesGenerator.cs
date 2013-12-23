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
            string result = currentPossibleState.ToString();

            while (!IsPossibleForCurrentState(result) && !IsPossibleForStateCode(result))
            {
                currentPossibleState++;
                result = currentPossibleState.ToString();
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
    }
}
