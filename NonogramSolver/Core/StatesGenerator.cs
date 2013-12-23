using System;
using NonogramSolver.Models;

namespace NonogramSolver.Core
{
    class StatesGenerator
    {
        public StatesGenerator(CellState[] initialCells, PanelLine numbers)
        {
            initialCellStates = initialCells;
        }

        private CellState[] initialCellStates;

        public CellState[] GetNextState()
        {

            throw new NotImplementedException();
        }
    }
}
