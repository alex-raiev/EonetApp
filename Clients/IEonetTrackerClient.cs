using System.Threading.Tasks;

namespace EonetApp.Clients
{
    using Models;

    public interface IEonetTrackerClient
    {
        Task<EventList> GetAllEvents();
    }
}
