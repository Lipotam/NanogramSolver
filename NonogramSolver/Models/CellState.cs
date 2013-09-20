using System;
using System.Xml.Serialization;

namespace NonogramSolver.Models
{
    [Serializable]
    public enum CellState
    {
        [XmlEnum(Name = "Undefined")]
        Undefined = 0,
        [XmlEnum(Name = "Empty")]
        Empty = 1,
        [XmlEnum(Name = "Filled")]
        Filled = 2,
        [XmlEnum(Name = "None")]
        None = 3
    }
}