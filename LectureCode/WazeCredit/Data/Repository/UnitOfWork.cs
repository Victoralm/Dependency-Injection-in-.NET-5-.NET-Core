using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICreditApplicationRepository CreditApplicationRepository { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            this._db = db;
            CreditApplicationRepository = new CreditApplicationRepository(this._db);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
