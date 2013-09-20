using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NonogramSolver.Models
{
    [Serializable]
    public class PanelLine
    {
        public PanelLine()
        {
            LineValues = new List<int>();
        }

        [XmlArrayItem("Number")]
        public List<int> LineValues { get; set; }

        public int GetSum()
        {
            return LineValues.Sum();
        }

        public int GetMinDistance()
        {
            return LineValues.Sum() + LineValues.Count - 1;
        }
    }
}
