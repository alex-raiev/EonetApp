using System.Collections.Generic;

namespace EonetApp.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Source> Sources { get; set; }
        public IEnumerable<Geometry> Geometries { get; set; }
    }
}
