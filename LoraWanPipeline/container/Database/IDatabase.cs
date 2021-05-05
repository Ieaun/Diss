namespace container.Database
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LoraWAN_Pipeline;
    interface IDatabase
    {
        Task Create(StubObject stubObject);

        Task<StubObject> Get(int Id);

        Task<List<StubObject>> GetAll();

        Task Update(StubObject stubObject);

        Task Delete(StubObject stubObject);
    }
}
