using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NonogramSolver.Models
{
    public class SolverModel
    {
        public SolverModel(CrosswordData crosswordData)
        {
            CheckData(crosswordData);

            fieldHeight = crosswordData.FieldHeight;
            fieldWidth = crosswordData.FieldWidth;

        }

        private int fieldWidth;
        private int fieldHeight;



        private static void CheckData(CrosswordData crosswordData)
        {
            if (crosswordData.FieldWidth == 0 || crosswordData.FieldHeight == 0 || crosswordData.TopPanel == null || crosswordData.LeftPanel == null || crosswordData.LeftPanel.Count != crosswordData.FieldHeight || crosswordData.TopPanel.Count != crosswordData.FieldWidth)
            {
                throw new DataException("Bad input parameters for solving");
            }

            int width = crosswordData.FieldWidth;
            int height = crosswordData.FieldHeight;

            int solidElementsFromLeft = 0;
            int solidElementsFromTop = 0;

            foreach (var column in crosswordData.TopPanel)
            {
                if (column.GetMinDistance() < height)
                {
                    throw new DataException(String.Format("Bad input parameters for solving, column {0} has height {1} more than field height {2}", crosswordData.TopPanel.IndexOf(column), column.GetMinDistance(), height));
                }
                solidElementsFromTop += column.GetSum();
            }

            foreach (var line in crosswordData.LeftPanel)
            {
                if (line.GetMinDistance() < width)
                {
                    throw new DataException(String.Format("Bad input parameters for solving, line {0} has width {1} more than field width {2}", crosswordData.LeftPanel.IndexOf(line), line.GetMinDistance(), width));
                }
                solidElementsFromLeft += line.GetSum();
            }

            if (solidElementsFromTop != solidElementsFromLeft)
            {
                throw new DataException("Bad input parameters for solving, summs for top and left panels have difference");
            }

            if (crosswordData.FieldCells != null)
            {
                if(crosswordData.FieldCells.SelectMany(line => line).Any(cell => cell != CellState.Empty || cell != CellState.Filled))
                    {
                    throw new DataException("Bad input parameters for solving, field has illegal value");
                    }
            }
        }
    }
}
