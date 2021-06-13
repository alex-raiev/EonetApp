using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EonetApp.Models;

namespace EonetApp.Clients
{
    public interface IEonetTrackerClient
    {
        Task<EventList> GetAllEvents();
    }
}
