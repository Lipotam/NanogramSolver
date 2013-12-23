using System;
using System.Data;
using System.Linq;

namespace NonogramSolver.Models
{
    public class SolverModel
    {
        #region Private members
        private readonly int fieldWidth;
        private readonly int fieldHeight;
        private readonly CellState[][] fieldCells;
        private readonly bool[] linesChanged, columnsChanged;

        #endregion

        #region Getters and Setters
        public bool[] LinesChanged
        {
            get
            {
                return linesChanged;
            }
        }

        public bool[] ColumnsChanged
        {
            get
            {
                return columnsChanged;
            }
        }

        public int FieldWidth
        {
            get
            {
                return fieldWidth;
            }
        }

        public int FieldHeight
        {
            get
            {
                return fieldHeight;
            }
        }

        public CellState[][] Matrix
        {
            get
            {
                return fieldCells;
            }
        }

        #endregion

        #region Initialization

        public SolverModel(CrosswordData crosswordData)
        {
            CheckData(crosswordData);

            fieldHeight = crosswordData.FieldHeight;
            fieldWidth = crosswordData.FieldWidth;
            if (crosswordData.FieldCells != null)
            {
                fieldCells = crosswordData.FieldCells;
            }
            else
            {
                fieldCells = new CellState[fieldWidth][];
                for (int i = 0; i < fieldWidth; i++)
                {
                    fieldCells[i] = new CellState[fieldHeight];
                    for (int j = 0; j < fieldHeight; j++)
                    {
                        fieldCells[i][j] = CellState.Undefined;
                    }
                }
            }

            linesChanged = new bool[fieldHeight];
            columnsChanged = new bool[fieldWidth];
        }

        private void CheckData(CrosswordData crosswordData)
        {
            if (crosswordData.FieldWidth == 0 || crosswordData.FieldHeight == 0 || crosswordData.TopPanelLines == null || crosswordData.LeftPanelLines == null || crosswordData.LeftPanelLines.Count() != crosswordData.FieldHeight || crosswordData.TopPanelLines.Count() != crosswordData.FieldWidth)
            {
                throw new DataException("Bad input parameters for solving");
            }

            int width = crosswordData.FieldWidth;
            int height = crosswordData.FieldHeight;

            int solidElementsFromLeft = 0;
            int solidElementsFromTop = 0;

            foreach (var column in crosswordData.TopPanelLines)
            {
                if (column.GetMinDistance() > height)
                {
                    throw new DataException(String.Format("Bad input parameters for solving, column {0} has height {1} more than field height {2}", Array.IndexOf(crosswordData.TopPanelLines, column), column.GetMinDistance(), height));
                }
                solidElementsFromTop += column.GetSum();
            }

            foreach (var line in crosswordData.LeftPanelLines)
            {
                if (line.GetMinDistance() > width)
                {
                    throw new DataException(String.Format("Bad input parameters for solving, line {0} has width {1} more than field width {2}", Array.IndexOf(crosswordData.LeftPanelLines, line), line.GetMinDistance(), width));
                }
                solidElementsFromLeft += line.GetSum();
            }

            if (solidElementsFromTop != solidElementsFromLeft)
            {
                throw new DataException("Bad input parameters for solving, summs for top and left panels have difference");
            }

            if (crosswordData.FieldCells != null)
            {
                if (crosswordData.FieldCells.SelectMany(line => line).Any(cell => cell != CellState.Empty || cell != CellState.Filled))
                {
                    throw new DataException("Bad input parameters for solving, field has illegal value");
                }
            }
        }

        #endregion


        #region Line and Column Changes
        public void ResetColumnChangedMarkers()
        {
            for (int i = 0; i < fieldWidth; i++)
            {
                columnsChanged[i] = false;
            }
        }

        public void ResetLineChangedMarkers()
        {
            for (int i = 0; i < fieldHeight; i++)
            {
                linesChanged[i] = false;
            }
        }

        public void SetChangesForFirstSearch()
        {
            for (int i = 0; i < fieldWidth; i++)
            {
                columnsChanged[i] = true;
            }
            for (int i = 0; i < fieldHeight; i++)
            {
                linesChanged[i] = true;
            }
        }

        public bool HasChanges()
        {
            for (int i = 0; i < fieldWidth; i++)
            {
                if (columnsChanged[i])
                {
                    return true;
                }
            }
            for (int i = 0; i < fieldHeight; i++)
            {
                if (linesChanged[i])
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
        #region lines and columns elements getter and setter
        public CellState[] GetLine(int index)
        {
            CellState[] result = new CellState[fieldWidth];
            for (int i = 0; i < fieldWidth; i++)
            {
                result[i] = fieldCells[i][index];
            }

            return result;
        }

        public void SaveLine(int index, CellState[] lineElements)
        {
            for (int i = 0; i < fieldWidth; i++)
            {
                CheckElementStateForOutput(lineElements[i]);
                if (fieldCells[i][index] != lineElements[i])
                {
                    fieldCells[i][index] = lineElements[i];
                    columnsChanged[i] = true;
                }
            }
        }

        public CellState[] GetColumn(int index)
        {
            CellState[] result = new CellState[fieldHeight];
            for (int i = 0; i < fieldHeight; i++)
            {
                result[i] = fieldCells[index][i];
            }

            return result;
        }

        public void SaveColumn(int index, CellState[] columnElements)
        {
            for (int i = 0; i < fieldHeight; i++)
            {
                CheckElementStateForOutput(columnElements[i]);
                if (fieldCells[index][i] != columnElements[i])
                {
                    fieldCells[index][i] = columnElements[i];
                    linesChanged[i] = true;
                }
            }
        }

        private static void CheckElementStateForOutput(CellState element)
        {
            if (!(element == CellState.Empty || element == CellState.Filled || element == CellState.Undefined))
            {
                throw new Exception("Solver returned bad element");
            }
        }
        #endregion
    }
}
