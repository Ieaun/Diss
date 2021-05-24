namespace StorageService.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StorageService.Notifications.ReceivedPackets;

    public interface IDatabase
    {
        Task Create(NewPacket packet);

        Task<NewPacket> Get(string deviceAddress);

        Task<List<NewPacket>> GetAll();

        Task Delete(NewPacket packet);
    }
}
