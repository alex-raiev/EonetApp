using System.Threading.Tasks;
using EonetApp.Models;

namespace EonetApp.Services
{
    public interface IEonetService
    {
        Task<EventList> GetAll();
        Task<Event> GetById(string id);
    }
}
