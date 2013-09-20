using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NonogramSolver.Models
{
    [Serializable]
    public class CrosswordData
    {
        [XmlElement("FieldWidth")]
        public int FieldWidth { get; set; }

        [XmlElement("FieldHeight")]
        public int FieldHeight { get; set; }

        [XmlArrayItem("Columns")]
        public List<PanelLine> TopPanel { get; set; }

        [XmlArrayItem("Lines")]
        public List<PanelLine> LeftPanel { get; set; }

        [XmlArrayItem("FieldRow")]
        public CellState[][] FieldCells { get; set; }


        public void AddNumberRow(int y, List<int> rowValues)
        {
            if (y < 0 || y > FieldHeight || rowValues == null)
            {
                throw new ArgumentException("Wrong coordinates or no data.");
            }

            LeftPanel[y].LineValues = rowValues;
        }

        public void AddNumberColumn(int x, List<int> columnValues)
        {
            if (x < 0 || x > FieldHeight || columnValues == null)
            {
                throw new ArgumentException("Wrong coordinates or no data.");
            }

            TopPanel[x].LineValues = columnValues;
        }

        public void FillCell(int x, int y)
        {
            if (x >= 0 && x < FieldWidth && y >= 0 && y < FieldHeight)
            {
                FieldCells[x][y] = CellState.Filled;
            }
            else
            {
                throw new ArgumentException("Wrong coordinates.");
            }
        }

        public void EmptyCell(int x, int y)
        {
            if (x >= 0 && x < FieldWidth && y >= 0 && y < FieldHeight)
            {
                FieldCells[x][y] = CellState.Empty;
            }
            else
            {
                throw new ArgumentException("Wrong coordinates.");
            }
        }

        public bool IsValid()
        {
            if (FieldCells.GetLength(0) == FieldWidth - 1 && FieldCells.GetLength(2) == FieldHeight - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
