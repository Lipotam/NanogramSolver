using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NonogramSolver.Models
{
    class CrosswordData
    {
        public CrosswordData(int width, int height)
        {
            if (fieldWidth < 1 || fieldHeight < 1)
            {
                throw new ArgumentException("Dimentions can't be less then 1.");
            }

            fieldWidth = width;
            fieldHeight = height;
            field = new CellStates[fieldWidth, fieldHeight];
            leftPanel = new List<int>[fieldHeight];
            topPanel = new List<int>[fieldWidth];
        }

        private readonly int fieldWidth;
        private int fieldHeight;
        private CellStates[,] field;
        private List<int>[] leftPanel;
        private List<int>[] topPanel;

        public CellStates[,] FieldCells
        {
            get
            {
                return field;
            }
        }

        public List<int>[] TopPanel
        {
            get
            {
                return topPanel;
            }
        }

        public List<int>[] LeftPanel
        {
            get
            {
                return leftPanel;
            }
        }

        public void AddNumberRow(int y, List<int> row)
        {
            if (y < 0 || y > fieldHeight || row == null)
            {
                throw new ArgumentException("Wrong coordinates or no data.");
            }

            leftPanel[y] = row;
        }

        public void AddNumberColumn(int x, List<int> column)
        {
            if (x < 0 || x > fieldHeight || column == null)
            {
                throw new ArgumentException("Wrong coordinates or no data.");
            }

            topPanel[x] = column;
        }

        public void FillCell(int x, int y)
        {
            if (x >= 0 && x < fieldWidth && y >= 0 && y < fieldHeight)
            {
                field[x, y] = CellStates.Filled;
            }
            else
            {
                throw new ArgumentException("Wrong coordinates.");
            }
        }

        public void EmptyCell(int x, int y)
        {
            if (x >= 0 && x < fieldWidth && y >= 0 && y < fieldHeight)
            {
                field[x, y] = CellStates.Empty;
            }
            else
            {
                throw new ArgumentException("Wrong coordinates.");
            }
        }
    }
}
