namespace container.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LoraWAN_Pipeline;
    public interface IDatabase
    {
        Task Create(RegisteredDevice registeredDevice);

        Task<RegisteredDevice> Get(string DeviceAddress);

        Task<List<RegisteredDevice>> GetAll();

        Task Update(RegisteredDevice registeredDevice);

        Task Delete(RegisteredDevice registeredDevice);
    }
}
