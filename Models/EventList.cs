using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EonetApp.Models
{
    
    public class EventList
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IQueryable<Event> Events { get; set; }
    }
}
