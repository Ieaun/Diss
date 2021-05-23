namespace UplinkService.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UplinkService.Models.Servers;

    public interface IDatabase
    {
        Task Create(Server Server);

        Task<Server> Get(int Id);

        Task<List<Server>> GetAll();

        Task Update(Server stubObject);

        Task Delete(Server stubObject);
    }
}
