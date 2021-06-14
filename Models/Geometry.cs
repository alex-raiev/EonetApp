using System;
using System.Collections.Generic;

namespace EonetApp.Models
{
    public class Geometry
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        // TODO: figure out why it crashed on deserializing. Do we still need custom json converter?
        //public List<double> Coordinates { get; set; }
    }
}
