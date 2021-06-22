using System.Collections.Generic;
using System.Threading.Tasks;

namespace EonetApp.Services
{
    using Models;

    public interface IEonetService
    {
        Task<IEnumerable<Event>> GetAll();
        Task<Event> GetById(string id);
    }
}
