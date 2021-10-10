using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICreditApplicationRepository CreditApplication { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            this._db = db;
            CreditApplication = new CreditApplicationRepository(this._db);
        }

        public void Dispose()
        {
            this._db.Dispose();
        }

        public void Save()
        {
            this._db.SaveChanges();
        }
    }
}
