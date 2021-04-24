using System.Collections.Generic;
using System.Threading.Tasks;
using LoraWAN_Pipeline;

namespace container.Database
{
    interface IDatabase
    {
        Task Create(StubObject stubObject);

        Task<StubObject> Get(int Id);

        Task<IEnumerable<StubObject>> GetAll();

        Task Update(StubObject stubObject);

        Task Delete(StubObject stubObject);
    }
}
