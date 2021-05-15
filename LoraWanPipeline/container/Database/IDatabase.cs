namespace container.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LoraWAN_Pipeline;
    public interface IDatabase
    {
        Task Create(RegisteredDevice stubObject);

        Task<RegisteredDevice> Get(int Id);

        Task<List<RegisteredDevice>> GetAll();

        Task Update(RegisteredDevice stubObject);

        Task Delete(RegisteredDevice stubObject);
    }
}
