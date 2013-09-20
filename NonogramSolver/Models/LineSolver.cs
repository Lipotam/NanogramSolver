using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NonogramSolver.Models
{
    public class LineSolver
    {
        public LineSolver(CellState[] linevector, List<int> numbers)
        {
            this.lineVector = linevector;
            this.numbers = numbers;
        }

        private CellState[] lineVector;
        private List<int> numbers;

        public void UpdateLineVector(CellState[] linevector)
        {
            this.lineVector = linevector;
        }

        public CellState[] GetLineVector()
        {
            return lineVector;
        }

        public void RecogniseLine()
        {




        }
    }
}
