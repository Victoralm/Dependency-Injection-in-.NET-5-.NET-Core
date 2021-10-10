using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public class CreditApplicationRepository : Repository<CreditApplication>, ICreditApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public CreditApplicationRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        // Update method is implemented here since isn't a good practice to implement it on the Repository

        public void Update(CreditApplication obj)
        {
            this._db.CreditApplicationModel.Update(obj);
        }
    }
}
