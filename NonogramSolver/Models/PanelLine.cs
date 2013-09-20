using System;
using System.Collections.Generic;
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
    }
}
