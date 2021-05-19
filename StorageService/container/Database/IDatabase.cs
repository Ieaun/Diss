namespace StorageService.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StorageService;
    using StorageService.Notifications.ReceivedPackets;

    public interface IDatabase
    {
        Task Create(NewPacket stubObject);

        Task<NewPacket> Get(int Id);

        Task<List<NewPacket>> GetAll();

        Task Update(NewPacket stubObject);

        Task Delete(NewPacket stubObject);
    }
}
